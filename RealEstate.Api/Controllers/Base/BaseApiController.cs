using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Api.Controllers.Base
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
