using System.Collections.Generic;
using System.Security.Claims;
using EquipmentRental.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.Core;

namespace EquipmentRental.WebApi.Tests.OrdersController;

public class OrdersControllerTestsBase : ControllerTestBase<OrderController>
{
    protected Mock<IMediator>? MediatorMock;
    protected string? UserEmail;

    protected override void Given()
    {
        base.Given();

        UserEmail = "testuser@rental.pl";
        
        MediatorMock = new Mock<IMediator>();
        Controller = new OrderController(MediatorMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new List<ClaimsIdentity>
                    {
                        new(new List<Claim>
                        {
                            new(ClaimTypes.Email, UserEmail)
                        })
                    })
                }
            }
        };
    }
}