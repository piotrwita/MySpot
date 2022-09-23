using MySpot.Application.Abstractions;
using MySpot.Application.Commands;

namespace MySpot.Infrastructure.DAL.Decorators;

//zeby udekorowac musi implementowac konkretny obiekt ktory ma udekorowac
internal sealed class UnitOfWorkCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler,
        IUnitOfWork unitOfWork)
    {
        _commandHandler = commandHandler;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(TCommand command)
    {
        await _unitOfWork.ExecuteAsync(() => _commandHandler.HandleAsync(command));
    }
}
