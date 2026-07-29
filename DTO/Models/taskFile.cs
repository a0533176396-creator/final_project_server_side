using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Models
{
    /// <summary>
    /// DTO for taskFile entity - represents a file attached to a task.
    /// </summary>
    public class taskFileDTO
    {
        /// <summary>
        /// Primary key - unique identifier for the file record.
        /// </summary>
        public int fileid { get; set; }

        /// <summary>
      /// Foreign key - references the Task.
     /// </summary>
 public int taskid { get; set; }

  /// <summary>
   /// Original file name (e.g., "סיכום.pdf").
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// Direct link to the file in Google Cloud Storage.
        /// </summary>
        //[Column("fileurl")] // או "fileurl"

        public string fileurl { get; set; }

        /// <summary>
        /// Upload date - optional timestamp of when the file was uploaded.
        /// </summary>
        public DateTime? uploaddate { get; set; }
    }
}
