namespace GeoCubed.Mediator.Test.Commands.TestCommand1;

public class TestCommandRequestHandler : IRequestHandler<TestCommandRequest, int>
{
    public Task<int> Handle(TestCommandRequest request)
    {
        return Task.FromResult(0);
    }
}
