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
            builder.Services.AddSingleton<LanguageService>();//tek bir language servisini kullan�r
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");//.resx dosyalar�) bulundu�u dizini belirtir
            //yazm�� oldu�umuz languga servisinin veri do�rulama i�lemi yap�l�r dataAnotation
            builder.Services.AddMvc().AddMvcLocalization().AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider=(type, factory) =>
            {
                var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                return factory.Create(nameof(SharedResource),assemblyName.Name);
            });
            //RequestLocalizationOptions, gelen isteklere g�re yerelle�tirme ayarlar�n� y�netir.
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {//Desteklenen k�lt�rlerin (dillerin) bir listesini tan�mlar. Burada �ngilizce (ABD), Frans�zca (Fransa) ve T�rk�e (T�rkiye) k�lt�rleri eklenmi�tir.
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
            //Uygulaman�n yerelle�tirmelarini  etkinle�tirir.
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
