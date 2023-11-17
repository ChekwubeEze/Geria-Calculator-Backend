using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Infrastructure.Infrastructure
{
    public class DatabaseFactory : Disposable , IDatabaseFactory
    {
        private readonly string _connectionString;
        private GeriaCalculatorContext _dataContext;
        public DatabaseFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public GeriaCalculatorContext Get()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GeriaCalculatorContext>();
            optionsBuilder.UseSqlServer(_connectionString, builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), errorNumbersToAdd: null));
            return _dataContext ?? (_dataContext = new GeriaCalculatorContext(optionsBuilder.Options));
        }

        protected override void DisposeCore(bool dispose)
        {
            if (_dataContext == null) return;
            _dataContext.Database.CloseConnection();
            _dataContext?.Dispose();
        }
    }
}
