namespace GeoCubed.Mediator.Test.Commands.TestCommand3;

public class TestCommand3RequestHandler : IRequestHandler<TestCommand3Request, TestCommand3Response>
{
    public Task<TestCommand3Response> Handle(TestCommand3Request request)
    {
        return Task.FromResult(new TestCommand3Response() { Value = 15 });
    }
}
