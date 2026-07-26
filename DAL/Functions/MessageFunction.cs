using DAL.Data;
using DAL.Models;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for message-related operations.
    /// </summary>
    public static class MessageFunction
    {
        static AppDbContext DB = new AppDbContext();

        //--------------------------קבלת כל ההודעות----------------------------
        public static List<Message> GetAllMessages()
        {
            return DB.Messages.ToList();
        }

        //--------------------------קבלת הודעה על פי קוד ההודעה----------------------------
        public static Message? GetMessageById(int id)
        {
            Message Message = DB.Messages.FirstOrDefault(p => p.Id == id)!;
            if (Message != null)
                return Message;
            return null;
        }

        //--------------------------------הוספת הודעה----------------------------------
        public static List<Message> AddNewMessage(Message m)
        {
            DB.Messages.Add(m);
            DB.SaveChanges();
            return GetAllMessages();
        }

        //----------------------------------עדכון הודעה----------------------------------
        public static List<Message> UpdateMessage(int idMessage, Message newMessage)
        {
            Message MessageToUpdate = DB.Messages.FirstOrDefault(p => p.Id == idMessage)!;
            if (MessageToUpdate != null)
            {
                MessageToUpdate.Role = newMessage.Role;
                MessageToUpdate.Content = newMessage.Content;
                MessageToUpdate.CreatedAt = newMessage.CreatedAt;
                MessageToUpdate.SessionId = newMessage.SessionId;
                DB.SaveChanges();
            }
            return GetAllMessages();
        }

        //--------------------------------מחיקת הודעה----------------------------------
        public static List<Message> DeleteMessage(int idMessage)
        {
            Message MessageToDelete = DB.Messages.FirstOrDefault(p => p.Id == idMessage)!;
            if (MessageToDelete != null)
            {
                DB.Messages.Remove(MessageToDelete);
                DB.SaveChanges();
            }
            return GetAllMessages();
        }
    }
}
