using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentRental.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Commands;
using Orders.Models.ValueObjects;
using Orders.Queries;

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

            // ValidateIfEquipmentIsAvailable();

            var equipmentItems = new Dictionary<string, int>();
            var equipmentTypes = input.EquipmentItems.GroupBy(x => x.EquipmentTypeCode);
            foreach (var type in equipmentTypes)
            {
                equipmentItems[type.Key] = type.Count();
            }

            var rentalPeriod = new RentalPeriod(input.RentalDate, input.ReturnDate);
            var orderData = new OrderData(
                equipmentItems,
                rentalPeriod,
                new Money(input.EquipmentItems.Sum(item => item.RentalPrice * rentalPeriod.Days))
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
        public async Task<IActionResult> ApproveRequest([FromBody] Guid orderId)
        {
            var command = new ApproveOrder(orderId);

            await _mediator.Send(command);

            return Accepted();
        }
        
        [HttpPut]
        [Route("submitPayment")]
        public async Task<IActionResult> SubmitPayment([FromBody] SubmitPaymentInput input)
        {
            var command = new ConfirmOrderPayment(input.OrderId, new Money(input.Amount));

            // TODO: Handler not defined
            await _mediator.Send(command);

            return Accepted();
        }
    }
}