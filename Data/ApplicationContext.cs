using Microsoft.EntityFrameworkCore;
using Veiculos.Api.Controllers;
using Veiculos.Api.Entity;

namespace Veiculos.Api.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<VeiculosEntity> Veiculos { get; set; }
    }
}
