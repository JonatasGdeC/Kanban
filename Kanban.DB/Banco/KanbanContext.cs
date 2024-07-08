using Kanban.Shared.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Kanban.DB.Banco
{
    public class KanbanContext : DbContext
    {
        public DbSet<Boards> Boards { get; set; }
        public DbSet<Columns> Columns { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Subtasks> Subtasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Columns>()
                .HasOne(c => c.Board)
                .WithMany(b => b.Columns)
                .HasForeignKey(c => c.BoardId);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Column)
                .WithMany(c => c.Tasks) // Aqui ajustamos para c.Tasks
                .HasForeignKey(t => t.ColumnId);

            modelBuilder.Entity<Subtasks>()
                .HasOne(s => s.Task)
                .WithMany(t => t.Subtasks)
                .HasForeignKey(s => s.TaskId);

            base.OnModelCreating(modelBuilder);
        }
        
        private string _connectionString = "Server=tcp:projectkanbanserver.database.windows.net,1433;Initial Catalog=Kanban;Persist Security Info=False;User ID=Jonatas;Password=Carvalho@2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public KanbanContext() {}
        
        public KanbanContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
