﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Data.Models;

[Table("TaskTags")]
public class TaskTag
{
    public int TaskId { get; set; }
    public TaskItem Task { get; set; }
    public int TagId { get; set; }
    public Tag Tag { get; set; }
}
