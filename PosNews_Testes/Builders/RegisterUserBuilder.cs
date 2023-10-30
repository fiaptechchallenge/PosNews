using Bogus;
using PosNews.Models;

namespace PosNews_Testes.Builders
{
    public class RegisterUserBuilder : Faker<RegisterUser>
    {
        public RegisterUserBuilder()
        {
            RuleFor(r => r.UserName, f => f.Internet.UserName())
            .RuleFor(r => r.Password, f => f.Internet.Password(length: 11, prefix: @"!0aA")) // Senha tem de ter letras (M e m), números e símbolos
            .RuleFor(r => r.Role, f => "admin");
        }
    }
}