using ApplicationAzureCacheRedis.Services;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ApplicationAzureCacheRedis.Web.Models
{
    public static class AzureCacheRedisWrapper
    {
        public static IDatabase Get()
        {
            try
            {
                var cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();

                AzureCacheRedisService.CacheConnection = cacheConnection;

                var cache = AzureCacheRedisService.GetDatabase();

                return cache;
            }
            catch
            {
                return null;
            }
        }

        public static List<ClientInfo> Clients()
        {
            try
            {
                var endpoint = (System.Net.DnsEndPoint)AzureCacheRedisService.GetEndPoints()[0];

                var server = AzureCacheRedisService.GetServer(endpoint.Host, endpoint.Port);

                return server.ClientList().ToList();
            }
            catch
            {
                return new List<ClientInfo>();
            }
        }
    }
}