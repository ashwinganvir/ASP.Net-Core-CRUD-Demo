using CRUD_Demo.DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRUD_Demo.DataAccess.Context
{
    public sealed class CRUD_DemoContext : DbContext
    {
        public CRUD_DemoContext(DbContextOptions options) 
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Contact> Contact { get; set; }
    }
}
