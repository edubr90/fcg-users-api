// "// Copyright (c) FIAP Cloud Games. All rights reserved."

using Users.Domain.Entities;

namespace Users.Application.Interfaces;
public interface IJwtService
{
    string GenerateToken(User user);
}
