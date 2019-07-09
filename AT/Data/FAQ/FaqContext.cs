using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.FAQ
{
    public class FaqContext : DbContext
    {
        public FaqContext(DbContextOptions<FaqContext> options) : base(options)
        {
        }

        public DbSet<FAQ> FAQs { get; set; }
       
       

    }
}
