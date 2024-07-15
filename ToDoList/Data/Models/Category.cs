using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Data.Models;

[Table("Categories")]
public class Category
{
    [Key]
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public List<TaskItem> Tasks { get; set; }
}
