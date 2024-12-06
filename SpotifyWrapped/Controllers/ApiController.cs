using ETLYelticDashboard.Classes.Database.Generic;
using Microsoft.AspNetCore.Mvc;
using SpotifyWrapped.Classes;

namespace SpotifyWrapped.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseIntermediary _dbSQL;
        private readonly LoadInfo _loadInfo;
        private readonly DataExtraction _extractInfo;
        public ApiController(DatabaseIntermediary databaseIntermediary, ILogger<HomeController> logger)
        {
            _dbSQL = databaseIntermediary;
            _logger = logger;
            _loadInfo = new LoadInfo(databaseIntermediary);
            _extractInfo = new DataExtraction();
        }

        [HttpGet("extract-spotify-data")]
        public async Task<IActionResult> Index()
        {
            var extractedData = await _extractInfo.ExtractionCicle();
            await _loadInfo.ProcessRawData(extractedData);

            return Ok("Procesado mi perro");
        }
    }
}
