namespace GeoCubed.Mediator;

/// <summary>
/// Exception if the handler is not found.
/// </summary>
public sealed class MediatorException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MediatorException"/> class.
    /// </summary>
    public MediatorException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MediatorException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public MediatorException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MediatorException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception</param>
    public MediatorException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
