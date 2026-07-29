using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Models;
using Google.Cloud.Storage.V1;
using System.IO;
using System.IO.Compression;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for task file-related operations.
    /// </summary>
    public static class file_tasks_function
    {
        //--------------------------קבלת כל קבצי המשימות----------------------------
        /// <summary>
        /// Retrieves all task files from the database.
        /// </summary>
        /// <returns>A list of all task files.</returns>
        public static List<taskFile> GetAllTaskFiles()
        {
            using (AppDbContext DB = new AppDbContext())
            { 
                return DB.TaskFiles.ToList();
            }
        }
        //--------------------------קבלת קובץ על פי קוד הקובץ----------------------------
        /// <summary>
        /// Retrieves a specific task file by its ID.
        /// </summary>
        /// <param name="id">The ID of the task file to retrieve.</param>
        /// <returns>The task file if found; otherwise null.</returns>
        public static taskFile? GetTaskFileById(int id)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                taskFile file = DB.TaskFiles.FirstOrDefault(p => p.fileid == id)!;
                if (file != null)
                    return file;
                return null;
            }
        }

        //--------------------------קבלת קבצים על פי קוד המשימה----------------------------
        /// <summary>
        /// Retrieves all task files associated with a specific task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>A list of task files for the specified task.</returns>
        public static List<taskFile> GetTaskFilesByTaskId(int taskId)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                return DB.TaskFiles.Where(f => f.taskid == taskId).ToList();
            }
        }

        //--------------------------קבלת קבצים על פי קוד המשתמש----------------------------
        /// <summary>
        /// Retrieves all task files for tasks belonging to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of task files for all tasks owned by the user.</returns>
        public static List<taskFile> GetTaskFilesByUserId(int userId)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                return DB.TaskFiles
                    .Where(f => f.Task.user_id == userId)
                    .ToList();
            }
        }

        //--------------------------------הוספת קובץ למשימה----------------------------------
        /// <summary>
        /// Adds a new task file to the database.
        /// </summary>
        /// <param name="taskid">The ID of the task.</param>
        /// <param name="taskFile">The task file to add.</param>
        /// <param name="fileStream">The file stream to upload.</param>
        /// <returns>A list of all task files after the addition.</returns>
        //public static List<taskFile> AddNewTaskFile(int taskid, string fileName, Stream fileStream)
        //{
        //    // יצירת אובייקט המודל
        //    taskFile newTaskFile = new taskFile
        //    {
        //        TaskId = taskid,
        //        FileName = fileName,
        //        UploadDate = DateTime.Now
        //    };
        //    using (AppDbContext DB = new AppDbContext())
        //    {
        //        try
        //        {
        //            newTaskFile.FileUrl = Upload_to_the_cloud.UploadFile(taskid, newTaskFile.FileName, fileStream);
        //            DB.TaskFiles.Add(newTaskFile);
        //            DB.SaveChanges();
        //            return GetAllTaskFiles();
        //        }
        //        catch (Exception)
        //        {

        //            throw new Exception();
        //        }

        //    }
        //}
        public static List<taskFile> AddNewTaskFile(int taskid, string fileName, Stream fileStream)
        {
            try
            {
                // יצירת אובייקט המודל
                taskFile newTaskFile = new taskFile
                {
                    taskid = taskid,
                    filename = fileName,
                    uploaddate = DateTime.UtcNow
                };

                using (AppDbContext DB = new AppDbContext())
                {
                    // נקודת כשל פוטנציאלית 1: העלאה לענן
                    newTaskFile.fileurl = Upload_to_the_cloud.UploadFile(taskid, newTaskFile.filename, fileStream);

                    // נקודת כשל פוטנציאלית 2: שמירה במסד הנתונים
                    DB.TaskFiles.Add(newTaskFile);
                    DB.SaveChanges();

                    return GetAllTaskFiles();
                }
            }
            catch (Exception ex)
            {
                // 1. הדפסת השגיאה האמיתית והמלאה לחלון ה-Console השחור של השרת
                Console.WriteLine("========================================");
                Console.WriteLine("🔥 REAL ERROR IN DAL:");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("========================================");

                // 2. זריקת השגיאה המקורית למעלה כדי שסוואגר יציג אותה במפורש
                throw new Exception($"DAL Failed: {ex.Message}. Inner Exception: {ex.InnerException?.Message}", ex);
            }
        }

        //--------------------------------מחיקת קובץ----------------------------------
        /// <summary>
        /// Deletes a task file from the database.
        /// </summary>
        /// <param name="fileId">The ID of the task file to delete.</param>
        /// <returns>A list of all task files after the deletion.</returns>
        public static List<taskFile> DeleteTaskFile(int fileId)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                taskFile fileToDelete = DB.TaskFiles.FirstOrDefault(p => p.fileid == fileId)!;
                if (fileToDelete != null)
                {
                    DB.TaskFiles.Remove(fileToDelete);
                    DB.SaveChanges();
                }
                return GetAllTaskFiles();
            }
        }

        //--------------------------------הורדת קובץ----------------------------------
        /// <summary>
        /// Downloads a task file from Google Cloud Storage.
        /// </summary>
        /// <param name="fileId">The ID of the file to download.</param>
        /// <param name="outputStream">The output stream to write the file data to.</param>
        /// <returns>True if the file was successfully downloaded; otherwise false.</returns>
        public static bool DownloadTaskFile(int fileId, Stream outputStream)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                // Get the file from the database
                taskFile file = DB.TaskFiles.FirstOrDefault(f => f.fileid == fileId);
                if (file == null)
                    return false;

                try
                {
                    // Extract bucket name from URL
                    // URL format: https://storage.googleapis.com/{bucketName}/{objectName}
                    string fileUrl = file.fileurl;
                    if (string.IsNullOrEmpty(fileUrl))
                        return false;

                    // Parse the URL to extract bucket name and object name
                    Uri uri = new Uri(fileUrl);
                    string[] pathParts = uri.AbsolutePath.TrimStart('/').Split('/');
                    
                    if (pathParts.Length < 2)
                        return false;

                    string bucketName = pathParts[0];
                    string objectName = string.Join("/", pathParts.Skip(1));

                    // Download the file from Google Cloud Storage
                    var storage = StorageClient.Create();
                    storage.DownloadObject(bucketName, objectName, outputStream);

                    return true;
                }
                catch (Exception ex)
                {
                    // Log the exception if needed
                    System.Diagnostics.Debug.WriteLine($"Error downloading file: {ex.Message}");
                    return false;
                }
            }
        }

        //--------------------------------הורדת קובץ עם שם קובץ----------------------------------
        /// <summary>
        /// Downloads a task file and returns file information for the client.
        /// </summary>
        /// <param name="fileId">The ID of the file to download.</param>
        /// <param name="outputStream">The output stream to write the file data to.</param>
        /// <returns>A tuple containing success status and file name if successful.</returns>
        public static (bool Success, string FileName) DownloadTaskFileWithInfo(int fileId, Stream outputStream)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                // Get the file from the database
                taskFile file = DB.TaskFiles.FirstOrDefault(f => f.fileid == fileId);
                if (file == null)
                    return (false, null);

                try
                {
                    // Extract bucket name and object name from URL
                    string fileUrl = file.fileurl;
                    if (string.IsNullOrEmpty(fileUrl))
                        return (false, null);

                    Uri uri = new Uri(fileUrl);
                    string[] pathParts = uri.AbsolutePath.TrimStart('/').Split('/');
                    
                    if (pathParts.Length < 2)
                        return (false, null);

                    string bucketName = pathParts[0];
                    string objectName = string.Join("/", pathParts.Skip(1));

                    // Download the file from Google Cloud Storage
                    var storage = StorageClient.Create();
                    storage.DownloadObject(bucketName, objectName, outputStream);

                    return (true, file.filename);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error downloading file: {ex.Message}");
                    return (false, null);
                }
            }
        }

        //--------------------------------בדיקה האם קובץ קיים----------------------------------
        /// <summary>
        /// Checks if a task file exists in the database.
        /// </summary>
        /// <param name="fileId">The ID of the file to check.</param>
        /// <returns>True if the file exists; otherwise false.</returns>
        public static bool FileExists(int fileId)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                return DB.TaskFiles.Any(f => f.fileid == fileId);
            }
        }

        //--------------------------------הורדת כל קבצים למשימה ספציפית----------------------------------
        /// <summary>
        /// Downloads all task files for a specific task as a ZIP archive.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="outputStream">The output stream to write the ZIP archive to.</param>
        /// <returns>True if the files were successfully downloaded; otherwise false.</returns>
        public static bool DownloadAllTaskFiles(int taskId, Stream outputStream)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                try
                {
                    // Get all files for the task
                     var taskFiles = DB.TaskFiles.Where(f => f.taskid == taskId).ToList();
                    if (taskFiles == null || taskFiles.Count == 0)
                        return false;

                    // Create a ZIP archive
                    using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in taskFiles)
                        {
                            if (string.IsNullOrEmpty(file.fileurl))
                                continue;

                            try
                            {
                                // Parse the URL to extract bucket name and object name
                                Uri uri = new Uri(file.fileurl);
                                string[] pathParts = uri.AbsolutePath.TrimStart('/').Split('/');
          
                                    if (pathParts.Length < 2)
                                        continue;

                                string bucketName = pathParts[0];
                                string objectName = string.Join("/", pathParts.Skip(1));

                                // Download the file from Google Cloud Storage
                                var storage = StorageClient.Create();
                                using (var memoryStream = new MemoryStream())
                                {
                                    storage.DownloadObject(bucketName, objectName, memoryStream);
                                    memoryStream.Position = 0;

                                    // Add the file to the ZIP archive
                                    var entry = zipArchive.CreateEntry(file.filename);
                                    using (var entryStream = entry.Open())
                                    {
                                        memoryStream.CopyTo(entryStream);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error downloading file {file.filename}: {ex.Message}");
                                // Continue with the next file
                                continue;
                            }
                        }
                    }

                return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error creating ZIP archive: {ex.Message}");
                    return false;
                }
            }
        }

   //--------------------------------הורדת קבצים עם מידע----------------------------------
        /// <summary>
        /// Downloads all task files for a specific task and returns file information.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="outputStream">The output stream to write the ZIP archive to.</param>
        /// <returns>A tuple containing success status and list of file names.</returns>
        public static (bool Success, List<string> FileNames) DownloadAllTaskFilesWithInfo(int taskId, Stream outputStream)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                try
                {
                    // Get all files for the task
                    var taskFiles = DB.TaskFiles.Where(f => f.taskid == taskId).ToList();
                    var fileNames = new List<string>();
             
                    if (taskFiles == null || taskFiles.Count == 0)
                        return (false, fileNames);

                    // Create a ZIP archive
                    using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in taskFiles)
                        {
                            if (string.IsNullOrEmpty(file.fileurl))
                                continue;
        
                            try
                            {
                                // Parse the URL to extract bucket name and object name
                                Uri uri = new Uri(file.fileurl);
                                string[] pathParts = uri.AbsolutePath.TrimStart('/').Split('/');
 
                                if (pathParts.Length < 2)
                                    continue;

                                string bucketName = pathParts[0];
                                string objectName = string.Join("/", pathParts.Skip(1));

                                // Download the file from Google Cloud Storage
                                var storage = StorageClient.Create();
                                using (var memoryStream = new MemoryStream())
                                {
                                    storage.DownloadObject(bucketName, objectName, memoryStream);
                                    memoryStream.Position = 0;

                                    // Add the file to the ZIP archive
                                    var entry = zipArchive.CreateEntry(file.filename);
                                    using (var entryStream = entry.Open())
                                    {
                                        memoryStream.CopyTo(entryStream);
                                    }

                                    fileNames.Add(file.filename);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error downloading file {file.filename}: {ex.Message}");
                                // Continue with the next file
                                continue;
                            }
                        }
                    }   

                    return (true, fileNames);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error creating ZIP archive: {ex.Message}");
                    return (false, new List<string>());
                }

            }
        }
        //--------------------------קבלת קבצים של משתמש לפי תאריך מסוים----------------------------
        /// <summary>
        /// Retrieves all task files uploaded by a specific user on a specific date.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="targetDate">The date to filter by.</param>
        /// <returns>A list of task files uploaded on that date.</returns>
        public static List<taskFile> GetFilesByUserAndDate(int userId, DateTime targetDate)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                return DB.TaskFiles
                    .Where(f => f.Task.user_id == userId &&
                                f.uploaddate.HasValue &&
                                f.uploaddate.Value.Date == targetDate.Date)
                    .ToList();
            }
        }

    }
}
