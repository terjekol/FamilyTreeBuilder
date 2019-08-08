using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FamilyTreeBuilder.ModelsGenerated
{
    public partial class FamilyTreeContext : DbContext
    {
        public FamilyTreeContext()
        {
        }

        public FamilyTreeContext(DbContextOptions<FamilyTreeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Person> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("host=localhost;database=FamilyTree;user id=postgres;password=abcd1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlHasEnum(null, "sex", new[] { "Male", "Female" })
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BirthDate)
                    .HasColumnName("birth_date")
                    .HasColumnType("date");

                entity.Property(e => e.DataOwnerId).HasColumnName("data_owner_id");

                entity.Property(e => e.DeathDate)
                    .HasColumnName("death_date")
                    .HasColumnType("date");

                entity.Property(e => e.Father).HasColumnName("father");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.IsMale).HasColumnName("is_male");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Mother).HasColumnName("mother");

                entity.HasOne(d => d.FatherNavigation)
                    .WithMany(p => p.InverseFatherNavigation)
                    .HasForeignKey(d => d.Father)
                    .HasConstraintName("person_father_fkey");

                entity.HasOne(d => d.MotherNavigation)
                    .WithMany(p => p.InverseMotherNavigation)
                    .HasForeignKey(d => d.Mother)
                    .HasConstraintName("person_mother_fkey");
            });
        }
    }
}
