using Geria.Core.Models;

namespace GeriaCalculatorApp.ViewModels
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public JsonWebToken Token { get; set; }
    }
}
