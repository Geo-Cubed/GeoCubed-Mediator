using GeoCubed.Mediator.Common;

namespace GeoCubed.Mediator;

/// <summary>
/// Concrete implementation of the <see cref="IMediator"/> interface.
/// </summary>
public sealed class Mediator : IMediator
{
    private readonly IServiceProvider _provider;
    private readonly string _handleMethodName;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    public Mediator(IServiceProvider provider)
    {
        this._provider = provider;
        var handlerType = typeof(IRequestHandler<IRequest<string>, string>);
        var requestType = typeof(IRequest<string>);
        this._handleMethodName = handlerType?
            .GetMethods()?
            .Where(x => 
                x.ReturnType == typeof(Task<string>)
                && x.GetParameters().Count() == 1
                && x.GetParameters().Any(x => x.ParameterType == requestType))
            .FirstOrDefault()?.Name 
            ?? string.Empty;
    }

    /// <summary>
    /// Sends and handles a request to the application.
    /// </summary>
    /// <typeparam name="TResponse">The return type of the request.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <returns>The handled response from the <see cref="IRequestHandler{TRequest, TResponse}"/> class.</returns>
    /// <exception cref="HandlerNotFoundException">Exception thrown if the handler cannot be found.</exception>
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        // TODO: I feel like this could be simplified to make an instance based on the request interface.
        var genericHandlerType = typeof(IRequestHandler<IRequest<TResponse>, TResponse>);
        var handler = MediatorHelper.GetHandlerType(genericHandlerType, request.GetType());
        var instance = handler.InstantiateType(this._provider)
            ?? throw new HandlerNotFoundException();
        var method = instance.GetType().GetMethod(this._handleMethodName)
            ?? throw new HandlerNotFoundException();

        TResponse response;
        try
        {
            response = (TResponse)await method.InvokeAsync(instance, request);
        }
        catch (ArgumentException ex)
        {
            throw new HandlerNotFoundException();
        }

        return response;
    }
}
