using Geria.Data.Domain.Infrastruture;
using Geria.Data.Domain.Model.Calculator.Entities;
using Geria.Data.Domain.Model.UserManagement.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Infrastructure
{
    public class GeriaCalculatorContext : DbContext, IUnitOfWork
    {
        public DbSet<User> users { get; set; }
        public DbSet<InputData> InputDatas { get; set; }
        public GeriaCalculatorContext(DbContextOptions<GeriaCalculatorContext> contextOptions) : base(contextOptions) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeriaCalculatorContext).Assembly);
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
