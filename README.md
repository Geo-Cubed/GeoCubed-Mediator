# GeoCubed Mediator

This is a mediator library I wrote where you can write requests and request handlers an have a mediator handle the executing of the requests passed to it

The mediator is injected into the class to Send requests and get their responses

## Quick Start

##### 1. Add a reference to the `GeoCubed.Mediator.csproj` file
This file can be found at `GeoCubed.Mediator/GeoCubed.Mediator/GeoCubed.Mediator.csproj`

##### 2. Write requests and request handlers

A request will implement the interface `IRequest<TResponse>` and the handler will implement the interface `IRequestHandler<IRequest<TResponse>, TResponse>`

This means a request will be passed to the request handler using the `Handle` method which will return the response

- Writing a request
A request must implement the `IRequest<>` interface
A request can have 0 or more fields on it that will be used in the request handler
```csharp
public class ExampleRequest : IRequest<ExampleResponse>
{
    public string RequestValue { get; set; }
}
```

- Writing a request handler
A request handler must implement the `IRequestHandler<>` interface
A request handler will implement the `Task<TResponse> Handle(IRequest<TResponse> request)` method
A request handler can have services injected into it
```csharp
public class ExampleRequestHandler : IRequestHandler<ExampleRequest, ExampleResponse>
{
    private readonly ExampleService _service;

    public ExampleRequestHandler(ExampleService service)
    {
        this._service = service;
    }

    public Task<ExampleResponse> Handle(ExampleRequest request)
    {
        return Task.FromResult(new ExampleResponse());
    }
}
```

##### 3. Register the mediator with the service collection
This will normally be done in Startup.cs or Program.cs

```csharp
services.AddMediator(Assembly.GetExecutingAssembly());
```

or

```csharp
GeoCubed.Mediator.MediatorServiceRegistration.AddMediator(services, Assembly.GetExecutingAssembly());
```

Note:
The assembly does not have to be specified but it will default to `Assembly.GetCallingAssembly()`
Each assembly that has request handlers will need registering

```csharp
services.AddMediator(assembly1);
services.AddMediator(assembly2);
```

## Using The Mediator

#### 1. Inject the mediator
#### 2. Call the mediators send method

```csharp
public class ExampleClass
{
    // * STEP 1 *
    private readonly IMediator _mediator;

    public ExampleClass(IMediator mediator)
    {
        this._mediator = mediator;
    }
    // * END STEP 1 *
    
    // * STEP 2 *
    public async void SomeTask()
    {
        var request = new ExampleRequest()
        {
            RequestValue = "Some Value",
        };

        ExampleResponse response = await this._mediator.Send(request);
    }
    // * END STEP 2 *
}
```

Note:
The mediator is asyncronous and returns a `Task<Response>` so the `Send(Request request)` method can be awaited to unpack the task.

## Test Project

Located in `GeoCubed.Mediator/GeoCubed.Mediator.Test` there is a xUnit test project `GeoCubed.Mediator.Test.csproj`
This project is used to test the validity of the functionality of the mediator
This project can be ignored as it is only used to verify that the mediator is working as expected