using DAL;
using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Functions
{
    /// <summary>
    /// Business Logic Layer function class for message-related operations.
    /// </summary>
    public static class MessageBLL
    {
        //--------------------------שליפת היסטוריה והמרה ל-DTO----------------------------
        public static List<MessageDTO> GetSessionMessagesDTO(int sessionId)
        {
   List<Message> dalMessages = MessageFunction.GetMessagesBySessionId(sessionId);
          List<MessageDTO> dtoList = new List<MessageDTO>();

     foreach (var msg in dalMessages)
      {
       // 1. שימוש ב-AppMapper להמרה הבסיסית (Id, SessionId, Role, CreatedAt)
          MessageDTO dto = AppMapper.MessageToDto(msg);

 // 2. השלמת הנתון החסר (הורדה מהענן)
         dto.TextContent = Upload_to_the_cloud.DownloadChatText(msg.ContentURL);

        dtoList.Add(dto);
            }

 return dtoList;
        }

        //--------------------------בניית System Prompt דינמי----------------------------
        /// <summary>
        /// בנאי System Prompt דינמי שמשלב זמן אמת, נתוני משתמש, והנחיות מותנות
        /// </summary>
      private static string BuildDynamicSystemPrompt(int userId)
        {
            // 1. שליפת נתוני זמן אמת
             string currentTime = DateTime.Now.ToString("HH:mm");
            string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
          string currentDayOfWeek = DateTime.Now.DayOfWeek.ToString();

            // 2. שליפת פרופיל המשתמש מ-DAL
          var userProfile = usersFunction.GetUserById(userId.ToString());

 // 3. שליפת התובנות שה-AI למד על המשתמש
          var userInsightsPrompt = UserInsightFunction.BuildUserProfilePrompt(userId);

            // 4. בניית ה-System Prompt הדינמי
   var promptBuilder = new StringBuilder();

            promptBuilder.AppendLine("אתה עוזר וירטואלי חכם, מקצועי ואמור ומאוד. ");
    promptBuilder.AppendLine("תפקידך: לעזור למשתמש לנהל את המשימות שלו באופן הנכון לו, ");
            promptBuilder.AppendLine("לאזן בין עבודה ומשפחה, ולתן ייעוץ מבוסס על הקשר אישי עמוק.");
            promptBuilder.AppendLine();

promptBuilder.AppendLine("=== נתוני רקע מערכתיים ===");
            promptBuilder.AppendLine($"• השעה הנוכחית: {currentTime}");
      promptBuilder.AppendLine($"• התאריך: {currentDate} ({currentDayOfWeek})");

      if (userProfile != null)
      {
    promptBuilder.AppendLine();
          promptBuilder.AppendLine("=== פרופיל המשתמש ===");
    promptBuilder.AppendLine($"• שם: {userProfile.First_name} {userProfile.Last_name}");
          
          if (!string.IsNullOrEmpty(userProfile.FamilyStatus))
         promptBuilder.AppendLine($"• סטטוס משפחתי: {userProfile.FamilyStatus}");
        
       if (!string.IsNullOrEmpty(userProfile.WorkStyle))
            promptBuilder.AppendLine($"• סגנון עבודה: {userProfile.WorkStyle}");
      
          if (!string.IsNullOrEmpty(userProfile.PreferredWorkHours))
promptBuilder.AppendLine($"• שעות עבודה מועדפות: {userProfile.PreferredWorkHours}");
}

         promptBuilder.AppendLine();
 promptBuilder.AppendLine(userInsightsPrompt);
 promptBuilder.AppendLine();

            promptBuilder.AppendLine("=== הנחיות חשיבה וקבלת החלטות ===");
promptBuilder.AppendLine("1. **דרישות למעשה (Must-Do)**: אם משתמש מבקש לבצע פעולה רגישה,");
      promptBuilder.AppendLine("   או שאלה דורשת נתון ספציפי שאינו בהיסטוריה - שאל להבהרה.");
            promptBuilder.AppendLine();
            promptBuilder.AppendLine("2. **הקשר וזיכרון**: אם המשתמש כותב משפט חלקי (למשל: 'תעדכן את זה'),");
   promptBuilder.AppendLine("   השתמש בהיסטוריית השיחה. אם ברור - אל תשאל שוב.");
   promptBuilder.AppendLine();
            promptBuilder.AppendLine("3. **שיקול דעת עם גבולות**: סמוך על ידע כללי אם הוא לא מנוגד");
  promptBuilder.AppendLine("   לפרופיל האישי. למשל, אם זה 20:00 וגיל הילדים 4-5, ");
   promptBuilder.AppendLine("   אל תשלח למישפחה לגינה - זה זמן שינה.");
            promptBuilder.AppendLine();
       promptBuilder.AppendLine("4. **סגנון תקשורת**: היה תמציתי, חם, ומובן. ");
  promptBuilder.AppendLine("   השתמש בתובנות על אישיות המשתמש כדי להתאים את הטון.");

            return promptBuilder.ToString();
   }

//--------------------------שליחת הודעה והחזרת DTO----------------------------
   public static async Task<MessageDTO> SendMessageAndGetReplyAsync(int sessionId, string userText, int userId)
     {
            // --- שלב א': שמירת הודעת המשתמש ---
 string userFileUrl = Upload_to_the_cloud.UploadChatText(sessionId, userText, "user");
            MessageFunction.AddNewMessage(new Message 
         { 
                SessionId = sessionId, 
                Role = SenderRole.User, 
   ContentURL = userFileUrl 
   });

          // --- שלב ב': בניית ההנחיה ל-AI עם נתוני משתמש ---
      string systemPrompt = BuildDynamicSystemPrompt(userId);

   // --- שלב ג': הכנת היסטוריית השיחה ---
      List<Message> history = MessageFunction.GetMessagesBySessionId(sessionId);
   var conversationContext = new StringBuilder();
 conversationContext.AppendLine(systemPrompt);
    conversationContext.AppendLine();
            conversationContext.AppendLine("=== היסטוריית השיחה ===");
            conversationContext.AppendLine();

     foreach (var msg in history)
            {
              string msgText = Upload_to_the_cloud.DownloadChatText(msg.ContentURL);
          conversationContext.AppendLine($"{msg.Role}: {msgText}");
         }

  string fullConversationContext = conversationContext.ToString();

      // --- שלב ד': פנייה למודל ה-AI ---
      string aiResponseText = await CallAiApiAsync(fullConversationContext);

    // --- שלב ה': שמירת תשובת ה-AI ---
     string aiFileUrl = Upload_to_the_cloud.UploadChatText(sessionId, aiResponseText, "assistant");
  
          List<Message> updatedSession = MessageFunction.AddNewMessage(new Message 
            { 
          SessionId = sessionId, 
         Role = SenderRole.Assistant, 
    ContentURL = aiFileUrl 
       });
            
 Message aiMessageDal = updatedSession[updatedSession.Count - 1];

   // --- שלב ו': המרה ל-DTO ---
        MessageDTO responseDto = AppMapper.MessageToDto(aiMessageDal);
            responseDto.TextContent = aiResponseText;

      // --- שלב ז': ניסיון קטיף תובנות חדשות מהתשובה (Future: Function Calling) ---
            // הערה: בגרסה העתידית, פה יתבצע extraction של תובנות ישירות ל-DB

    return responseDto;
      }

        //--------------------------קריאה ל-API של AI (כרגע Mock)----------------------------
        /// <summary>
/// פנייה ל-API של מודל שפה
  /// בעתיד: OpenAI, Google Gemini, Anthropic Claude וכו'
        /// כרגע: סימולציה
      /// </summary>
        private static async Task<string> CallAiApiAsync(string fullConversationContext)
        {
   // דימוי של זמן המתנה לרשת
  await Task.Delay(1000);
            
 // TODO: החלפה בקריאה אמיתית ל-OpenAI/Gemini
            // דוגמה (OpenAI):
      // var client = new OpenAIClient(apiKey);
  // var message = await client.ChatCompletion.CreateChatCompletion(new CreateChatCompletionRequest { ... });
         // return message.Choices[0].Message.Content;

   return "קראתי את הנתונים שלך בהקפדה. אנני מוכן לעזור לך! ??";
        }
    }
}
