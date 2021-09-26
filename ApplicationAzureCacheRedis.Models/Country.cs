using System.ComponentModel.DataAnnotations;

namespace ApplicationAzureCacheRedis.Models
{
    public class Country
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public string Continent { get; set; }

        public string Region { get; set; }

        public int Population { get; set; }
    }
}
