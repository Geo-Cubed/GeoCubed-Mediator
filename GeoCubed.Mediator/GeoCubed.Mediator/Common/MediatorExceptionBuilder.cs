namespace GeoCubed.Mediator.Common;

internal class MediatorExceptionBuilder
{
    private string _message;
    private Exception? _innerException;

    private MediatorExceptionBuilder()
    {
        this._message = string.Empty;
    }

    internal static MediatorExceptionBuilder AnException()
    {
        return new MediatorExceptionBuilder();
    }

    internal MediatorExceptionBuilder WithMessage(string message)
    {
        this._message = message;
        return this;
    }

    internal MediatorExceptionBuilder WithInnerException(Exception innerException)
    {
        this._innerException = innerException;
        return this;
    }

    internal MediatorException Build()
    {
        MediatorException ex;
        if (string.IsNullOrEmpty(this._message) && this._innerException == null)
        {
            ex = new MediatorException();
        } 
        else if (this._innerException == null)
        {
            ex = new MediatorException(this._message);
        }
        else
        {
            ex = new MediatorException(this._message, this._innerException);
        }

        return ex;
    }
}
