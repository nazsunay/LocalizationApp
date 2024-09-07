using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Resources;

namespace LocalizationApp.Services
{
    public class LanguageService
    {
        public class SharedResource
        {

        }
        //type.GetTypeInfo().Assembly ifadesi, SharedResource sınıfının bulunduğu derlemeyi (assembly) alır.
        //FullName, derlemenin tam adını(isim, versiyon, kültür vb.) döndürür.
        //new AssemblyName(...), bu tam adı alarak yeni bir AssemblyName nesnesi oluşturur.Bu nesne, derlemenin adını ve diğer bilgilerini içerir.

        private readonly IStringLocalizer _localizer;
        public LanguageService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);

            _localizer = factory.Create(nameof(SharedResource),assemblyName.Name);

        }
        //factory.Create(...), IStringLocalizer nesnesini oluşturmak için kullanılır.
        //nameof(SharedResource), kaynak dosyasının adını belirtir(örneğin, Resources).
        //assemblyName.Name, SharedResource sınıfının bulunduğu derlemenin adını belirtir.
        //Bu iki bilgi bir araya gelerek, yerelleştirilmiş metinleri almak için gerekli olan _localizer nesnesini oluşturur.
        public LocalizedString GetKey(string key)
        {
            return _localizer[key];
        }
        //Bu metot, bir anahtar (key) alır ve _localizer aracılığıyla bu anahtara karşılık gelen yerelleştirilmiş metni döndürür.
       // LocalizedString, yerelleştirilmiş metin ve ilgili bilgileri(örneğin, anahtar) içeren bir nesnedir.
        //Örneğin, GetKey("Hello") çağrıldığında, _localizer["Hello"] ifadesi, "Hello" anahtarına karşılık gelen yerelleştirilmiş metni döndürür.
    }
}
