using LocalizationApp.Models;
using LocalizationApp.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LocalizationApp.Controllers
{
    public class HomeController : Controller
    {
        //Uygulama i�inde hata ay�klama, bilgi kaydetme gibi i�lemler i�in kullan�l�r.
        private readonly ILogger<HomeController> _logger;
        //servise ula��m sa�lar
        private readonly LanguageService _localization;

        // Desteklenen k�lt�rler listesi ( �talyanca yok )
        private static readonly List<string> _supportedCultures = new List<string> { "en-US", "tr-TR", "fr-FR" };

        public HomeController(ILogger<HomeController> logger, LanguageService localization)
        {
            _logger = logger;
            _localization = localization;
        }

        public IActionResult Index()
        {
            // Localization ile "Welcome" anahtar�n� al�p ViewBag'e at�yoruz
            ViewBag.Welcome = _localization.GetKey("Welcome").Value;

            //  ge�erli k�lt�r� al�yoruz dil bilgisi culture i�inde saklan�r ve ula��m sa�lan�r
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            return View();
        }

        public IActionResult ChangeLanguage(string culture)
        {
            // K�lt�r ge�erli mi kontrol�
            if (string.IsNullOrEmpty(culture) || !_supportedCultures.Contains(culture))
            {

                TempData["Error"] = "Ge�ersiz dil se�imi!";
                return RedirectToAction("Index");
            }

            // Ge�erli k�lt�r i�in cookie ayar�
            //e�erli k�lt�r� saklamak i�in bir �erez (cookie) olu�turur. Bu �erez, kullan�c�n�n se�ti�i k�lt�r bilgisini tutar.
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });

            // Kullan�c�y� �nceki sayfaya y�nlendiriyoruz
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}