﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EhodBoutiqueEnLigne.Models.Entities;
using System.Data;

namespace EhodBoutiqueEnLigne.Data
{
    public class EhodBDD : DbContext
    {
        private IDbConnection DbConnection { get; }

        public EhodBDD(DbContextOptions<EhodBDD> options, IConfiguration config)
            : base(options)
        {
            DbConnection = new SqlConnection(config.GetConnectionString("EhodBDD"));
        }

        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderLine> OrderLine { get; set; }
        public virtual DbSet<Product> Product { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConnection.ConnectionString, providerOptions => providerOptions.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.HasIndex(e => e.OrderId)
                    .HasName("IX_OrderLineEntity_OrderEntityId");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderLineEntity_OrderEntity_OrderEntityId").OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderLine__Produ__52593CB8").OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}


