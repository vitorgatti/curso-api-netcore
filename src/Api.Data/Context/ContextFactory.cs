using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            var connectionString = "User ID=postgres;Password=Postgres2020!;Server=localhost;Port=5432;Database=dbApi;Integrated Security=true;Pooling=true;";
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return new MyContext(optionsBuilder.Options);
        }
    }
}
