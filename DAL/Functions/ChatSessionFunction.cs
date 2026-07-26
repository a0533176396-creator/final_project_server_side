using DAL.Data;
using DAL.Models;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for chat session-related operations.
  /// </summary>
    public static class ChatSessionFunction
    {
        static AppDbContext DB= new AppDbContext();
        public static List<ChatSession> GetAllDepartments()
        {
            return DB.ChatSessions.ToList();
        }

        //--------------------------קבלת שיחה על פי קוד השיחה----------------------------
        public static ChatSession? GetChatSessionById(short id)
        {
            ChatSession ChatSession = DB.ChatSessions.FirstOrDefault(p => p.Id == id)!;
            if (ChatSession != null)
                return ChatSession;
            return null;
        }

        //--------------------------------הוספת שיחה----------------------------------
        public static List<ChatSession> AddNewChatSession(ChatSession s)
        {
            //ChatSession department = new ChatSession();
            DB.ChatSessions.Add(s);
            DB.SaveChanges();
            return GetAllDepartments();
        }

        //----------------------------------עדכון שיחה----------------------------------
        public static List<ChatSession> UpdateChatSession(short idChatSession, ChatSession newChatSession)
        {
            ChatSession ChatSessionToUpdate = DB.ChatSessions.FirstOrDefault(p => p.Id == idChatSession)!;
            if (ChatSessionToUpdate != null)
            {
                ChatSessionToUpdate.Title = newChatSession.Title;
                ChatSessionToUpdate.Messages = newChatSession.Messages;
                ChatSessionToUpdate.UserId = newChatSession.UserId;
                ChatSessionToUpdate.CreatedAt = newChatSession.CreatedAt;
                ChatSessionToUpdate.UpdatedAt = newChatSession.UpdatedAt;
                DB.SaveChanges();
            }
            return GetAllDepartments();
        }

        //--------------------------------מחיקת שיחה----------------------------------
        public static List<ChatSession> DeleteChatSession(short idChatSession)
        {
            ChatSession ChatSessionToDelete = DB.ChatSessions.FirstOrDefault(p => p.Id == idChatSession)!;
            if (ChatSessionToDelete != null)
            {
                DB.ChatSessions.Remove(ChatSessionToDelete);
                DB.SaveChanges();
            }
            return GetAllDepartments();
        }
    }
}

