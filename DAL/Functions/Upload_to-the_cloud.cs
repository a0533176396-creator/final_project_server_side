using Google.Apis.Auth.OAuth2; // ⚠️ שימו לב להוספת ה-using הזה עבור GoogleCredential
using Google.Cloud.Storage.V1;
using System;
using System.IO;

public class Upload_to_the_cloud
{
    private static readonly string _bucketName = "final-tasks-project-files-2026"; // שם המאגר שלכם

    public static string UploadFile(int taskId, string fileName, Stream fileStream)
    {
        // 1. הגדרת נתיב יחסי לקובץ ה-JSON בתיקיית Secrets של פרויקט הריצה (ה-Web API)
        string credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "Secrets", "google-gcs-key.json");

        // 2. טעינת ההרשאות ישירות מתוך הקובץ
        GoogleCredential credential = GoogleCredential.FromFile(credentialPath);

        // 3. התחברות לגוגל קלאוד באמצעות ההרשאות שנטענו מהקובץ
        var storage = StorageClient.Create(credential);

        // 4. פירוק שם הקובץ ובניית שם ייחודי עם חותמת זמן (מונע דריסה ושומר על הסיומת)
        string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName); // למשל: "summary"
        string extension = Path.GetExtension(fileName); // למשל: ".pdf"
        string dateStr = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        string newFileName = $"{nameWithoutExtension}_{dateStr}{extension}";
        string objectName = $"tasks/{taskId}/{newFileName}";
        // תוצאה לדוגמה: tasks/105/summary_2026-07-27_14-30-00.pdf

        // 5. העלאת הקובץ בפועל לענן של גוגל
        storage.UploadObject(_bucketName, objectName, null, fileStream);

        // 6. יצירת הקישור הישיר לקובץ והחזרתו
        string publicUrl = $"https://storage.googleapis.com/{_bucketName}/{objectName}";
        return publicUrl;
    }
}
