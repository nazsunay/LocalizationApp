using LocalizationApp.Models;
using LocalizationApp.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LocalizationApp.Controllers
{
    public class HomeController : Controller
    {
        //Uygulama içinde hata ayýklama, bilgi kaydetme gibi iþlemler için kullanýlýr.
        private readonly ILogger<HomeController> _logger;
        //servise ulaþým saðlar
        private readonly LanguageService _localization;

        // Desteklenen kültürler listesi ( Ýtalyanca yok )
        private static readonly List<string> _supportedCultures = new List<string> { "en-US", "tr-TR", "fr-FR" };

        public HomeController(ILogger<HomeController> logger, LanguageService localization)
        {
            _logger = logger;
            _localization = localization;
        }

        public IActionResult Index()
        {
            // Localization ile "Welcome" anahtarýný alýp ViewBag'e atýyoruz
            ViewBag.Welcome = _localization.GetKey("Welcome").Value;

            //  geçerli kültürü alýyoruz dil bilgisi culture içinde saklanýr ve ulaþým saðlanýr
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            return View();
        }

        public IActionResult ChangeLanguage(string culture)
        {
            // Kültür geçerli mi kontrolü
            if (string.IsNullOrEmpty(culture) || !_supportedCultures.Contains(culture))
            {

                TempData["Error"] = "Geçersiz dil seçimi!";
                return RedirectToAction("Index");
            }

            // Geçerli kültür için cookie ayarý
            //eçerli kültürü saklamak için bir çerez (cookie) oluþturur. Bu çerez, kullanýcýnýn seçtiði kültür bilgisini tutar.
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });

            // Kullanýcýyý önceki sayfaya yönlendiriyoruz
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}