using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.IO;
using System.Text;

namespace DAL.Functions // (או ה-Namespace שבו המחלקה נמצאת כרגע)
{
    public class Upload_to_the_cloud
    {
        // הדלי המקורי לקבצי המשימות
        private static readonly string _bucketName = "final-tasks-project-files-2026";

        // הדלי החדש להיסטוריית הצ'אט
        private static readonly string _chatBucketName = "ai-chat-history-bucket-2026";

        // פונקציית העזר להתחברות
        private static StorageClient GetStorageClient()
        {
            string credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "Secrets", "google-gcs-key.json");
            GoogleCredential credential = GoogleCredential.FromFile(credentialPath);
            return StorageClient.Create(credential);
        }

        // ====================================================================
        // הפונקציה המקורית שלכם - ללא שינוי! (כדי לא לשבור קוד קיים)
        // ====================================================================
        public static string UploadFile(int taskId, string fileName, Stream fileStream)
        {
            var storage = GetStorageClient();

            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string dateStr = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            string newFileName = $"{nameWithoutExtension}_{dateStr}{extension}";
            string objectName = $"tasks/{taskId}/{newFileName}";

            storage.UploadObject(_bucketName, objectName, null, fileStream);

            return $"https://storage.googleapis.com/{_bucketName}/{objectName}";
        }

        // ====================================================================
        // פונקציה חדשה עבור הצ'אט - העלאת טקסט
        // ====================================================================
        public static string UploadChatText(int sessionId, string text, string role)
        {
            var storage = GetStorageClient();

            string dateStr = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
            string objectName = $"chats/session_{sessionId}/{role}_{dateStr}.txt";

            byte[] byteArray = Encoding.UTF8.GetBytes(text);
            using (var stream = new MemoryStream(byteArray))
            {
                storage.UploadObject(_chatBucketName, objectName, "text/plain", stream);
            }

            return $"https://storage.googleapis.com/{_chatBucketName}/{objectName}";
        }

        // ====================================================================
        // פונקציה חדשה עבור הצ'אט - הורדת טקסט
        // ====================================================================
        public static string DownloadChatText(string fileUrl)
        {
            var storage = GetStorageClient();

            // חילוץ הנתיב הפנימי מה-URL המלא
            string objectName = fileUrl.Substring(fileUrl.IndexOf("chats/"));

            using (var stream = new MemoryStream())
            {
                storage.DownloadObject(_chatBucketName, objectName, stream);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
