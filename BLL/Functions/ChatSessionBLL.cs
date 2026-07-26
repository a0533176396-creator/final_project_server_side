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
        //-----------------------------------GetAllChatSessions-----------------------------------
        public static List<ChatSessionDTO> GetAllChatSessions()
        {
            List<ChatSession> allData = ChatSessionFunction.GetAllDepartments();
            return allData.Select(AppMapper.ChatSessionToDto).ToList();
        }

        //-----------------------------------GetChatSessionById-----------------------------------
        public static ChatSessionDTO? GetChatSessionById(short id)
        {
            ChatSession? chatSession = ChatSessionFunction.GetChatSessionById(id);
            if (chatSession == null)
                return null;
            return AppMapper.ChatSessionToDto(chatSession);
        }

        //-----------------------------------AddNewChatSession-----------------------------------
        public static List<ChatSessionDTO> AddNewChatSession(ChatSessionDTO newChatSession)
        {
            ChatSession newChatSessionTBL = AppMapper.DtoToChatSession(newChatSession);
            List<ChatSession> allData = ChatSessionFunction.AddNewChatSession(newChatSessionTBL);
            return allData.Select(AppMapper.ChatSessionToDto).ToList();
        }

        //-----------------------------------UpdateChatSession-----------------------------------
        public static List<ChatSessionDTO> UpdateChatSession(short idChatSession, ChatSessionDTO newChatSession)
        {
            ChatSession newChatSessionTBL = AppMapper.DtoToChatSession(newChatSession);
            List<ChatSession> allData = ChatSessionFunction.UpdateChatSession(idChatSession, newChatSessionTBL);
            return allData.Select(AppMapper.ChatSessionToDto).ToList();
        }

        //-----------------------------------DeleteChatSession-----------------------------------
        public static List<ChatSessionDTO> DeleteChatSession(short idChatSession)
        {
            List<ChatSession> allData = ChatSessionFunction.DeleteChatSession(idChatSession);
            return allData.Select(AppMapper.ChatSessionToDto).ToList();
        }
    }
}
