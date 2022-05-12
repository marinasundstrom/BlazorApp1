﻿using MassTransit;

using Documents.Contracts;
using Documents.Services;

namespace Documents.Consumers;

public class SendEmailConsumer : IConsumer<SendEmail>
{
    private readonly IEmailService _emailService;

    public SendEmailConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<SendEmail> context)
    {
        var message = context.Message;

        await _emailService.SendEmail(message.Recipient, message.Subject, message.Body);
    }
}