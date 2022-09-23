using Humanizer;
using Microsoft.Extensions.Logging;
using MySpot.Application.Abstractions;
using System.Diagnostics;

namespace MySpot.Infrastructure.Logging.Decorators;

internal sealed class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly ILogger<ICommandHandler<TCommand>> _logger;

    public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler,
        ILogger<ICommandHandler<TCommand>> logger)
    {
        _logger = logger;
        _commandHandler = commandHandler;
    }

    public async Task HandleAsync(TCommand command)
    {
        var commandName = command.GetType().Name.Underscore();
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        //specjalnie nie loguje z $ zeby miec wlasciwosc w logach
        _logger.LogInformation("Started handling a command: {CommandName}", commandName);
        await _commandHandler.HandleAsync(command);
        stopwatch.Stop();
        _logger.LogInformation("Completed handling a command: {CommandName} in {Elapsed}", commandName, stopwatch.Elapsed);
    }
}