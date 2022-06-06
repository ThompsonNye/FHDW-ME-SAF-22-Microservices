using System.Net;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cars.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nuyken.Vegasco.Backend.Microservices.Cars.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Cars.Models.Dtos;
using Nuyken.Vegasco.Backend.Microservices.Cars.Models.Requests;

namespace Nuyken.Vegasco.Backend.Microservices.Cars.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CarsController> _logger;
        private readonly IMapper _mapper;

        public CarsController(IApplicationDbContext dbContext, IMapper mapper,
            ILogger<CarsController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Returns all <see cref="CarDto"/> entries.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CarDto>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _dbContext.Cars
                .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Returns a single <see cref="CarDto"/> entry.
        /// </summary>
        /// <param name="id">The entry's id.</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CarDto), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            var carId = new CarId(id);
            var result = await _dbContext.Cars.FirstOrDefaultAsync(x => x.Id == carId);
            return result is not null
                ? Ok(result)
                : NotFound();
        }

        /// <summary>
        /// Creates a new consumption entry.
        /// </summary>
        /// <param name="createCarCommand"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CarDto), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> CreateNewAsync([FromBody] CreateCarCommand createCarCommand,
            CancellationToken cancellationToken)
        {
            var car = _mapper.Map<Car>(createCarCommand);
            _dbContext.Cars.Add(car);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetSingle), new {id = car.Id}, car);
        }

        /// <summary>
        /// Updates an existing car entry.
        /// </summary>
        /// <param name="id">The entry's id.</param>
        /// <param name="updateCarCommand">The updated values (if provided, the id has to match the route id).</param>
        /// <param name="producer"></param>
        /// <param name="config"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateAsync(Guid id,
            [FromBody] UpdateCarCommand updateCarCommand,
            [FromServices] IConfiguration config,
            CancellationToken cancellationToken)
        {
            var carId = new CarId(id);
            var car =
                await _dbContext.Cars.FirstOrDefaultAsync(x => x.Id == carId, cancellationToken);
            if (car is null)
            {
                return NotFound();
            }

            car.Name = updateCarCommand.Name ?? car.Name;
            
            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing car entry.
        /// </summary>
        /// <param name="id">The entry's id.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var carId = new CarId(id);
            var car =
                await _dbContext.Cars.FirstOrDefaultAsync(x => x.Id == carId, cancellationToken);
            if (car is null)
            {
                return NotFound();
            }

            _dbContext.Cars.Remove(car);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}