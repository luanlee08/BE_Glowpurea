using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [ApiController]
    [Route("api/shapes")]
    public class ShapesController : ControllerBase
    {
        private readonly IShapeService _shapeService;

        public ShapesController(IShapeService shapeService)
        {
            _shapeService = shapeService;
        }

        // GET: api/shapes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shapes = await _shapeService.GetAllAsync();

            return Ok(shapes);
        }
    }
}
