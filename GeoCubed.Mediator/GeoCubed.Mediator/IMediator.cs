namespace GeoCubed.Mediator;

/// <summary>
/// Interface for the mediator.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends and handles a request to the application.
    /// </summary>
    /// <typeparam name="TResponse">The return type of the request.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <returns>The handled response from the <see cref="IRequestHandler{TRequest, TResponse}"/> class.</returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
}
