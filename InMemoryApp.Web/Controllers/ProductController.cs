using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {

            if (_memoryCache.TryGetValue("callback", out string callbackMessage))
            {
                ViewBag.CallbackMessage = callbackMessage;
            }

            //"ProductName" cache'de yoksa ilk kez oluştur
            if (!_memoryCache.TryGetValue("ProductName", out string productName))
            {
                productName = "Laptop";

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

                // Veri silindiğinde çalışacak metot
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    // Burası süre dolduğunda arka planda çalışır
                    _memoryCache.Set("callback", $"{key} anahtarlı veri ({value}) bellekten silindi. Sebep: {reason}");
                });

                _memoryCache.Set("ProductName", productName, options);
            }

            ViewBag.ProductName = productName;
            ViewBag.CallbackMessage = callbackMessage;

            return View();


            //2.yol   out: bir methodda birden fazla değer dönebilmek için kullanılır.
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("ProductName")))
            //{

            //    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            //    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30); //cache de 30 sn tutacak

            //    options.SlidingExpiration = TimeSpan.FromSeconds(10); //cache de 10 sn boyunca kullanılmazsa silinecek

            //    //options.Priority = CacheItemPriority.High;//High: bu data önemli silme low:memory dolarsa sil neverRomeve: memory dolsada silme


            //    //Bir datanın hangi sebepten dolayı memory den silindiğini söyler
            //    options.RegisterPostEvictionCallback((key, value, reason, state) =>
            //    {
            //        _memoryCache.Set("callback", $"{key} => {value} => Sebep: {reason}");
            //    });


            //_memoryCache.TryGetValue("ProductName", out string productName3);

            //productName3 = _memoryCache.Get<string>("ProductName");
            //ViewBag.productName3 = productName3;


        }


            //cache de bişey alamazsa oluşturur
            //_memoryCache.GetOrCreate<string>("ProductName", entry =>
            //{
            //    //cache de 10 sn tutacak
            //    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
            //    return "Macbook";
            //});
        }

    }

