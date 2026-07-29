using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Models
{
    public class usersDTO
    {
        public int Id { get; set; }
        public string sub { get; set; }
        public string? First_name { get; set; }
        public string? Last_name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public bool Wont_help { get; set; } = true;
    }
}
