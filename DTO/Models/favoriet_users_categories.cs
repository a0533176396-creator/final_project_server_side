using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTO.Models
{
    public class favoriet_users_categoriesDTO
    {
        public int Id { get; set; }
        public int user_id { get; set; }
        public int category_id { get; set; }
    }
}
