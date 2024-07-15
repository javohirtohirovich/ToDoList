﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Data.Enums;

namespace ToDoList.Data.Models;

[Table("TaskItems")]
public class TaskItem
{
    [Key]
    public int TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }   
    public int Priority { get; set; }
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;
    public int? CategoryId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Category Category { get; set; }
    public List<TaskTag> TaskTags { get; set; }
}
