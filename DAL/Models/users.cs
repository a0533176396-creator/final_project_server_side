using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class users
    {
        public int Id { get; set; }
        public string sub { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Wont_help { get; set; }=true;

        // קשר לטבלת Tasks
        public ICollection<tasks> Tasks { get; set; } = new List<tasks>();

        // קשר לטבלת ChatSessions
        public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();

        // קשר לטבלת FavoriteUserCategories
        public ICollection<favoriet_users_categories> FavoriteUserCategories { get; set; } = new List<favoriet_users_categories>();
    }
}
