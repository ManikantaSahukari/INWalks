﻿using Microsoft.AspNetCore.Identity;

namespace INWalksAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
