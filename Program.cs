using LocalizationApp.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using static LocalizationApp.Services.LanguageService;

namespace LocalizationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Localizer
            builder.Services.AddSingleton<LanguageService>();//tek bir language servisini kullanýr
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");//.resx dosyalarý) bulunduðu dizini belirtir
            //yazmýþ olduðumuz languga servisinin veri doðrulama iþlemi yapýlýr dataAnotation
            builder.Services.AddMvc().AddMvcLocalization().AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider=(type, factory) =>
            {
                var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                return factory.Create(nameof(SharedResource),assemblyName.Name);
            });
            //RequestLocalizationOptions, gelen isteklere göre yerelleþtirme ayarlarýný yönetir.
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {//Desteklenen kültürlerin (dillerin) bir listesini tanýmlar. Burada Ýngilizce (ABD), Fransýzca (Fransa) ve Türkçe (Türkiye) kültürleri eklenmiþtir.
                var supportCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("fr-Fr"),
                    new CultureInfo("tr-Tr"),
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "tr-Tr", uiCulture: "tr-Tr");
                options.SupportedCultures = supportCultures;
                options.SupportedUICultures = supportCultures;
                options.RequestCultureProviders.Insert(0,new QueryStringRequestCultureProvider());

            });
            #endregion

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
            //Uygulamanýn yerelleþtirmelarini  etkinleþtirir.
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
