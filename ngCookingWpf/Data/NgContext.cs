using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using apis.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Configuration;

namespace apis.Data
{
    public class NgContext : IdentityDbContext<Models.User, IdentityRole<Int32>, Int32>, INgContext
    {
        IConfigurationRoot _configuration;

        public DbSet<Ingredient> Ingredients { get; set; }

        public virtual DbSet<Recette> Recettes { get; set; }

        public DbSet<Categorie> Categories { get; set; }

        public DbSet<Commentaire> Commentaires { get; set; }

        public DbSet<IngredientRecette> IngredientsRecettes { get; set; }

        public NgContext(IConfigurationRoot configuration)
        {
            _configuration = configuration;

            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./ngCooking.db");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IngredientRecette>()
                .HasKey(t => new { t.IngredientId, t.RecetteId });

            builder.Entity<Recette>()
                .ToTable("Recette") //ForSqliteToTable("recette")
                .Ignore(r => r.Ingredients)
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.CreatorId)
                .HasPrincipalKey(u => u.Id);

            builder.Entity<IngredientRecette>()
                .ToTable("IngredientRecette") //ForSqliteToTable("IngredientRecette")
                .HasOne(ir => ir.Recette)
                .WithMany(r => r.IngredientsRecettes)
                .HasForeignKey(ir => ir.RecetteId)
                .HasPrincipalKey(r => r.Id);

            builder.Entity<IngredientRecette>()
                .HasOne(ir => ir.Ingredient)
                .WithMany(i => i.IngredientsRecettes)
                .HasForeignKey(ir => ir.IngredientId)
                .HasPrincipalKey(i => i.Id);

            builder.Entity<Commentaire>()
                .ToTable("Commentaire") //ForSqliteToTable("Commentaire")
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            builder.Entity<Commentaire>()
                .HasOne(c => c.Recette)
                .WithMany(r => r.Commentaires)
                .HasForeignKey(c => c.RecetteId);

            builder.Entity<User>()
                .ToTable("Utilisateur") //...
                .Ignore(u => u.Password);

            builder.Entity<Recette>()
                .Ignore(u => u.Calories);

            builder.Entity<Ingredient>()
                .ToTable("Ingredient"); //ForSqliteToTable("Ingredient");

            base.OnModelCreating(builder);
        }
    }
}
