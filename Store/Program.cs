using Microsoft.Extensions.Options;
using Repository.DataInConcurrentDictionary;
using Repository.DataInConcurrentDictionary.TestData.Extensions;
using Repository.Interfaces;
using Serilog;
using Store.AutoMapperProfiles;
using Store.MailSender;
using Store.MailSender.MailKit;
using Store.MailSender.MailKit.Options;
using Store.NotificatorBackgroundService;
using Store.NotificatorBackgroundService.Options;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up.");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((_, conf) => conf.WriteTo.Console());

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<AutoMapperProfile>();
    });
    builder.Services.AddTestProductsData();
    builder.Services.AddSingleton<IOptions<IOptionMailSender>>(Options.Create(builder.Configuration.GetSection("EmailSetting").Get<OptionMailSender>()));
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IMailSender<MessageData>, MailSender>();

    builder.Services.Configure<OptionsNotificator>(builder.Configuration.GetSection("NotificatorBackgroundServiceSetting"));
    builder.Services.AddHostedService<NotificatorBackgroundService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    //app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch(Exception e)
{
    Log.Fatal(e, "Fatal server error.");
}
finally
{
    Log.Information("Server shut down.");
    Log.CloseAndFlush();
}