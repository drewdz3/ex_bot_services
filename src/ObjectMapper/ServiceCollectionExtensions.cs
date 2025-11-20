using Microsoft.Extensions.DependencyInjection;

namespace ObjectMapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMapping<TSource, TTarget>(this IServiceCollection services, bool andBack = false)
            where TSource : class
            where TTarget : class
        {
            services.AddScoped<IConversionSet<TSource, TTarget>, ConversionSet<TSource, TTarget>>();
            if (andBack)
            {
                services.AddScoped<IConversionSet<TTarget, TSource>, ConversionSet<TTarget, TSource>>();
            }
            return services;
        }
    }
}
