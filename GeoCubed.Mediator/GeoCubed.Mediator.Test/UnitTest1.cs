using GeoCubed.Mediator.Test.Commands.TestCommand1;

namespace GeoCubed.Mediator.Test;

public class UnitTest1
{
    private readonly IMediator _mediator;

    public UnitTest1(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
        this._mediator = mediator;
    }

    [Fact]
    public async void Test1()
    {
        var response = await this._mediator.Send(new TestCommandRequest());
        Assert.Equal(0, response);
    }
}