using Microsoft.AspNetCore.Mvc;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class Consumptions : ControllerBase
    {
        private readonly IApplicationDbContext _dbContext;
        public Consumptions(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _dbContext.Consumptions.ToList();
            return Ok(result);
        }
    }
}
