using BE_Glowpurea.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    [ServiceFilter(typeof(SingleSessionFilter))]
    [ApiController]
    public abstract class AdminBaseController : ControllerBase
    {
    }
}
