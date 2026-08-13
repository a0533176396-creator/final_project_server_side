using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Models
{
    // ==========================================================
    // מחלקת עזר (Model) לקבלת הנתונים מהבקשה (Request)
    // ==========================================================
    public class SendMessageRequest
    {
        public int SessionId { get; set; }
        public string Text { get; set; }
    }
}
