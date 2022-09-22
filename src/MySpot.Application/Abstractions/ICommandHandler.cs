namespace MySpot.Application.Abstractions;

//handler generyczny
//typ ktory wrzucony w ramach parametru generycznego chce zeby byl typem referencyjnym
//chce tez zeby implementowal moj ICommand
//in - akceptuje szerszy zaskres moich typow
public interface ICommandHandler<in TCommand> where TCommand : class
{
    Task HandleAsync(TCommand command);
}
