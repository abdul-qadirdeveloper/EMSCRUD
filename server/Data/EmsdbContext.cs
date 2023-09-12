using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using Ems.Models.Emsdb;

namespace Ems.Data
{
  public partial class EmsdbContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public EmsdbContext(DbContextOptions<EmsdbContext> options):base(options)
    {
    }

    public EmsdbContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);



        builder.Entity<Ems.Models.Emsdb.Employee>()
              .Property(p => p.DateOfBirth)
              .HasColumnType("date");

        builder.Entity<Ems.Models.Emsdb.Employee>()
              .Property(p => p.Id)
              .HasPrecision(10, 0);
        this.OnModelBuilding(builder);
    }


    public virtual DbSet<Ems.Models.Emsdb.Employee> Employees
    {
      get;
      set;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
    }
  }
}
