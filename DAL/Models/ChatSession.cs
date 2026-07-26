using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class ChatSession
    {
        public int Id { get; set; }
 
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
    
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // קשר לטבלת Users
        public users User { get; set; }

        // קשר לטבלת Messages
        public ICollection<Message> Messages { get; set; }
    }
}
