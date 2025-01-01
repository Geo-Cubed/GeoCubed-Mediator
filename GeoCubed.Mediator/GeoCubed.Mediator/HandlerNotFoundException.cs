namespace GeoCubed.Mediator;

/// <summary>
/// Exception if the handler is not found.
/// </summary>
public sealed class HandlerNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerNotFoundException"/> class.
    /// </summary>
    public HandlerNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerNotFoundException"/> class.
    /// </summary>
    /// <param name="msg">The error message.</param>
    public HandlerNotFoundException(string msg)
        : base(msg)
    {
    }
}
