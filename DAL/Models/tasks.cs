using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class tasks
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Task_Date { get; set; }
      
        [ForeignKey(nameof(User))]
        public int user_id { get; set; }
    
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        // קשר לטבלת Users
        public users User { get; set; }

        // קשר לטבלת Categories
        public categories Category { get; set; }

        // קשר לטבלת TaskFiles
        public ICollection<taskFile> TaskFiles { get; set; } = new List<taskFile>();
    }
}
