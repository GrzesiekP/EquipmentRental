using Microsoft.AspNetCore.Mvc;

namespace Tests.Core;

public class ControllerTestBase<T> : TestsBase where T : ControllerBase
{
    protected IActionResult? Result;
    protected T? Controller;

    protected OkObjectResult? OkResult =>
        Result switch
        {
            OkObjectResult ok => ok,
            _ => null
        };
    
    protected AcceptedResult? AcceptedResult =>
        Result switch
        {
            AcceptedResult accepted => accepted,
            _ => null
        };
}