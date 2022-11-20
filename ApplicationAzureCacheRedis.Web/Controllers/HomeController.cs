using ApplicationAzureCacheRedis.Models;
using ApplicationAzureCacheRedis.Web.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ApplicationAzureCacheRedis.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var countryModel = new CountryModel(new CountryDbContext(), AzureCacheRedisWrapper.Get());

            var sw = Stopwatch.StartNew();

            var source = string.Empty;
            var countries = countryModel.Get(ref source);

            sw.Stop();

            double ms = sw.ElapsedTicks / (Stopwatch.Frequency / (1000.0));

            ViewBag.Message = $"Results read from {source}. MS: {ms}";

            return View(countries);
        }

        public ActionResult Clear()
        {
            try
            {
                ViewBag.Message = "Azure Cache for Redis has not been cleared.";

                var cache = AzureCacheRedisWrapper.Get();

                if (cache != null)
                {
                    ViewBag.Message = "Azure Cache for Redis cleared.";

                    cache.KeyDelete("Message");
                    cache.KeyDelete("CountriesList");
                    cache.KeyDelete("countriesList");
                }
                else
                {
                    ViewBag.Message = "Azure Cache for Redis unavailable.";
                }
            }
            catch
            {
                ViewBag.Message = "Azure Cache for Redis unavailable.";
            }

            return View();
        }

        public ActionResult GetCache()
        {
            var countryModel = new CountryModel(new CountryDbContext(), AzureCacheRedisWrapper.Get());

            var sw = Stopwatch.StartNew();

            var countries = countryModel.GetByCache();

            sw.Stop();

            double ms = sw.ElapsedTicks / (Stopwatch.Frequency / (1000.0));

            ViewBag.Message = $"Results read from Azure Cache for Redis. MS: {ms}";

            return View(countries);
        }

        public ActionResult GetDatabase()
        {
            var countryModel = new CountryModel(new CountryDbContext(), AzureCacheRedisWrapper.Get());

            var sw = Stopwatch.StartNew();

            var countries = countryModel.GetByDatabase();

            sw.Stop();

            double ms = sw.ElapsedTicks / (Stopwatch.Frequency / (1000.0));

            ViewBag.Message = $"Results read from Database. MS: {ms}";

            return View(countries);
        }

        public ActionResult RebuildDatabase()
        {
            var db = new CountryDbContext();

            CountryInitalizer.Initialize(db);

            ViewBag.Message = "Rebuilt local database. This operation does not change the Azure Cache for Redis.";

            return View();
        }

        public ActionResult Test()
        {
            ViewBag.Success = false;

            try
            {
                var cache = AzureCacheRedisWrapper.Get();

                var clients = AzureCacheRedisWrapper.Clients();

                ViewBag.PingResult = cache.Execute("PING").ToString();
                ViewBag.GetMessageTest1 = cache.StringGet("Message").ToString();
                ViewBag.SetMessage = cache.StringSet("Message", $"Hello World from the ASP.NET web application {DateTime.Now}.").ToString();
                ViewBag.GetMessageTest2 = cache.StringGet("Message").ToString();
                ViewBag.ClientListTest = string.Empty;

                if (clients != null && clients.Any())
                {
                    var output = new StringBuilder().AppendLine("Client Response(s)<br />");

                    foreach (var client in clients)
                    {
                        output.AppendLine(client.Raw);
                    }

                    ViewBag.ClientListTest = output.ToString();
                }

                ViewBag.Success = true;
            }
            catch
            {
                //ignored
            }

            return View();
        }
    }
}