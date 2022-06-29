using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Store.MailSender;
using Store.MailSender.MailKit;
using Store.NotificatorBackgroundService.Options;
using System.Diagnostics;

namespace Store.NotificatorBackgroundService
{
    public class NotificatorBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<NotificatorBackgroundService> _logger;
        private readonly OptionsNotificator _options;

        public NotificatorBackgroundService(IServiceScopeFactory serviceScopeFactory, 
            ILogger<NotificatorBackgroundService> logger,
            IOptions<OptionsNotificator> options)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var policy = Policy.Handle<Exception>()
                .RetryAsync(3, (e, retryCount) => _logger.LogWarning("Error while sending email. Retrying: {Attempt}", retryCount));
            
            using var timer = new PeriodicTimer(_options.TimeRepeat);
            Stopwatch sw = Stopwatch.StartNew();
            
            while(await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Cервер работает уже {WorkTime}", sw.Elapsed);
                
                using var scope = _serviceScopeFactory.CreateAsyncScope();

                var mailSender = scope.ServiceProvider.GetRequiredService<IMailSender<MessageData>>();

                var policyRes = await policy.ExecuteAndCaptureAsync(async cancelToken => {

                    await mailSender.Send(new MessageData
                    {
                        Subject = "Уведомление от сервера.",
                        Message = $"Сервер работает уже: {sw.Elapsed}"
                    }, cancelToken);

                }, stoppingToken);

                if (policyRes.Outcome == OutcomeType.Failure)
                    _logger.LogError(policyRes.FinalException, "There was an error while sending email.");
                else
                    _logger.LogInformation("Email sent.");
            }

            //stoppingToken.ThrowIfCancellationRequested();
        }
    }
}