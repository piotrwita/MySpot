namespace MySpot.Application.Abstractions;

//posiada dwa parametry
// tquery i tresult
//tquery musi byc typem referencyjnym i musi implementowac tquery od tresult
public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}