using BLL.Functions;
using DAL.Functions;
using DAL.Models;
using DTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace tasks_project.Controllers
{
    /// <summary>
    /// API controller for task file-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileTasksController : ControllerBase
    {
        //--------------------------------------------
        // קבלת כל קבצי המשימות
        //--------------------------------------------
 /// <summary>
        /// Retrieves all task files from the database.
        /// </summary>
  [HttpGet("GetAllTaskFiles")]
        public IActionResult GetAllTaskFiles()
      {
    return Ok(file_tasksBLL.GetAllTaskFiles());
  }

        //--------------------------------------------
        // קבלת קובץ על פי קוד הקובץ
        //--------------------------------------------
/// <summary>
     /// Retrieves a specific task file by its ID.
     /// </summary>
        /// <param name="fileId">The ID of the task file to retrieve.</param>
  [HttpGet("GetTaskFileById/{fileId}")]
     public IActionResult GetTaskFileById(int fileId)
        {
            return Ok(file_tasksBLL.GetTaskFileById(fileId));
    }

        //--------------------------------------------
        // קבלת קבצים על פי קוד המשימה
        //--------------------------------------------
  /// <summary>
        /// Retrieves all task files associated with a specific task.
    /// </summary>
  /// <param name="taskId">The ID of the task.</param>
        [HttpGet("GetTaskFilesByTaskId/{taskId}")]
  public IActionResult GetTaskFilesByTaskId(int taskId)
        {
            return Ok(file_tasksBLL.GetTaskFilesByTaskId(taskId));
        }

        //--------------------------------------------
        // קבלת קבצים על פי קוד המשתמש
        //--------------------------------------------
        /// <summary>
        /// Retrieves all task files for tasks belonging to a specific user.
   /// </summary>
      /// <param name="userId">The ID of the user.</param>
  [HttpGet("GetTaskFilesByUserId/{userId}")]
        public IActionResult GetTaskFilesByUserId(int userId)
        {
  return Ok(file_tasksBLL.GetTaskFilesByUserId(userId));
        }

        //--------------------------------------------
        // הוספת קובץ למשימה
        //--------------------------------------------
        /// <summary>
        /// Adds a new task file to the database.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="taskFileDTO">The task file DTO with file information.</param>
        /// // 1. שכבת ה-API (ה-Controller) - חשופה ל-Swagger ול-React
        [HttpPost("{taskId}/upload-file")]
        public IActionResult UploadFile([FromRoute] int taskId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("לא נבחר קובץ.");

            // פתיחת ה-Stream מתוך ה-IFormFile
            using (Stream fileStream = file.OpenReadStream())
            {
                // קריאה ל-BLL שמחזירה DTOs
                // (החליפו את FileTasksBLL בשם המחלקה האמיתי שלכם ב-BLL)
                var updatedListDto = file_tasksBLL.AddNewTaskFile(taskId, file.FileName, fileStream);

                return Ok(updatedListDto);
            }
        }



        //      [HttpPost("AddNewTaskFile/{taskId}")]
        //      public IActionResult AddNewTaskFile(int taskId, [FromForm] taskFileDTO taskFileDTO, [FromForm] IFormFile file)
        //      {
        //if (file == null || file.Length == 0)
        //       {
        //  return BadRequest("No file provided");
        //     }

        //     try
        //     {
        //      using (var stream = file.OpenReadStream())
        //    {
        //  var result = file_tasksBLL.AddNewTaskFile(taskId, taskFileDTO, stream);
        //           return Ok(result);
        //      }
        // }
        //  catch (Exception ex)
        //     {
        // return StatusCode(500, $"Error uploading file: {ex.Message}");
        //          }
        //}

        //--------------------------------------------
        // מחיקת קובץ
        //--------------------------------------------
        /// <summary>
        /// Deletes a task file from the database.
        /// </summary>
        /// <param name="fileId">The ID of the task file to delete.</param>
        [HttpDelete("DeleteTaskFile/{fileId}")]
        public IActionResult DeleteTaskFile(int fileId)
     {
      return Ok(file_tasksBLL.DeleteTaskFile(fileId));
        }

    //--------------------------------------------
        // הורדת קובץ יחיד
        //--------------------------------------------
        /// <summary>
   /// Downloads a single task file.
        /// </summary>
        /// <param name="fileId">The ID of the file to download.</param>
        [HttpGet("DownloadTaskFile/{fileId}")]
  public IActionResult DownloadTaskFile(int fileId)
     {
      try
      {
   var (success, fileName) = file_tasksBLL.DownloadTaskFileWithInfo(fileId, Response.Body);

     if (!success)
                {
       return NotFound("File not found");
    }

      Response.ContentType = "application/octet-stream";
      Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");

     return Ok();
         }
  catch (Exception ex)
      {
                return StatusCode(500, $"Error downloading file: {ex.Message}");
          }
        }

        //--------------------------------------------
  // בדיקה האם קובץ קיים
 //--------------------------------------------
        /// <summary>
   /// Checks if a task file exists in the database.
        /// </summary>
        /// <param name="fileId">The ID of the file to check.</param>
        [HttpGet("FileExists/{fileId}")]
        public IActionResult FileExists(int fileId)
        {
    bool exists = file_tasksBLL.FileExists(fileId);
        return Ok(new { exists = exists });
        }

        //--------------------------------------------
        // הורדת כל קבצי המשימה כ-ZIP
   //--------------------------------------------
      /// <summary>
        /// Downloads all task files for a specific task as a ZIP archive.
     /// </summary>
      /// <param name="taskId">The ID of the task.</param>
        [HttpGet("DownloadAllTaskFiles/{taskId}")]
        public IActionResult DownloadAllTaskFiles(int taskId)
        {
            try
            {
  var memoryStream = new MemoryStream();
                bool success = file_tasksBLL.DownloadAllTaskFiles(taskId, memoryStream);

           if (!success)
         {
      return NotFound("No files found for this task");
      }

      memoryStream.Position = 0;
             return File(memoryStream.ToArray(), "application/zip", $"task_{taskId}_files.zip");
            }
    catch (Exception ex)
 {
          return StatusCode(500, $"Error downloading files: {ex.Message}");
            }
        }

        //--------------------------------------------
        // הורדת כל קבצי המשימה כ-ZIP עם מידע
      //--------------------------------------------
    /// <summary>
        /// Downloads all task files for a specific task and returns file information.
     /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        [HttpGet("DownloadAllTaskFilesWithInfo/{taskId}")]
        public IActionResult DownloadAllTaskFilesWithInfo(int taskId)
        {
    try
      {
var memoryStream = new MemoryStream();
       var (success, fileNames) = file_tasksBLL.DownloadAllTaskFilesWithInfo(taskId, memoryStream);

          if (!success)
           {
                return NotFound("No files found for this task");
      }

       memoryStream.Position = 0;
                var response = new
             {
  success = true,
 fileCount = fileNames.Count,
    fileNames = fileNames,
        zipData = Convert.ToBase64String(memoryStream.ToArray())
           };

     return Ok(response);
            }
          catch (Exception ex)
            {
              return StatusCode(500, $"Error downloading files: {ex.Message}");
       }
        }
    }
}
