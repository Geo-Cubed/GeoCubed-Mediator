namespace GeoCubed.Mediator;

/// <summary>
/// Interface for mediator requests.
/// </summary>
/// <typeparam name="TResponse">The return type of the request.</typeparam>
public interface IRequest<out TResponse>
{
}
