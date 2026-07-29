using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for task-related operations.
    /// </summary>
    public static class tasksFunction
    {
        //--------------------------קבלת כל המשימות----------------------------

        // הפונקציה שמביאה את כל המשימות - כאן נמצא התיקון הקריטי!
        public static List<tasks> GetAllTasks()
        {
            using (AppDbContext DB = new AppDbContext())
            {
                return DB.Tasks
                    .Include(t => t.Users)      // מורה ל-EF לבצע JOIN לטבלת המשתמשים
                    .Include(t => t.Category)  // מורה ל-EF לבצע JOIN לטבלת הקטגוריות
                    .ToList();
            }
        }
        //--------------------------קבלת משימה על פי קוד המשימה----------------------------
        public static tasks? GetTaskById(int id)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                tasks Task = DB.Tasks.FirstOrDefault(p => p.Id == id)!;
                if (Task != null)
                    return Task;
                return null;
            }
        }

        public static tasks AddNewTask(tasks t)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                // 1. הוספה ושמירה (EF ייצר Id חדש למשימה)
                DB.Tasks.Add(t);
                DB.SaveChanges();

                // 2. שליפה מחדש של המשימה עם כל קשרי הגומלין (JOIN)
                tasks savedTask = DB.Tasks
                    .Include(task => task.Users)       // טוען את נתוני המשתמש
                    .Include(task => task.Category)    // טוען את נתוני הקטגוריה
                                                       // .ThenInclude(c => c.ParentCategory) // (הערה: אם צריך גם את קטגוריית האב, משתמשים ב-ThenInclude)
                    .FirstOrDefault(task => task.Id == t.Id); // מסנן לפי ה-ID החדש

                // 3. החזרת אובייקט DAL מלא לשכבה שקראה לפונקציה
                return savedTask;

            }
        }


        //----------------------------------עדכון משימה----------------------------------
        public static List<tasks> UpdateTask(int idTask, tasks newTask)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                tasks TaskToUpdate = DB.Tasks.FirstOrDefault(p => p.Id == idTask)!;
                if (TaskToUpdate != null)
                {
                    TaskToUpdate.Title = newTask.Title;
                    TaskToUpdate.Task_Date = newTask.Task_Date;
                    TaskToUpdate.user_id = newTask.user_id;
                    TaskToUpdate.CategoryId = newTask.CategoryId;
                    DB.SaveChanges();
                }
                return GetAllTasks();
            }
        }

        //--------------------------------מחיקת משימה----------------------------------
        public static List<tasks> DeleteTask(int idTask)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                tasks TaskToDelete = DB.Tasks.FirstOrDefault(p => p.Id == idTask)!;
                if (TaskToDelete != null)
                {
                    DB.Tasks.Remove(TaskToDelete);
                    DB.SaveChanges();
                }
                return GetAllTasks();
            }
        }
    }
}
