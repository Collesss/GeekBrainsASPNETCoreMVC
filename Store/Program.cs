using Microsoft.Extensions.Options;
using Repository.DataInConcurrentDictionary;
using Repository.DataInConcurrentDictionary.TestData.Extensions;
using Repository.Interfaces;
using Store.AutoMapperProfiles;
using Store.MailSender;
using Store.MailSender.MailKit;
using Store.MailSender.MailKit.Options;

var builder = WebApplication.CreateBuilder(args);

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
