﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using Repozytorium.IRepo;

namespace Repozytorium.Models
{
  public class OglContext : IdentityDbContext, IOglContext
  {
    public OglContext() : base("DefaultConnection") { }

    public static OglContext Create()
    {
      return new OglContext();
    }

    public DbSet<Kategoria>   Kategorie { get; set; }
    public DbSet<Ogloszenie>  Ogloszenia { get; set; }
    public DbSet<Uzytkownik>  Uzytkownicy { get; set; }
    public DbSet<Ogloszenie_Kategoria> Ogloszenie_Kategoria { get; set; }
    public DbSet<Zdjecie>     Zdjecia         { get; set; }
    public DbSet<ZdjeciePlik> ZdjeciaPliki    { get; set; }
    public DbSet<Edytor> Edytor { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
      modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
      modelBuilder.Entity<Ogloszenie>().HasRequired(x => x.Uzytkownik)
        .WithMany(x => x.Ogloszenia)
        .HasForeignKey(x => x.UzytkownikId)
        .WillCascadeOnDelete(true);


      //// Potrzebne dla klas Identity    
      //base.OnModelCreating(modelBuilder);
      //// using System.Data.Entity.ModelConfiguration.Conventions;    
      //// Wyłącza konwencję, która automatycznie tworzy liczbę mnogą dla nazw tabel  w bazie danych    
      //// Zamiast Kategorie zostałaby utworzona tabela o nazwie Kategories    
      //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
      //// Wyłącza konwencję CascadeDelete    
      //// CascadeDelete zostanie włączone za pomocą Fluent API    
      //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
      //// Używa się Fluent API, aby ustalić powiązanie pomiędzy tabelami    
      //// i włączyć CascadeDelete dla tego powiązania    
      //modelBuilder.Entity<Ogloszenie>().HasRequired(x => x.Uzytkownik)
      //  .WithMany(x => x.Ogloszenia)
      //  .HasForeignKey(x => x.UzytkownikId)
      //  .WillCascadeOnDelete(true);
    }

  }
}