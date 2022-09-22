namespace MySpot.Infrastructure.DAL;

//w ramach wywolania metody bylo uruchomione w sposob transakcyjny
internal interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
}
