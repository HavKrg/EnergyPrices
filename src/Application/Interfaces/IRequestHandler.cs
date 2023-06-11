namespace Application.Interfaces;

public interface IRequestHandler<TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request);
}