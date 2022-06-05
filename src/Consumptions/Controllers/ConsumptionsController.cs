using System.Net;
using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Kafka;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Dtos;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Requests;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConsumptionsController : ControllerBase
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<ConsumptionsController> _logger;
        private readonly IMapper _mapper;

        public ConsumptionsController(IApplicationDbContext dbContext, IMapper mapper,
            ILogger<ConsumptionsController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Returns all <see cref="ConsumptionDto"/> entries.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ConsumptionDto>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _dbContext.Consumptions
                .ProjectTo<ConsumptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Returns a single <see cref="ConsumptionDto"/> entry.
        /// </summary>
        /// <param name="id">The entry's id.</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ConsumptionDto), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            var consumptionId = new ConsumptionId(id);
            var result = await _dbContext.Consumptions.FirstOrDefaultAsync(x => x.Id == consumptionId);
            return result is not null
                ? Ok(result)
                : NotFound();
        }

        /// <summary>
        /// Creates a new consumption entry.
        /// </summary>
        /// <param name="createConsumptionCommand"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ConsumptionDto), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> CreateNewAsync([FromBody] CreateConsumptionCommand createConsumptionCommand,
            CancellationToken cancellationToken)
        {
            var consumption = _mapper.Map<Consumption>(createConsumptionCommand);
            _dbContext.Consumptions.Add(consumption);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetSingle), new {id = consumption.Id}, consumption);
        }

        /// <summary>
        /// Updates an existing consumption entry.
        /// </summary>
        /// <param name="id">The entry's id.</param>
        /// <param name="updateConsumptionCommand">The updated values (if provided, the id has to match the route id).</param>
        /// <param name="producer"></param>
        /// <param name="config"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateAsync(Guid id,
            [FromBody] UpdateConsumptionCommand updateConsumptionCommand,
            [FromServices] KafkaDependentProducer<Null, string> producer,
            [FromServices] IConfiguration config,
            CancellationToken cancellationToken)
        {
            var consumptionId = new ConsumptionId(id);
            var consumption =
                await _dbContext.Consumptions.FirstOrDefaultAsync(x => x.Id == consumptionId, cancellationToken);
            if (consumption is null)
            {
                return NotFound();
            }

            consumption.Amount = updateConsumptionCommand.Amount ?? consumption.Amount;
            consumption.Distance = updateConsumptionCommand.Distance ?? consumption.Distance;
            consumption.CarId = updateConsumptionCommand.CarId.HasValue
                ? new(updateConsumptionCommand.CarId.Value)
                : consumption.CarId;
            consumption.DateTime = updateConsumptionCommand.DateTime ?? consumption.DateTime;
            consumption.IgnoreInCalculation =
                updateConsumptionCommand.IgnoreInCalculation ?? consumption.IgnoreInCalculation;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var topic = config.GetValue<string>("Kafka:UpdateConsumptionsTopic");
            producer.Produce(topic, new()
            {
                Value = JsonSerializer.Serialize(consumption)
            }, report => DeliveryReportHandler.Handle(report, _logger));

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing consumption entry.
        /// </summary>
        /// <param name="id">The entry's id.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var consumptionId = new ConsumptionId(id);
            var consumption =
                await _dbContext.Consumptions.FirstOrDefaultAsync(x => x.Id == consumptionId, cancellationToken);
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