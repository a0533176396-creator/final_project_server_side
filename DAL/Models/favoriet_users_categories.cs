using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class favoriet_users_categories
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int user_id { get; set; }

        [ForeignKey(nameof(Category))]
        public int category_id { get; set; }

        // קשר לטבלת Users
        public users User { get; set; }

        // קשר לטבלת Categories
        public categories Category { get; set; }
    }
}
