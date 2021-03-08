using System;
using System.Net;
using Mag.VisualizationLocation.Adapter.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Mag.VisualizationLocation.Adapter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly IScenario _scenario;

        public PlaceController(IScenario scenario)
        {
            if (scenario != null) _scenario = scenario;
        }

        [HttpPost("RegistrationInfo")]
        public IActionResult RegistrationInfo([FromBody] RegistrationInfoRequest request)
        {
            try
            {
                var response = _scenario.GetRegistrationInfo(request);

                return Ok(response);
            }
            catch (NotFoundParmsException)
            {
                return new StatusCodeResult((int)HttpStatusCode.UnprocessableEntity);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
        }

        [HttpPost("RegistrationInfoDto")]
        public IActionResult RegistrationInfoDto([FromBody] RegistrationsByBaseStationsRequest request)
        {
            return Ok("RegistrationInfoDto");
            //try
            //{
            //    var response = _scenario.GetRegistrationInfo(request);

            //    return Ok(response);
            //}
            //catch (NotFoundParmsException exception)
            //{
            //    return BadRequest(exception);
            //}
            //catch (Exception exception)
            //{
            //    return BadRequest(exception);
            //}
        }
    }
}
