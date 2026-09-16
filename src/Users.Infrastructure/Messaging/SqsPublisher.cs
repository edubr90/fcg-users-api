using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Users.Application.Interfaces;

namespace Users.Infrastructure.Messaging;

public class SqsPublisher : ISqsPublisher
{
    private readonly IAmazonSQS _sqs;

    public SqsPublisher(IAmazonSQS sqs)
    {
        _sqs = sqs;
    }

    public async Task PublishAsync<T>(string queueUrl, T message, CancellationToken cancellationToken = default) where T : class
    {
        var body = JsonSerializer.Serialize(message);
        await _sqs.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = body
        }, cancellationToken);
    }
}
