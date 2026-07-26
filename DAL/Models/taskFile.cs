using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Represents a file attached to a task.
    /// </summary>
    public class taskFile
    {
        /// <summary>
        /// Primary key - unique identifier for the file record.
        /// </summary>
        [Key]
        public int FileId { get; set; }

        /// <summary>
        /// Foreign key - references the Tasks table.
        /// </summary>
        [ForeignKey(nameof(Task))]
        public int TaskId { get; set; }

        /// <summary>
        /// Original file name (e.g., "סיכום.pdf").
        /// </summary>
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        /// <summary>
        /// Direct link to the file in Google Cloud Storage.
        /// </summary>
        [Required]
        [StringLength(500)]
        [Url]
        public string FileUrl { get; set; }

        /// <summary>
        /// Upload date - optional timestamp of when the file was uploaded.
        /// </summary>
        public DateTime? UploadDate { get; set; }

        /// <summary>
        /// Navigation property - relationship to the Task.
        /// </summary>
        public tasks Task { get; set; }
    }
}
