using ApiWuthHush.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ApiWuthHush.ServicesCol
{
    public static class ApplicationServices
    {
        public static IServiceCollection MyServices(this IServiceCollection services)
        {
            services.AddDbContext<UsersContext>(opt => opt.UseSqlite("Data Source =mydata.db"));
            services.AddScoped<UsersServices>();
            return services;
        }
    }
}
