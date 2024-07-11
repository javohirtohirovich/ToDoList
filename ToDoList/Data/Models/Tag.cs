using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Data.Entites;

[Table("Tags")]
public class Tag
{
    [Key]
    public int TagId { get; set; }
    public string Name { get; set; }
    public List<TaskTag> TaskTags { get; set; }
}
