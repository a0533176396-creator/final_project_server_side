using AutoMapper;
using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace BLL.Functions
{
    /// <summary>
    /// Business Logic Layer function class for message-related operations.
    /// </summary>
    public static class MessageBLL
    {
        //-----------------------------------GetAllMessages-----------------------------------
        public static List<MessageDTO> GetAllMessages()
        {
            List<Message> allData = MessageFunction.GetAllMessages();
 return allData.Select(AppMapper.MessageToDto).ToList();
        }

   //-----------------------------------GetMessageById-----------------------------------
        public static MessageDTO? GetMessageById(int id)
  {
          Message? message = MessageFunction.GetMessageById(id);
          if (message == null)
    return null;
      return AppMapper.MessageToDto(message);
        }

  //-----------------------------------AddNewMessage-----------------------------------
  public static List<MessageDTO> AddNewMessage(MessageDTO newMessage)
     {
            Message newMessageTBL = AppMapper.DtoToMessage(newMessage);
      List<Message> allData = MessageFunction.AddNewMessage(newMessageTBL);
     return allData.Select(AppMapper.MessageToDto).ToList();
        }

    //-----------------------------------UpdateMessage-----------------------------------
  public static List<MessageDTO> UpdateMessage(int idMessage, MessageDTO newMessage)
        {
         Message newMessageTBL = AppMapper.DtoToMessage(newMessage);
            List<Message> allData = MessageFunction.UpdateMessage(idMessage, newMessageTBL);
        return allData.Select(AppMapper.MessageToDto).ToList();
        }

    //-----------------------------------DeleteMessage-----------------------------------
     public static List<MessageDTO> DeleteMessage(int idMessage)
        {
            List<Message> allData = MessageFunction.DeleteMessage(idMessage);
            return allData.Select(AppMapper.MessageToDto).ToList();
     }
    }
}
