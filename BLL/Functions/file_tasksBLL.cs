using AutoMapper;
using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Functions
{
    /// <summary>
    /// Business Logic Layer function class for task file-related operations.
    /// </summary>
    public static class file_tasksBLL
    {
  //-----------------------------------GetAllTaskFiles-----------------------------------
        /// <summary>
     /// Retrieves all task files from the database.
        /// </summary>
     /// <returns>A list of all task file DTOs.</returns>
        public static List<taskFileDTO> GetAllTaskFiles()
     {
List<taskFile> allData = file_tasks_function.GetAllTaskFiles();
            return allData.Select(AppMapper.TaskFileToDto).ToList();
        }

        //-----------------------------------GetTaskFileById-----------------------------------
    /// <summary>
        /// Retrieves a specific task file by its ID.
     /// </summary>
   /// <param name="id">The ID of the task file to retrieve.</param>
        /// <returns>The task file DTO if found; otherwise null.</returns>
        public static taskFileDTO? GetTaskFileById(int id)
        {
   taskFile? file = file_tasks_function.GetTaskFileById(id);
     if (file == null)
       return null;
      return AppMapper.TaskFileToDto(file);
        }

        //-----------------------------------GetTaskFilesByTaskId-----------------------------------
        /// <summary>
        /// Retrieves all task files associated with a specific task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>A list of task file DTOs for the specified task.</returns>
     public static List<taskFileDTO> GetTaskFilesByTaskId(int taskId)
        {
   List<taskFile> allData = file_tasks_function.GetTaskFilesByTaskId(taskId);
     return allData.Select(AppMapper.TaskFileToDto).ToList();
     }

        // new: return DTOs with signed fileurl when serviceAccountPath provided
        public static List<taskFileDTO> GetTaskFilesByTaskIdSigned(int taskId, string serviceAccountPath, string bucketName, TimeSpan? ttl = null)
        {
            var files = GetTaskFilesByTaskId(taskId); // returns List<taskFileDTO>
            if (files == null || string.IsNullOrEmpty(serviceAccountPath)) return files;

            try
            {
                var signer = UrlSigner.FromServiceAccountPath(serviceAccountPath);
                var expire = ttl ?? TimeSpan.FromMinutes(30);

                foreach (var f in files)
                {
                    try
                    {
                        var objectName = ExtractObjectNameFromDto(f, bucketName);
                        if (string.IsNullOrEmpty(objectName)) continue;
                        f.fileurl = signer.Sign(bucketName, objectName, expire, HttpMethod.Get);
                    }
                    catch
                    {
                        // leave original f.fileurl on failure
                    }
                }
            }
            catch
            {
                // signer init failed - return original files
            }

            return files;
        }

        private static string ExtractObjectNameFromDto(taskFileDTO f, string bucketName)
        {
            if (f == null) return null;
            var candidate = (f.fileurl ?? f.filename ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(candidate)) return null;

            try
            {
                const string gsHost = "https://storage.googleapis.com/";
                if (candidate.StartsWith(gsHost, StringComparison.OrdinalIgnoreCase))
                {
                    var uri = new Uri(candidate);
                    var segments = uri.AbsolutePath.TrimStart('/').Split('/');
                    if (segments.Length >= 2) return string.Join("/", segments.Skip(1)); // skip bucket segment
                }

                if (!string.IsNullOrEmpty(bucketName))
                {
                    var idx = candidate.IndexOf(bucketName, StringComparison.OrdinalIgnoreCase);
                    if (idx >= 0)
                    {
                        var after = candidate.Substring(idx + bucketName.Length).TrimStart('/', '\\');
                        if (!string.IsNullOrEmpty(after)) return after;
                    }
                }

                return candidate.TrimStart('/', '\\');
            }
            catch
            {
                return candidate.TrimStart('/', '\\');
            }
        }


        //-----------------------------------GetTaskFilesByUserId-----------------------------------
        /// <summary>
        /// Retrieves all task files for tasks belonging to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of task file DTOs for all tasks owned by the user.</returns>
        public static List<taskFileDTO> GetTaskFilesByUserId(int userId)
        {
   List<taskFile> allData = file_tasks_function.GetTaskFilesByUserId(userId);
     return allData.Select(AppMapper.TaskFileToDto).ToList();
        }

    //-----------------------------------AddNewTaskFile-----------------------------------
        /// <summary>
        /// Adds a new task file to the database.
   /// </summary>
        /// <param name="taskId">The ID of the task.</param>
/// <param name="taskFileDTO">The task file DTO to add.</param>
        /// <param name="fileStream">The file stream to upload.</param>
        /// <returns>A list of all task file DTOs after the addition.</returns>
    public static List<taskFileDTO> AddNewTaskFile(int taskId, string fileName, Stream fileStream)
        {
    //taskFile newTaskFile = AppMapper.DtoToTaskFile(taskFileDTO);
       List<taskFile> allData = file_tasks_function.AddNewTaskFile(taskId, fileName, fileStream);
    return allData.Select(AppMapper.TaskFileToDto).ToList();
        }

        //-----------------------------------DeleteTaskFile-----------------------------------
    /// <summary>
   /// Deletes a task file from the database.
        /// </summary>
  /// <param name="fileId">The ID of the task file to delete.</param>
        /// <returns>A list of all task file DTOs after the deletion.</returns>
    public static List<taskFileDTO> DeleteTaskFile(int fileId)
        {
    List<taskFile> allData = file_tasks_function.DeleteTaskFile(fileId);
   return allData.Select(AppMapper.TaskFileToDto).ToList();
        }

        //-----------------------------------DownloadTaskFile-----------------------------------
        /// <summary>
        /// Downloads a task file from Google Cloud Storage.
    /// </summary>
        /// <param name="fileId">The ID of the file to download.</param>
        /// <param name="outputStream">The output stream to write the file data to.</param>
      /// <returns>True if the file was successfully downloaded; otherwise false.</returns>
   public static bool DownloadTaskFile(int fileId, Stream outputStream)
 {
     return file_tasks_function.DownloadTaskFile(fileId, outputStream);
  }

        //-----------------------------------DownloadTaskFileWithInfo-----------------------------------
      /// <summary>
      /// Downloads a task file and returns file information for the client.
  /// </summary>
        /// <param name="fileId">The ID of the file to download.</param>
        /// <param name="outputStream">The output stream to write the file data to.</param>
        /// <returns>A tuple containing success status and file name if successful.</returns>
        public static (bool Success, string FileName) DownloadTaskFileWithInfo(int fileId, Stream outputStream)
        {
   return file_tasks_function.DownloadTaskFileWithInfo(fileId, outputStream);
        }

        //-----------------------------------FileExists-----------------------------------
        /// <summary>
        /// Checks if a task file exists in the database.
        /// </summary>
        /// <param name="fileId">The ID of the file to check.</param>
        /// <returns>True if the file exists; otherwise false.</returns>
        public static bool FileExists(int fileId)
        {
            return file_tasks_function.FileExists(fileId);
        }

        //-----------------------------------DownloadAllTaskFiles-----------------------------------
        /// <summary>
        /// Downloads all task files for a specific task as a ZIP archive.
     /// </summary>
        /// <param name="taskId">The ID of the task.</param>
   /// <param name="outputStream">The output stream to write the ZIP archive to.</param>
        /// <returns>True if the files were successfully downloaded; otherwise false.</returns>
        public static bool DownloadAllTaskFiles(int taskId, Stream outputStream)
        {
   return file_tasks_function.DownloadAllTaskFiles(taskId, outputStream);
        }

 //-----------------------------------DownloadAllTaskFilesWithInfo-----------------------------------
        /// <summary>
    /// Downloads all task files for a specific task and returns file information.
   /// </summary>
    /// <param name="taskId">The ID of the task.</param>
        /// <param name="outputStream">The output stream to write the ZIP archive to.</param>
        /// <returns>A tuple containing success status and list of file names.</returns>
        public static (bool Success, List<string> FileNames) DownloadAllTaskFilesWithInfo(int taskId, Stream outputStream)
        {
return file_tasks_function.DownloadAllTaskFilesWithInfo(taskId, outputStream);
      }
    }
}
