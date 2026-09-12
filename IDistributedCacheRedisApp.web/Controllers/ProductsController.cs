using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace IDistributedCacheRedisApp.web.Controllers
{
    public class ProductsController : Controller
    {

        private IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public IActionResult Index()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(1));//memory cache deki data 1 dakika sonra silinecek.

            //_distributedCache.SetString("name", "Redis Cache", options);

            //-----------------------------------------------------------------

            //buda class üzerinden eklemek için
            string jsonProduct = JsonSerializer.Serialize(new Models.Product()
            {
                Id = 1,
                Name = "Kalem",
                Price = 100
            });

            _distributedCache.SetString("product", jsonProduct, options);

            return View();
        }

        public IActionResult Show()
        {
            //string name = _distributedCache.GetString("name");

            //ViewBag.name = name;

            string jsonProduct = _distributedCache.GetString("product");

            return View();
        }

        public IActionResult Remove()
        {
            //_distributedCache.Remove("name");
            _distributedCache.Remove("product");

            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/car.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image", imageByte);
            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] bytes = _distributedCache.Get("image");


            return File(bytes, "image/jpeg");
        }
    }
}
