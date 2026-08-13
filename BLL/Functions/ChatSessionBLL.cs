using AutoMapper;
using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace BLL.Functions
{
    /// <summary>
    /// Business Logic Layer function class for chat session-related operations.
    /// </summary>
    public static class ChatSessionBLL
    {
        // קבלת כל השיחות של משתמש
        public static List<ChatSession> GetUserSessions(int userId)
        {
            return ChatSessionFunction.GetChatSessionsByUserId(userId);
        }

        // פתיחת שיחה חדשה
        public static ChatSession CreateNewSession(int userId, string title)
        {
            ChatSession newSession = new ChatSession
            {
                UserId = userId,
                Title = title
            };

            return ChatSessionFunction.AddNewChatSession(newSession);
        }

        // מחיקת שיחה
        public static bool DeleteSession(int sessionId)
        {
            return ChatSessionFunction.DeleteChatSession(sessionId);
        }
    }
}
