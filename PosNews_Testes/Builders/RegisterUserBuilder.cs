using Bogus;
using PosNews.Models;

namespace PosNews_Testes.Builders
{
    public class RegisterUserBuilder : Faker<RegisterUser> 
    {
        public RegisterUserBuilder()
        {
            RuleFor(r => r.UserName, f => f.Name.FullName());
            RuleFor(r => r.Password, "Abcdef@1234");
            RuleFor(r => r.UserName, f => f.PickRandom(new List<string>() { "admin", "user"}));
        }
    }
}
