using DAL.Models;
using DTO.Models;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Mapper
{
    [Mapper]
    public static partial class AppMapper
    {
        //המרה לDTO
        public static partial usersDTO UserToDto(Users user);
        //public static partial categoriesDTO CategoryToDto(categories category);
        //public static partial tasksDTO TaskToDto(tasks task);
        public static partial favoriet_users_categoriesDTO FavorietUserCategoryToDto(favoriet_users_categories favorietUserCategory);
        public static partial ChatSessionDTO ChatSessionToDto(ChatSession chatSession);
        //public static partial MessageDTO MessageToDto(Message message);
        public static partial taskFileDTO TaskFileToDto(taskFile taskFile);
        public static partial UserInsightDTO UserInsightToDto(UserInsight userInsight);
 
        //המרה לDAL
        public static partial Users DtoToUser(usersDTO userDto);
        public static partial categories DtoToCategory(categoriesDTO categoryDto);
        //public static partial tasks DtoToTask(tasksDTO taskDto);
        public static partial favoriet_users_categories DtoToFavorietUserCategory(favoriet_users_categoriesDTO favorietUserCategoryDto);
        public static partial ChatSession DtoToChatSession(ChatSessionDTO chatSessionDto);
        //public static partial Message DtoToMessage(MessageDTO messageDto);
        public static partial taskFile DtoToTaskFile(taskFileDTO taskFileDto);
        public static partial UserInsight DtoToUserInsight(UserInsightDTO userInsightDto);
        public static tasks DtoToTask(tasksDTO taskDto)
        {
            // בדיקת בטיחות למניעת שגיאות Null
            if (taskDto == null)
            {
                return null;
            }

            // יצירת אובייקט DAL חדש ונקי
            tasks t = new tasks();
            t= new tasks
            {
                Id = taskDto.Id,
                Title = taskDto.Title,
                Task_Date = taskDto.Task_Date,

                // העתקת המפתחות הזרים בלבד (Foreign Keys)
                user_id = taskDto.user_id,       // מקשר למשתמש
                CategoryId = taskDto.CategoryId  // מקשר לקטגוריה

                // אנו לא מאתחלים את:
                // Users
                // Category
                // TaskFiles
                // הם יישארו null, וזה בדיוק מה ש-Entity Framework מצפה לקבל ביצירת/עדכון רשומה!
            };
            return t;
        }

        // המרה ל-DTO עבור הודעה
        // אנו אומרים למאפר: "תתעלם משדה הטקסט, ה-BLL יטפל בו"
        [MapperIgnoreTarget(nameof(MessageDTO.TextContent))]
        public static partial MessageDTO MessageToDto(Message message);

        // המרה ל-DAL עבור הודעה
        // אנו אומרים למאפר: "תתעלם משדה ה-URL, ה-BLL יטפל בו"
        [MapperIgnoreTarget(nameof(Message.ContentURL))]
        public static partial Message DtoToMessage(MessageDTO messageDto);
        //טעינה עצלה - המרה לDTO עם JOIN


        //מחלקת משימות
        // אומרים ל-Mapperly מאיפה לקחת את הנתונים עבור השדות שלא תואמים בשם
        [MapProperty("Users.First_name", "user_first_name")]
        [MapProperty("Users.Last_name", "user_last_name")]
        [MapProperty("Category.Name", "CategoryName")] 
        [MapProperty("Category.Color", "color")]
        public static partial tasksDTO TaskToDto(tasks task);

        // פונקציית ה-Projection (התחליף המקצועי לטעינה עצלה)
       [MapProperty("Users.First_name", "user_first_name")]
        [MapProperty("Users.Last_name", "user_last_name")]
        [MapProperty("Category.Name", "CategoryName")] 
        [MapProperty("Category.Color", "color")]        // פונקציה זו תתרגם את ההמרה ישירות לשאילתת SQL עם JOIN
        public static partial IQueryable<tasksDTO> TaskToDtoList(this IQueryable<tasks> query);


        // הגדרת הניתוב
        [MapProperty("ParentCategory.Name", "father_name")]
        //[MapProperty("ParentCategory.Color", "Color")]


        public static partial categoriesDTO CategoryToDto(categories category);

        // פונקציית ה-Projection
        [MapProperty("ParentCategory?.Name", "father_name")]
        //[MapProperty("ParentCategory.Color", "Color")]
        public static partial IQueryable<categoriesDTO> CategoryToDtoList(this IQueryable<categories> query);
    }

}
