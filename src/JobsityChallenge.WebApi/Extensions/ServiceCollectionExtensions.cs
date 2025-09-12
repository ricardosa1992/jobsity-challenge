using JobsityChallenge.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JobsityChallenge.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        SqlConnectionStringBuilder sqlBuilder = new(connectionString)
        {
            TrustServerCertificate = true,
            Encrypt = false
        };

        SqlClientFactory.Instance.CreateConnection().ConnectionString = sqlBuilder.ConnectionString;
        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(sqlBuilder.ConnectionString));

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}