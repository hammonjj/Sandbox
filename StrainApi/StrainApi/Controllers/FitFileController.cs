using FitFileParser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StrainApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FitFileController : ControllerBase
    {
        private readonly ILogger<FitFileController> _logger;

        public FitFileController(ILogger<FitFileController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostSingleFitFile([FromForm(Name = "files")] ICollection<IFormFile> files)
        {
            var fileParser = new FitFileParser.FitFileParser();
            
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
                    filePaths.Add(filePath);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await formFile.CopyToAsync(stream);
                }
            }
                
            await Task.CompletedTask;

            return Ok();
        }
    }
}
