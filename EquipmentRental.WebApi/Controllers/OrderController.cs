using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Equipment.Models.ValueObjects;
using EquipmentRental.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Commands;
using Orders.Models.ValueObjects;
using Orders.Queries;
using EquipmentItem = Equipment.Models.Entities.EquipmentItem;

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

            var equipmentItems = input.EquipmentItems.Select(x => new EquipmentItem(
                new EquipmentType(x.EquipmentTypeCode, x.RentalPrice))).ToList();

            var rentalPeriod = new RentalPeriod(input.RentalDate, input.ReturnDate);
            var orderData = new OrderData(
                equipmentItems,
                rentalPeriod
                );
            
            var command = new SubmitOrder(orderId, orderData, User.Email());
            Console.WriteLine($"{nameof(OrderController)} publishing {nameof(SubmitOrder)}");
            await _mediator.Send(command);

            return Ok(orderId);
        }

        private void ValidateIfEquipmentIsAvailable()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("approve")]
        public async Task<IActionResult> ApproveRequest([FromBody] Guid orderId)
        {
            var command = new ApproveOrder(orderId);

            await _mediator.Send(command);

            return Accepted();
        }
        
        [HttpPut]
        [Route("submit-payment")]
        public async Task<IActionResult> SubmitPayment([FromBody] SubmitPaymentInput input)
        {
            var command = new ConfirmOrderPayment(input.OrderId, new Money(input.Amount));

            // TODO: Handler not defined
            await _mediator.Send(command);

            return Accepted();
        }
    }
}