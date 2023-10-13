using Bogus;
using PosNews.Models;

namespace PosNews_Testes.Builders
{
    public class RegisterUserBuilder : Faker<RegisterUser>
    {
        public RegisterUserBuilder()
        {
            RuleFor(r => r.UserName, f => f.Name.FirstName())
            .RuleFor(r => r.Password, f => "senhaTeste@841")
            .RuleFor(r => r.Role, f => f.PickRandom(new List<string>() { "admin", "user" }));
        }
    }
}
