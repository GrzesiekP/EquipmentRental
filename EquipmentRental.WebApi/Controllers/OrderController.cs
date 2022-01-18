using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Commands;

namespace EquipmentRental.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOrder()
        {
            var orderId = Guid.NewGuid();

            var command = new SubmitOrder(orderId);
            await _mediator.Send(command);

            return Ok(orderId);
        }

        [HttpPut]
        public async Task<IActionResult> ApproveRequest([FromBody] Guid orderId)
        {
            var command = new ApproveRequest(orderId);

            await _mediator.Send(command);

            return Accepted();
        }
    }
}