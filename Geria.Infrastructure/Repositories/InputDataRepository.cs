using Geria.Data.Domain.Model.Calculator.Entities;
using Geria.Data.Domain.Model.Calculator.Repositories;
using Geria.Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Infrastructure.Repositories
{
    public class InputDataRepository : RepositoryBase<InputData>, IInputDataRepository
    {
        public InputDataRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
