using DAL.Data;
using DAL.Models;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for chat session-related operations.
    /// </summary>
    public static class ChatSessionFunction
    {
        //--------------------------קבלת כל השיחות במערכת (למנהל)----------------------------
        public static List<ChatSession> GetAllChatSessions()
        {
            using (AppDbContext DB = new AppDbContext())
            {
                return DB.ChatSessions.ToList();
            }
        }

        //--------------------------קבלת כל השיחות של משתמש ספציפי----------------------------
        public static List<ChatSession> GetChatSessionsByUserId(int userId)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                // מחזיר את כל השיחות של הלקוח, מסודרות מהחדשה לישנה
                return DB.ChatSessions
                         .Where(cs => cs.UserId == userId)
                         .OrderByDescending(cs => cs.Id)
                         .ToList();
            }
        }

        //--------------------------קבלת שיחה ספציפית על פי קוד----------------------------
        public static ChatSession? GetChatSessionById(int id)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                ChatSession session = DB.ChatSessions.FirstOrDefault(cs => cs.Id == id)!;
                return session;
            }
        }

        //--------------------------------הוספת שיחה חדשה----------------------------------
        public static ChatSession AddNewChatSession(ChatSession newSession)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                DB.ChatSessions.Add(newSession);
                DB.SaveChanges();
                // נחזיר את האובייקט שנוצר כדי שהשכבה העליונה תקבל את ה-Id החדש שנוצר ב-DB
                return newSession;
            }
        }

        //----------------------------------עדכון כותרת שיחה----------------------------------
        public static ChatSession? UpdateChatSessionTitle(int sessionId, string newTitle)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                ChatSession sessionToUpdate = DB.ChatSessions.FirstOrDefault(cs => cs.Id == sessionId)!;
                if (sessionToUpdate != null)
                {
                    sessionToUpdate.Title = newTitle;
                    DB.SaveChanges();
                }
                return sessionToUpdate;
            }
        }

        //--------------------------------מחיקת שיחה (ימחק אוטומטית גם את ההודעות)----------------------------------
        public static bool DeleteChatSession(int sessionId)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                ChatSession sessionToDelete = DB.ChatSessions.FirstOrDefault(cs => cs.Id == sessionId)!;
                if (sessionToDelete != null)
                {
                    DB.ChatSessions.Remove(sessionToDelete);
                    DB.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}

