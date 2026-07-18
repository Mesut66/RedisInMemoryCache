using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

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
            //1.yol
            if (string.IsNullOrEmpty(_memoryCache.Get<string>("ProductName")))
            {
                //Get ve Set üzerinden memoriye bir data kaydetme ve okuma işlemi yapabiliriz.
                _memoryCache.Set("ProductName", "Laptop");
                var productName = _memoryCache.Get<string>("ProductName");
                ViewBag.ProductName = productName;
            }

            //2.yol   out: bir methodda birden fazla değer dönebilmek için kullanılır.
            if (!_memoryCache.TryGetValue("ProductName", out string productName2))
            {
                _memoryCache.Set("ProductName", "Laptop");
                productName2 = _memoryCache.Get<string>("ProductName");
                ViewBag.ProductName2 = productName2;
            }

            //cache de bişey alamazsa oluşturur
            _memoryCache.GetOrCreate<string>("ProductName", entry =>
            {
                //cache de 10 sn tutacak
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return "Macbook";
            });


            return View();
        }

    }
}
