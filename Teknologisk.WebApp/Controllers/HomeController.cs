using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Teknologisk.WebApp.Models;

namespace Teknologisk.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TelemetryClient _client;

        public HomeController(ILogger<HomeController> logger)
        {
            _client = new TelemetryClient(TelemetryConfiguration.CreateDefault());
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Go to the web app in Azure Portal and click on 'Application Insights' > There will be a link in the top to Application Insights web app dash board
            // Click on top pane 'Logs (Analytics)', and query the traces tables or something else
            _client.TrackPageView(nameof(HomeController));
            _client.TrackTrace("Hello world from from HomeController", SeverityLevel.Information);
            _client.TrackTrace("Here are some properties", new Dictionary<string, string>
            {
                { "Request.Headers", JsonConvert.SerializeObject(Request.Headers.ToDictionary(pair => pair.Key, pair => pair.Value))}
            });
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
