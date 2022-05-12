using Microsoft.EntityFrameworkCore;

using MassTransit;

using Documents.Consumers;
using Documents.Services;
using Documents.Data;
using Documents;
using Documents.Contracts;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using MassTransit.MessageData;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

const string ConnectionStringKey = "sqlserver";

var connectionString = Configuration.GetConnectionString(ConnectionStringKey, "Documents");

builder.Services.AddDbContext<DocumentsContext>((sp, options) =>
{
    options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
});

// TODO: Switch out for Azure Storage later
IMessageDataRepository messageDataRepository = new InMemoryMessageDataRepository();

builder.Services.AddSingleton(messageDataRepository);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    //x.AddConsumers(typeof(Program).Assembly);

    x.AddConsumer<CreateDocumentFromTemplateConsumer>();

    x.AddRequestClient<CreateDocumentFromTemplate>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.UseMessageData(messageDataRepository);

        cfg.ConfigureEndpoints(context);
    });
})
.AddMassTransitHostedService(true)
.AddGenericRequestClient();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRazorTemplateCompiler, RazorTemplateCompiler>();
builder.Services.AddScoped<IPdfGenerator, PdfGenerator>();

//IronPdf.Logging.Logger.EnableDebugging = true;
//IronPdf.Logging.Logger.LogFilePath = "Default.log"; //May be set to a directory name or full file
//IronPdf.Logging.Logger.LoggingMode = IronPdf.Logging.Logger.LoggingModes.All;

var app = builder.Build();

await SeedData.EnsureSeedData(app);

app.MapGet("/", () => "Hello World!");

app.MapPost("/createdoc", async (string templateId, [FromBody] string model, IRequestClient<CreateDocumentFromTemplate> requestClient) =>
{
    var result = await requestClient.GetResponse<DocumentResponse>(new CreateDocumentFromTemplate(templateId, DocumentFormat.Html, JsonConvert.SerializeObject(model)));
    var message = result.Message;
    return Results.File(await message.Document.Value, message.DocumentFormat == DocumentFormat.Html ? "application/html" : "application/pdf");
})
        .WithName("Greetings_Hi")
        .WithTags("Greetings")
        //.RequireAuthorization()
        .Produces(StatusCodes.Status200OK);

app.Run();

record Req(string Model);