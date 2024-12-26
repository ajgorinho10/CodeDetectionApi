using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CodeDetection.Controllers
{
    [ApiController]
    [Route("Detect/")]
    public class ProgramController : ControllerBase
    {
        [HttpPost("qr")]
        public IActionResult GetQRResponse([FromBody] ImageRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Img))
            {
                return BadRequest("img is required.");
            }


            var kod = KodQR.qr.Qrapi.Detection(request.Img);

            var response = new ResponseModel
            {
                Message = $"OK!",
                StatusCode = 200,
                Timestamp = DateTime.UtcNow
            };

            return Ok(kod);
        }

        [HttpPost("bar")]
        public IActionResult GetBARResponse([FromBody] ImageRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Img))
            {
                return BadRequest("img is required.");
            }

            var kod = KodQR.bar.Barapi.Detection(request.Img);

            var response = new ResponseModel
            {
                Message = $"OK!",
                StatusCode = 200,
                Timestamp = DateTime.UtcNow
            };

            return Ok(kod);
        }

        [HttpPost("")]
        public IActionResult GetALLResponse([FromBody] ImageRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Img))
            {
                return BadRequest("img is required.");
            }

            var kod = KodQR.qr.Qrapi.DetectionQrBar(request.Img);

            var response = new ResponseModel
            {
                Message = "OK!",
                StatusCode = 200,
                Timestamp = DateTime.UtcNow
            };

            return Ok(kod);
        }
    }
}
