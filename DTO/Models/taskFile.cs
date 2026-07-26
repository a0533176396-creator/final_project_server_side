using System;
using System.Collections.Generic;
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
        public int FileId { get; set; }

        /// <summary>
      /// Foreign key - references the Task.
     /// </summary>
 public int TaskId { get; set; }

  /// <summary>
   /// Original file name (e.g., "ριλεν.pdf").
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Direct link to the file in Google Cloud Storage.
         /// </summary>
      public string FileUrl { get; set; }

        /// <summary>
        /// Upload date - optional timestamp of when the file was uploaded.
        /// </summary>
        public DateTime? UploadDate { get; set; }
    }
}
