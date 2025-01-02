
namespace GeoCubed.Mediator.Test.Commands.TestCommand4;

public class TestCommand4RequestHandler : IRequestHandler<TestCommand4Request, int>
{
    private readonly InjectedService _service;

    public TestCommand4RequestHandler(InjectedService service)
    {
        ArgumentNullException.ThrowIfNull(service, nameof(service));
        this._service = service;
    }

    public Task<int> Handle(TestCommand4Request request)
    {
        return Task.FromResult(this._service.Square(request.Value));
    }
}
