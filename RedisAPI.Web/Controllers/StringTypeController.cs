using Microsoft.AspNetCore.Mvc;
using RedisAPI.Web.Services;
using StackExchange.Redis;

namespace RedisAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDatabase(0);
        }
        public IActionResult Index()
        {
            db.StringSet("mykey", "Hello, Redis!");
            db.StringSet("mykey2", "Hello, Redis2!");
            return View();
        }

        public IActionResult GetValue()
        {
            var value = db.StringGet("mykey");
            var value2 = db.StringGet("mykey2");
            ViewBag.Value = value;
            ViewBag.Value2 = value2;
            return View();
        }
    }
}
