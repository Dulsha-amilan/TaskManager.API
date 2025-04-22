using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DueDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
