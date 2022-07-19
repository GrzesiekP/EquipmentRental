using System;
using System.Linq;
using System.Threading.Tasks;
using EquipmentRental.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Commands;
using Orders.Models.ValueObjects;
using Orders.Queries;
using EquipmentItem = Orders.Models.Entities.EquipmentItem;

namespace EquipmentRental.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _mediator.Send(new GetOrders(User.Email()));

            return Ok(result);
        }
        
        [HttpGet]
        [Route("[controller]/{orderId:guid}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var result = await _mediator.Send(new GetOrder(orderId, User.Email()));

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderInput input)
        {
            var orderId = Guid.NewGuid();

            var orderData = new OrderData(
                input.EquipmentItems
                    .Select(i => new EquipmentItem(new EquipmentType(i.EquipmentTypeCode, i.RentalPrice)))
                    .ToList(),
                new RentalPeriod(input.RentalDate, input.ReturnDate));
            var command = new SubmitOrder(orderId, orderData, User.Email());
            await _mediator.Send(command);

            return Ok(orderId);
        }

        [HttpPut]
        public async Task<IActionResult> ApproveRequest([FromBody] Guid orderId)
        {
            var command = new ApproveOrder(orderId);

            await _mediator.Send(command);

            return Accepted();
        }
    }
}