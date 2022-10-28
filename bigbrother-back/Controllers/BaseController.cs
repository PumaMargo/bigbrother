using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace bigbrother_back.Utility
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        public const int MaxPageCount = 2000;
    }
}
