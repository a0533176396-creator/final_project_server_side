using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTO.Models
{
    public class tasksDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Task_Date { get; set; }
        public int user_id { get; set; }
        public string user_first_name { get; set; }   
        public string user_last_name { get; set; }
        //public string File_path { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string color { get; set; }
    }
}
