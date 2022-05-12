﻿using Documents.Data;
using Documents.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Documents;

public class SeedData
{
    public static async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedData>>();

            var context = scope.ServiceProvider.GetRequiredService<DocumentsContext>();
            //await context.Database.EnsureDeletedAsync();
            //context.Database.Migrate();
            await context.Database.EnsureCreatedAsync();

            var documentTemplate = await context.DocumentTemplates.FirstOrDefaultAsync(dt => dt.Id == "greeting");

            if(documentTemplate is null) 
            {
                documentTemplate = new DocumentTemplate()
                {
                    Id = "greeting",
                    Name = "Greeting",
                    TemplateLanguage = DocumentTemplateLanguage.Razor,
                    Content = 
@$"
@model dynamic
Hello, @Model.Name!"
                };

                context.DocumentTemplates.Add(documentTemplate);

                await context.SaveChangesAsync();
            }
        }
    }
}