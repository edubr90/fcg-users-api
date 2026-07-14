// "// Copyright (c) FIAP Cloud Games. All rights reserved."

using Users.Domain.Interfaces;
using Users.Infrastructure.Persistence;

namespace Users.Infrastructure.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly UsersDbContext _db;

    public UnitOfWork(UsersDbContext db) => _db = db;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _db.SaveChangesAsync(cancellationToken);
    }
}
