using DAL.Data;
using DAL.Models;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for task-related operations.
    /// </summary>
    public static class tasksFunction
    {
        static AppDbContext DB = new AppDbContext();

        //--------------------------קבלת כל המשימות----------------------------
        public static List<tasks> GetAllTasks()
        {
            return DB.Tasks.ToList();
        }

        //--------------------------קבלת משימה על פי קוד המשימה----------------------------
        public static tasks? GetTaskById(int id)
        {
            tasks Task = DB.Tasks.FirstOrDefault(p => p.Id == id)!;
            if (Task != null)
                return Task;
            return null;
        }

        //--------------------------------הוספת משימה----------------------------------
        public static List<tasks> AddNewTask(tasks t)
        {
            DB.Tasks.Add(t);
            DB.SaveChanges();
            return GetAllTasks();
        }

        //----------------------------------עדכון משימה----------------------------------
        public static List<tasks> UpdateTask(int idTask, tasks newTask)
        {
            tasks TaskToUpdate = DB.Tasks.FirstOrDefault(p => p.Id == idTask)!;
            if (TaskToUpdate != null)
            {
                TaskToUpdate.Title = newTask.Title;
                TaskToUpdate.Task_Date = newTask.Task_Date;
                TaskToUpdate.user_id = newTask.user_id;
                TaskToUpdate.File_path = newTask.File_path;
                TaskToUpdate.CategoryId = newTask.CategoryId;
                DB.SaveChanges();
            }
            return GetAllTasks();
        }

        //--------------------------------מחיקת משימה----------------------------------
        public static List<tasks> DeleteTask(int idTask)
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
