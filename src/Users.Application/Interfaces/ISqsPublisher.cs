// "// Copyright (c) FIAP Cloud Games. All rights reserved."

namespace Users.Application.Interfaces;
public interface ISqsPublisher
{
    Task PublishAsync<T>(string queueUrl, T message, CancellationToken cancellationToken = default) where T : class;
}
