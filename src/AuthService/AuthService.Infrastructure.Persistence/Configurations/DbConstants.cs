using Microsoft.EntityFrameworkCore;

namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public static class DbConstants
{
    // Define your prefix here
    public const string TablePrefix = "auth_";

    public const DeleteBehavior DeleteBehavior = DeleteBehavior.Restrict;
}
