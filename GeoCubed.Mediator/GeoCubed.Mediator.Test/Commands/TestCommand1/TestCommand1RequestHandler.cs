namespace GeoCubed.Mediator.Test.Commands.TestCommand1;

public class TestCommand1RequestHandler : IRequestHandler<TestCommand1Request, int>
{
    public Task<int> Handle(TestCommand1Request request)
    {
        return Task.FromResult(0);
    }
}
