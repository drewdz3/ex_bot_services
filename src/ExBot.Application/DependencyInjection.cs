using ExBot.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace ExBot.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //  register use cases
            services.AddTransient<IGetUserUc, GetUserUc>();
            services.AddTransient<IGetUsersUc, GetUsersUc>();
            services.AddTransient<ICreateUserUc, CreateUserUc>();
            services.AddTransient<IUpdateUserUc, UpdateUserUc>();
            services.AddTransient<IDeleteUserUc, DeleteUserUc>();

            return services;
        }
    }
}
