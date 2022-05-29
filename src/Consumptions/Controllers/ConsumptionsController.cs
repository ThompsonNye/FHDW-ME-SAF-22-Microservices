using System.Net;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Dtos;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConsumptionsController : ControllerBase
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public ConsumptionsController(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all <see cref="ConsumptionDto"/> entries.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ConsumptionDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _dbContext.Consumptions
                .ProjectTo<ConsumptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            
            return Ok(result);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            var consumptionId = new ConsumptionId(id);
            var result = await _dbContext.Consumptions.FirstOrDefaultAsync(x => x.Id == consumptionId);
            return result is not null 
            ? Ok(result)
            : NotFound();
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ConsumptionDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateNewAsync([FromBody] ConsumptionDto consumptionDto, CancellationToken cancellationToken)
        {
            var consumption = _mapper.Map<Consumption>(consumptionDto);
            _dbContext.Consumptions.Add(consumption);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetSingle), new { id = consumption.Id }, consumption);
        }
        
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ConsumptionDto consumptionDto, CancellationToken cancellationToken)
        {
            var consumptionId = new ConsumptionId(id);
            var consumption = await _dbContext.Consumptions.FirstOrDefaultAsync(x => x.Id == consumptionId, cancellationToken);
            if (consumption is null)
            {
                return NotFound();
            }

            _mapper.Map(consumptionDto, consumption);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
        
        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var consumptionId = new ConsumptionId(id);
            var consumption = await _dbContext.Consumptions.FirstOrDefaultAsync(x => x.Id == consumptionId, cancellationToken);
            if (consumption is null)
            {
                return NotFound();
            }

            _dbContext.Consumptions.Remove(consumption);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
