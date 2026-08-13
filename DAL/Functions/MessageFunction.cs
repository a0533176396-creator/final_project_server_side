using DAL.Data;
using DAL.Models;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for message-related operations.
    /// </summary>
    public static class MessageFunction
    {
        //--------------------------קבלת כל ההודעות של שיחה ספציפית (לצורך שליחה ל-AI)----------------------------
        public static List<Message> GetMessagesBySessionId(int sessionId)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                // חשוב מאוד: לסדר לפי תאריך יצירה כדי שה-AI יקבל את השיחה בסדר כרונולוגי נכון
                return DB.Messages
                         .Where(m => m.SessionId == sessionId)
                         .OrderBy(m => m.CreatedAt)
                         .ToList();
            }
        }

        //--------------------------קבלת הודעה ספציפית על פי קוד----------------------------
        public static Message? GetMessageById(int id)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                Message msg = DB.Messages.FirstOrDefault(m => m.Id == id)!;
                return msg;
            }
        }

        //--------------------------------הוספת הודעה חדשה (משתמש או AI)----------------------------------
        public static List<Message> AddNewMessage(Message newMessage)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                // האובייקט newMessage כבר מכיל את ה-Enum (Role) ואת הקישור לדלי (ContentUrl)
                DB.Messages.Add(newMessage);
                DB.SaveChanges();

                // נחזיר את כל היסטוריית השיחה המעודכנת
                return GetMessagesBySessionId(newMessage.SessionId);
            }
        }

        //--------------------------------מחיקת הודעה בודדת----------------------------------
        public static bool DeleteMessage(int messageId)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                Message msgToDelete = DB.Messages.FirstOrDefault(m => m.Id == messageId)!;
                if (msgToDelete != null)
                {
                    DB.Messages.Remove(msgToDelete);
                    DB.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        // הערה: בדרך כלל לא נהוג לעדכן (Update) הודעות בצ'אט היסטורי, 
        // לכן הפונקציה הזו הושמטה, אך ניתן להוסיפה באותה תבנית אם נדרש.

    }
}
