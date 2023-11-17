using Geria.Data.Domain.Model.Calculator.Entities;
using Geria.Data.Domain.Model.Calculator.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Infrastructure.Services.Calculatormanagement
{
    public class CalculatorServices : ICalculatorServices
    {
        private readonly IInputDataRepository _inputDataRepository;

        public CalculatorServices(IInputDataRepository inputDataRepository)
        {
            _inputDataRepository = inputDataRepository;
        }
        public async Task<InputData> Create(InputData input)
        {
            _inputDataRepository.Add(input);
            await _inputDataRepository.UnitOfWork.SaveChangesAsync();
            return input;
        }

        public IEnumerable<InputData> GetAllByUser(string UserName)
        {
            if (UserName == null) throw new ArgumentNullException(nameof(UserName));
            var query = _inputDataRepository.GetMany(x => x.UserName == UserName);
            return query.ToList();
        }

        public decimal Calculation(int num1, int num2, string sign)
        {
            decimal sum = 0.00m;
            if (sign == "+")
            {
                sum = num1 + num2;
            }
            else if(sign == "-")
            {
                sum = num1 - num2;
            }
            else if(sign == "*")
            {
                sum = num1 * num2;
            }
            else if(sign == "/")
            {
                sum = num1 / num2;
            }
            return sum;
        }

        public IEnumerable<InputData> GetAll()
        {
            return _inputDataRepository.GetAll(x => x.UserName == "");
        }
    }
}
