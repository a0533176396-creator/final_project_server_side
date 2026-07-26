using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class categories
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        //[ForeignKey(nameof(ParentCategory))]
        public int? father_id { get; set; }

        // שים את ה-ForeignKey כאן, מעל מאפיין הניווט!
        [ForeignKey(nameof(father_id))]

        // קשר לקטגוריה ההורה (Self-referencing relationship)
        public categories? ParentCategory { get; set; }

        // קשר לקטגוריות-ילדות
        public ICollection<categories>? ChildCategories { get; set; } = new List<categories>();

        // קשר לטבלת Tasks
        public ICollection<tasks> Tasks { get; set; } = new List<tasks>();

        // קשר לטבלת FavoriteUserCategories
        public ICollection<favoriet_users_categories> FavoriteUserCategories { get; set; } = new List<favoriet_users_categories>();
    }
}
