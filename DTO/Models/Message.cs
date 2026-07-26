using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTO.Models
{
  public class MessageDTO
    {
 public int Id { get; set; }
        public int SessionId { get; set; }
        public string Role { get; set; } // "user" ае "assistant"
        public string ContentURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
