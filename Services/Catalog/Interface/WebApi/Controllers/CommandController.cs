using CraftedSpecially.Application.Common.Interfaces;
using CraftedSpecially.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using CraftedSpecially.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using WebApi;

namespace CraftedSpecially.Catalog.Interface.WebApi.Controllers;

[ApiController]
[Route("productmanagement/command")]
public class CommandController : ControllerBase
{
    private readonly ILogger<CommandController> _logger;
    private readonly Instrumentation _instrumentation;

    public CommandController(
        ILogger<CommandController> logger, 
        Instrumentation instrumentation)
    {
        _logger = logger;
        _instrumentation = instrumentation;
    }

    [HttpPost("registerproduct")]
    public async Task<IActionResult> RegisterProduct(
        [FromBody] RegisterProductCommand command,
        [FromServices] ICommandHandler<RegisterProductCommand> commandHandler)
    {
        _instrumentation.CatalogRegisterRequestCounter.Add(1);
        
        return await HandleCommand(command, commandHandler);
    }

    private async Task<IActionResult> HandleCommand<T>(
        Command command, ICommandHandler<T> commandHandler) where T : Command
    {
        _logger.LogInformation("Handling command {Command}", command.Type);
        await commandHandler.Handle((T)command);
        return Ok();
    }
}