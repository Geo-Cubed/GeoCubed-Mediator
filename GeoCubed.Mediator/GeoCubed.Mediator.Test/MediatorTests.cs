using GeoCubed.Mediator.Test.Commands.TestCommand1;
using GeoCubed.Mediator.Test.Commands.TestCommand2;
using GeoCubed.Mediator.Test.Commands.TestCommand3;
using GeoCubed.Mediator.Test.Commands.TestCommand4;
using GeoCubed.Mediator.Test.Commands.TestCommand5;
using GeoCubed.Mediator.Test.Commands.TestCommand6;

namespace GeoCubed.Mediator.Test;

/// <summary>
/// Tests for the mediator.
/// </summary>
public class MediatorTests
{
    private readonly IMediator _mediator;

    public MediatorTests(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
        this._mediator = mediator;
    }

    /// <summary>
    /// Test that a value is returned from the mediator.
    /// </summary>
    [Fact]
    public async void TestValueReturn()
    {
        var response = await this._mediator.Send(new TestCommand1Request());
        Assert.Equal(0, response);
    }

    /// <summary>
    /// Test that a value can be passed in the command.
    /// </summary>
    [Fact]
    public async void TestValuePassed()
    {
        var value = 10;
        var response = await this._mediator.Send(new TestCommand2Request() { Value = value });

        Assert.Equal(value, response);
    }

    /// <summary>
    /// Test that the command correctly returns an object.
    /// </summary>
    [Fact]
    public async void TestReturnsObject()
    {
        var response = await this._mediator.Send(new TestCommand3Request());

        Assert.Equal(15, response.Value);
    }

    /// <summary>
    /// Test that services are correctly injected when completing the request.
    /// </summary>
    [Fact]
    public async void TestDependancyInjectionOnHandler()
    {
        var value = 10;
        var response = await this._mediator.Send(new TestCommand4Request() { Value = value });

        Assert.Equal(Math.Pow(10, 2), response);
    }

    /// <summary>
    /// Test that an exception is thrown when there is no handler for the request.
    /// </summary>
    [Fact]
    public async void TestExceptionOnNoHandler()
    {
        var request = new TestCommand5Request();
        var exception = await Assert.ThrowsAsync<MediatorException>(() => this._mediator.Send(request));

        var exceptionMessage = string.Format("There was no handler found for the request {0}.", request.GetType().Name);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    /// <summary>
    /// Test that an exception is thrown when there is no handle method.
    /// </summary>
    [Fact(Skip = "No clue how to test this.")]
    public async void TestExceptionOnNoMethod()
    {
        // TODO: Work out how to test this one.
    }

    /// <summary>
    /// Test that an exception is captured inside a mediator exception and thrown by the mediator when happening on the handler.
    /// </summary>
    [Fact]
    public async void TestExceptionOnHandler()
    {
        var request = new TestCommand6Request();
        var exception = await Assert.ThrowsAsync<MediatorException>(() => this._mediator.Send(request));

        var exceptionMessage = string.Format("An exception occured while trying to run the 'Handle' method on the handler {0} see inner exception for more details", typeof(TestCommand6RequestHandler).Name); ;
        Assert.Equal(exceptionMessage, exception.Message);

        Assert.Equal(typeof(NotImplementedException), exception.InnerException.GetType());
        Assert.Equal("No Implementation", exception.InnerException.Message);
    }

    [Fact(Skip = "No clue how to test this.")]
    public async void TestExceptionOnInvoke()
    {
        // TODO: Work out how to test this.
    }
}