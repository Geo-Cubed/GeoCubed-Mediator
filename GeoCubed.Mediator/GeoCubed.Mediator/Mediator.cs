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

        var handlerType = MediatorHelper.CreateRequestHandlerType();
        this._handleMethodName = handlerType.GetMethods()[0].Name;
    }

    /// <summary>
    /// Sends and handles a request to the application.
    /// </summary>
    /// <typeparam name="TResponse">The return type of the request.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <returns>The handled response from the <see cref="IRequestHandler{TRequest, TResponse}"/> class.</returns>
    /// <exception cref="MediatorException">Exception thrown if there are issues with either finding the handler, finding the method on the handler or handling the request.</exception>
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var genericHandlerType = MediatorHelper.CreateRequestHandlerType(request.GetType(), typeof(TResponse));

        // Get the instance from the service provider.
        var instance = this._provider.GetService(genericHandlerType);
        if (instance == null)
        {
            var exception = MediatorExceptionBuilder
                .AnException()
                .WithMessage(MediatorHelper.ERR_NO_HANDLER(genericHandlerType))
                .Build();

            throw exception;
        }

        // Get the method from the instance.
        var method = instance.GetType().GetMethod(this._handleMethodName);
        if (method == null)
        {
            var exception = MediatorExceptionBuilder
                .AnException()
                .WithMessage(MediatorHelper.ERR_NO_METHOD(this._handleMethodName, genericHandlerType))
                .Build();

            throw exception;
        }

        // Call the method.
        TResponse response;
        try
        {
            response = (TResponse)await method.InvokeAsync(instance, request);
        }
        catch (Exception ex)
        {
            var innerException = ex is ArgumentException
                ? ex
                : ex.InnerException ?? ex;

            var exception = MediatorExceptionBuilder
                .AnException()
                .WithMessage(MediatorHelper.ERR_EXCEPTION_ON_HANDLER(this._handleMethodName, genericHandlerType))
                .WithInnerException(innerException)
                .Build();

            throw exception;
        }

        return response;
    }
}
