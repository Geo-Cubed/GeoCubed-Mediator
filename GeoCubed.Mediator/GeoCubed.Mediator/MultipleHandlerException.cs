namespace GeoCubed.Mediator;

/// <summary>
/// <see cref="Exception"/> for when multiple of the same handler are found.
/// </summary>
public sealed class MultipleHandlerException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultipleHandlerException"/> class.
    /// </summary>
    public MultipleHandlerException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultipleHandlerException"/> class.
    /// </summary>
    /// <param name="message">The error message to use.</param>
    public MultipleHandlerException(string message)
        : base(message)
    {
    }
}
