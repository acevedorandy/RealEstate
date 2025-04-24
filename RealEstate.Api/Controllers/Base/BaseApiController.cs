using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Api.Controllers.Base
{
    [ApiVersion("1.0")]
    [Route("api/v{version}/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
