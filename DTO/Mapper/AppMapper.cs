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
        public static partial usersDTO UserToDto(users user);
        //public static partial categoriesDTO CategoryToDto(categories category);
        //public static partial tasksDTO TaskToDto(tasks task);
        public static partial favoriet_users_categoriesDTO FavorietUserCategoryToDto(favoriet_users_categories favorietUserCategory);
        public static partial ChatSessionDTO ChatSessionToDto(ChatSession chatSession);
        public static partial MessageDTO MessageToDto(Message message);
        public static partial taskFileDTO TaskFileToDto(taskFile taskFile);
        
        //המרה לDAL
        public static partial users DtoToUser(usersDTO userDto);
        public static partial categories DtoToCategory(categoriesDTO categoryDto);
        public static partial tasks DtoToTask(tasksDTO taskDto);
        public static partial favoriet_users_categories DtoToFavorietUserCategory(favoriet_users_categoriesDTO favorietUserCategoryDto);
        public static partial ChatSession DtoToChatSession(ChatSessionDTO chatSessionDto);
        public static partial Message DtoToMessage(MessageDTO messageDto);
        public static partial taskFile DtoToTaskFile(taskFileDTO taskFileDto);

        //טעינה עצלה - המרה לDTO עם JOIN


        //מחלקת משימות
        // אומרים ל-Mapperly מאיפה לקחת את הנתונים עבור השדות שלא תואמים בשם
        [MapProperty("User.First_name", "user_first_name")]
        [MapProperty("User.Last_name", "user_last_name")]
        [MapProperty("Category.Name", "CategoryName")] // הנחתי שלקטגוריה יש שדה name
        [MapProperty("Category.Color", "color")]
        public static partial tasksDTO TaskToDto(tasks task);

        // פונקציית ה-Projection (התחליף המקצועי לטעינה עצלה)
        // פונקציה זו תתרגם את ההמרה ישירות לשאילתת SQL עם JOIN
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
