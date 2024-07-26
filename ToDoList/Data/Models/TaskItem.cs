using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Data.Models;

[Table("TaskItems")]
public class TaskItem
{
    [Key]
    public int TaskId { get; set; }
    public string Task { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; } = false;
    public bool IsImportant { get; set; } = false;
    public int? CategoryId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Category Category { get; set; }
}
