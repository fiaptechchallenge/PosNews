using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Contextes
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) :base(options)
        {
            
        }
    }
}
