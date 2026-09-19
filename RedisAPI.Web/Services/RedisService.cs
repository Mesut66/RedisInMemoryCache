using StackExchange.Redis;

namespace RedisAPI.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;

        private ConnectionMultiplexer _redis;
        public IDatabase db { get; private set; }
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }

        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";
            _redis = ConnectionMultiplexer.Connect(configString);

        }

        //redis db ye yazacak ama bende kurulmadu: Redis 15 d var hangisine söylersen yazar
        public IDatabase GetDatabase(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
