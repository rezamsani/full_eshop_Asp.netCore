using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Contexts
{
    public interface IIdentityDataBaseContext
    {
        DbSet<Domain.Entities.User> Users { get; set; }
    }
}
