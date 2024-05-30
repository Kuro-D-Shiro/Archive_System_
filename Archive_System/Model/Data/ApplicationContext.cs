using Microsoft.EntityFrameworkCore;

namespace Archive_System.Model.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<IssuedDocument> IssuedDocuments { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-FCNQA20;Database=Archive_Manager;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Document>()
                .HasMany(d => d.Subscribers)
                .WithMany(s => s.Documents)
                .UsingEntity<IssuedDocument>(
                    j => j
                        .HasOne(s => s.Subscriber)
                        .WithMany(i => i.IssuedDocuments)
                        .HasForeignKey(id => id.SubscriberId),
                    j => j
                        .HasOne(d => d.Document)
                        .WithMany(i => i.IssuedDocuments)
                        .HasForeignKey(id => id.DocumentId),
                    j =>
                    {
                        j.Property(pt => pt.IssueDate);
                        j.HasKey(t => t.Id);
                        j.Property(t => t.Id).ValueGeneratedOnAdd(); 
                        j.ToTable("IssueDocuments");
                    });

            modelBuilder
                .Entity<Document>()
                .HasOne(d => d.Cell)
                .WithOne(c => c.Document)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
