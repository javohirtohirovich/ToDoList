using Microsoft.EntityFrameworkCore;
using ToDoList.Data.Models;

namespace ToDoList.Data;

public class MainContext : DbContext
{
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Category> Categories { get; set; }

    public MainContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "todolist.db");
        optionsBuilder.UseSqlite($"Filename = {dbPath}");
    }
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //}
}
