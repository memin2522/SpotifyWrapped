using ETLYelticDashboard.Classes.Database.Generic;
using Microsoft.AspNetCore.Mvc;
using SpotifyWrapped.Classes;
using SpotifyWrapped.Models;
using System.Diagnostics;

namespace SpotifyWrapped.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseIntermediary _dbSQL;
        private readonly LoadInfo _loadInfo;
        private readonly DataExtraction _extractInfo;

        public HomeController(DatabaseIntermediary databaseIntermediary, ILogger<HomeController> logger)
        {
            _dbSQL = databaseIntermediary;
            _logger = logger;
            _loadInfo = new LoadInfo(databaseIntermediary);
            _extractInfo = new DataExtraction();
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
