namespace GeoCubed.Mediator;

/// <summary>
/// Request handler interface.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles a request.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <returns>The response of the request, defined by the <see cref="TResponse"/>.</returns>
    Task<TResponse> Handle(TRequest request);
}
