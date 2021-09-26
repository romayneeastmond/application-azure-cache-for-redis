using ApplicationAzureCacheRedis.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationAzureCacheRedis.Web.Models
{
    public class CountryModel
    {
        private readonly CountryDbContext _db;
        private readonly IDatabase _cache;

        public string Key
        {
            get { return "CountriesList"; }
        }

        public CountryModel(CountryDbContext db, IDatabase cache)
        {
            _db = db;
            _cache = cache;
        }

        public List<Country> Get(ref string source)
        {
            source = "Database";

            var key = Key;

            var countries = new List<Country>();

            try
            {
                string serializedCountries = _cache.StringGet(key);

                if (!string.IsNullOrWhiteSpace(serializedCountries))
                {
                    countries = JsonConvert.DeserializeObject<List<Country>>(serializedCountries);

                    source = "Azure Cache for Redis";
                }
                else
                {
                    countries = _db.Countries.Select(x => x).ToList();

                    _cache.StringSet(key, JsonConvert.SerializeObject(countries));
                }
            }
            catch
            {
                countries = _db.Countries.Select(x => x).ToList();
            }

            if (countries != null && countries.Any())
            {
                countries = countries.OrderBy(x => x.Name).ThenBy(x => x.Code).ThenBy(x => x.Population).ToList();
            }

            return countries;
        }

        public List<Country> GetByDatabase()
        {
            return _db.Countries.Select(x => x).ToList();
        }

        public List<Country> GetByCache()
        {
            var key = Key;

            var countries = new List<Country>();

            try
            {
                string serializedCountries = _cache.StringGet(key);

                if (!string.IsNullOrWhiteSpace(serializedCountries))
                {
                    countries = JsonConvert.DeserializeObject<List<Country>>(serializedCountries);
                }
            }
            catch
            {
                //ignored
            }

            return countries;
        }
    }
}