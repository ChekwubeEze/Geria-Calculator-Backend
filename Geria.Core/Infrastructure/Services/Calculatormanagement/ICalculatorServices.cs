using Geria.Data.Domain.Model.Calculator.Entities;
using Geria.Data.Domain.Model.UserManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Infrastructure.Services.Calculatormanagement
{
    public interface ICalculatorServices
    {
        Task<InputData> Create(InputData input);
        IEnumerable<InputData> GetAllByUser(string UserName);
        IEnumerable<InputData> GetAll();
        decimal Calculation(int num1, int num2, string sign);
    }
}
