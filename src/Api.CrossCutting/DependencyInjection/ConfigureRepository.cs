using Api.Data.Context;
using Api.Data.Repository;
using Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.DependencyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            serviceCollection.AddEntityFrameworkNpgsql().AddDbContext<MyContext>(
                options => options.UseNpgsql("User ID=postgres;Password=Postgres2020!;Server=localhost;Port=5432;Database=dbApi;Integrated Security=true;Pooling=true;")
            );
        }
    }
}
