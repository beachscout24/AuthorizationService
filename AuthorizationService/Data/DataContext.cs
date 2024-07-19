using AuthorizationService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationService.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {  
    }

    DbSet<User> users { get; set; }
}
