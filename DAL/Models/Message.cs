using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Message
    {
        public int Id { get; set; }
        
        [ForeignKey(nameof(ChatSession))]
        public int SessionId { get; set; }
        
        public SenderRole Role { get; set; } // "user" או "assistant"
        public string ContentURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // קשר לטבלת ChatSession
        public ChatSession ChatSession { get; set; }
    }
}
