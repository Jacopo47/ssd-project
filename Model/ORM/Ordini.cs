using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

  public class DatabaseContext : DbContext
  {

    public DbSet<Ordini> Ordini { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite(Config.Instance.getConnectionString());
    }
  }

  public class Ordini
  {
      public long id { get; set; }
      public string customer { get; set; }
      public int time { get; set; }
      public int quant { get; set; }
  }
