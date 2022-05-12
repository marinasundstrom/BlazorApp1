using MassTransit;
using MassTransit.MessageData;

using Documents.Contracts;
using Documents.Services;
using Documents.Data;
using Microsoft.EntityFrameworkCore;
using Documents.Models;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Documents.Consumers;

public class CreateDocumentFromTemplateConsumer : IConsumer<CreateDocumentFromTemplate>
{
    private readonly DocumentsContext _documentsContext;
    private readonly IRazorTemplateCompiler _razorTemplateCompiler;
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IMessageDataRepository _messageDataRepository;

    public CreateDocumentFromTemplateConsumer(
        DocumentsContext documentsContext,
        IRazorTemplateCompiler razorTemplateCompiler,
        IPdfGenerator pdfGenerator,
        IMessageDataRepository messageDataRepository) 
    {
        _documentsContext = documentsContext;
        _razorTemplateCompiler = razorTemplateCompiler;
        _pdfGenerator = pdfGenerator;
        _messageDataRepository = messageDataRepository;
    }

    public async Task Consume(ConsumeContext<CreateDocumentFromTemplate> context)
    {
        var request = context.Message;

        DocumentTemplate? documentTemplate = await _documentsContext.DocumentTemplates
            .FirstOrDefaultAsync(dt => dt.Id == request.TemplateId, context.CancellationToken);

        if(documentTemplate is null)
        {
            throw new Exception("DocumentTemplate not found");
        }

        string documentContent;

        if (documentTemplate.TemplateLanguage == DocumentTemplateLanguage.Razor)
        {
            documentContent = await RenderRazorTemplate(request, documentTemplate);
        }
        else
        {
            documentContent = documentTemplate.Content;
        }

        byte[] documentBytes;

        if (request.DocumentFormat == DocumentFormat.Html)
        {
            documentBytes = Encoding.UTF8.GetBytes(documentContent);
        }
        else if (request.DocumentFormat == DocumentFormat.Pdf)
        {
            documentBytes = await GeneratePdf(documentContent);
        }
        else
        {
            throw new Exception("Invalid format selected");
        }

        await context.RespondAsync<DocumentResponse>(new DocumentResponse(request.DocumentFormat, await _messageDataRepository.PutBytes(documentBytes, TimeSpan.FromDays(1), context.CancellationToken)));
    }

    private async Task<string> RenderRazorTemplate(CreateDocumentFromTemplate request, DocumentTemplate documentTemplate)
    {
        string renderedHtml;

        Console.WriteLine("Json: {0}", request.Model.Replace("\\", string.Empty).Trim('"'));


        dynamic model = JsonConvert.DeserializeObject<object>(request.Model.Replace("\\", string.Empty).Trim('"'));

        Console.WriteLine("Name: {0}", model.Name);

        if (!_razorTemplateCompiler.HasCachedTemplate(request.TemplateId))
        {
            renderedHtml = await _razorTemplateCompiler.CompileAndRenderAsync(
                request.TemplateId,
                documentTemplate.Content,
                model);
        }
        else
        {
            renderedHtml = await _razorTemplateCompiler.RenderAsync(
              request.TemplateId,
              model);
        }

        return renderedHtml;
    }

    private async Task<byte[]> GeneratePdf(string renderedHtml)
    {
        var pdfStream = await _pdfGenerator.GeneratePdfFromHtmlAsync(renderedHtml);
        MemoryStream memoryStream = new();
        await pdfStream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream.GetBuffer();
    }
}