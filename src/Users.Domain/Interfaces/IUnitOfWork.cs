// "// Copyright (c) FIAP Cloud Games. All rights reserved."

namespace Users.Domain.Interfaces;
public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
