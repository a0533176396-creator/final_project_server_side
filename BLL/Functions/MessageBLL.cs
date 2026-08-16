using DAL;
using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.AIPlatform.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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

        //--------------------------בניית SystemInstruction דינמי מופרד----------------------------
        /// <summary>
        /// בנאי SystemInstruction דינמי שמשלב זמן אמת, נתוני משתמש, והנחיות מותנות
        /// </summary>
        private static string BuildDynamicSystemPrompt(int userId)
        {
            // 1. שליפת נתוני זמן אמת
            string currentTime = DateTime.Now.ToString("HH:mm");
            string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
            string currentDayOfWeek = DateTime.Now.DayOfWeek.ToString();

            // 2. שליפת פרופיל המשתמש מ-DAL (לפי ID מספרי)
            var userProfile = usersFunction.GetUserById(userId);

            // 3. שליפת התובנות שה-AI למד על המשתמש
            var userInsightsPrompt = UserInsightFunction.BuildUserProfilePrompt(userId);

            // 4. בניית ה-System Prompt הדינמי
            var promptBuilder = new StringBuilder();

            promptBuilder.AppendLine("אתה עוזר וירטואלי חכם, מקצועי ואדיב מאוד.");
            promptBuilder.AppendLine("תפקידך: לעזור למשתמש לנהל את המשימות שלו באופן הנכון לו, ");
            promptBuilder.AppendLine("לאזן בין עבודה ומשפחה, ולתת ייעוץ מבוסס על הקשר אישי עמוק.");
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
            promptBuilder.AppendLine("1. **דרישות למעשה (Must-Do)**: אם משתמש מבקש לבצע פעולה רגישה, או שאלה דורשת נתון ספציפי שאינו בהיסטוריה - שאל להבהרה.");
            promptBuilder.AppendLine("2. **הקשר וזיכרון**: אם המשתמש כותב משפט חלקי (למשל: 'תעדכן את זה'), השתמש בהיסטוריית השיחה. אם ברור - אל תשאל שוב.");
            promptBuilder.AppendLine("3. **שיקול דעת עם גבולות**: סמוך עלKnowledge if it is not contradictory to the personal profile. For example, if it is 20:00 and the children are 4-5 years old, do not send the family to the garden - it's bedtime.");
            promptBuilder.AppendLine("4. **סגנון תקשורת**: היה תמציתי, חם, ומובן. השתמש בתובנות על אישיות המשתמש כדי להתאים את הטון.");

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

            // --- שלב ב': בניית הנחיית המערכת הדינמית (SystemInstruction) ---
            string systemPrompt = BuildDynamicSystemPrompt(userId);

            // --- שלב ג': הכנת היסטוריית השיחה כמבנה מובנה של הודעות ---
            List<Message> history = MessageFunction.GetMessagesBySessionId(sessionId);
            var conversationContents = new List<Content>();

            foreach (var msg in history)
            {
                string msgText = Upload_to_the_cloud.DownloadChatText(msg.ContentURL);
                string role = msg.Role == SenderRole.User ? "user" : "model";

                conversationContents.Add(new Content
                {
                    Role = role,
                    Parts = { new Part { Text = msgText } }
                });
            }

            // --- שלב ד': פנייה אמתית למודל ה-AI ב-Google Cloud Vertex AI ---
            string aiResponseText = await CallAiApiAsync(systemPrompt, conversationContents);

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

            return responseDto;
        }

        //--------------------------קריאה ל-REST API של Vertex AI (תומך נטפרי - HTTP/1.1)----------------------------
        private static async Task<string> CallAiApiAsync(string systemPrompt, List<Content> conversationContents)
        {
            try
            {
                // 1. טעינת הרשאות והפקת Token עוקף gRPC
                string credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "Secrets", "google-gcs-key.json");
                GoogleCredential credential = GoogleCredential.FromFile(credentialPath)
                    .CreateScoped("https://www.googleapis.com/auth/cloud-platform");

                string accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                // 2. בניית גוף הבקשה בפורמט JSON
                var requestBody = new
                {
                    systemInstruction = new
                    {
                        parts = new[] { new { text = systemPrompt } }
                    },
                    contents = conversationContents.Select(c => new
                    {
                        role = c.Role,
                        parts = c.Parts.Select(p => new { text = p.Text }).ToArray()
                    }).ToArray()
                };

                string jsonPayload = JsonSerializer.Serialize(requestBody);

                // 3. שליחה ב-HTTP/1.1 (תואם נטפרי)
                string projectId = "project-030df7cb-ddf5-49b9-85d";
                string url = $"https://us-central1-aiplatform.googleapis.com/v1/projects/{projectId}/locations/us-central1/publishers/google/models/gemini-1.5-flash:generateContent";

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, content);

                    string responseJson = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        return $"שגיאה מהשרת ({response.StatusCode}): {responseJson}";
                    }

                    // 4. חילוץ התשובה מתוך ה-JSON
                    using (JsonDocument doc = JsonDocument.Parse(responseJson))
                    {
                        var root = doc.RootElement;
                        if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                        {
                            var firstCandidate = candidates[0];
                            if (firstCandidate.TryGetProperty("content", out var contentElem) &&
                                contentElem.TryGetProperty("parts", out var parts) &&
                                parts.GetArrayLength() > 0)
                            {
                                return parts[0].GetProperty("text").GetString();
                            }
                        }
                    }

                    return "לא התקבלה תשובה משרת ה-AI.";
                }
            }
            catch (Exception ex)
            {
                return $"שגיאה בתקשורת עם סוכן ה-AI: {ex.Message}";
            }
        }
    }
}
