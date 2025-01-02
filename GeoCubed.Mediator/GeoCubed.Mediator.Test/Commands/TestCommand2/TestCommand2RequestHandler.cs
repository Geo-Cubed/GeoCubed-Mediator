namespace GeoCubed.Mediator.Test.Commands.TestCommand2;

public class TestCommand2RequestHandler : IRequestHandler<TestCommand2Request, int>
{
    public Task<int> Handle(TestCommand2Request request)
    {
        return Task.FromResult(request.Value);
    }
}
