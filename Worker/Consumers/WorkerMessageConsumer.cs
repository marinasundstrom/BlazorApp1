using MassTransit;

using Worker.Contracts;

namespace Worker.Consumers;

public class WorkerMessageConsumer : IConsumer<WorkerMessage>
{
    public async Task Consume(ConsumeContext<WorkerMessage> context)
    {
        var message = context.Message;

        await Task.Delay(new Random().Next(500, 2000));

        await context.Publish(new Contracts.WorkerResponse($"Greetings, {message.Text}!"));
    }
}
