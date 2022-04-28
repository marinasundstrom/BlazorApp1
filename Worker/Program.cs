using MassTransit;

using Worker.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    //x.AddConsumers(typeof(Program).Assembly);
    x.AddConsumer<Message2Consumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
})
.AddMassTransitHostedService(true)
.AddGenericRequestClient();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

/* app.MapGet("/greetings", (string name) => new GreetingsResponse($"Greetings, {name}!"))
        .WithName("Greetings_Hi")
        .WithTags("Greetings")
        //.RequireAuthorization()
        .Produces<GreetingsResponse>(StatusCodes.Status200OK); */

app.Run();

public record GreetingsResponse(string Message);