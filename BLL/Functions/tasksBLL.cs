using AutoMapper;
using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace BLL.Functions
{
    /// <summary>
    /// Business Logic Layer function class for task-related operations.
    /// </summary>
    public static class tasksBLL
    {
        //-----------------------------------GetAllTasks-----------------------------------
        public static List<tasksDTO> GetAllTasks()
        {
            List<tasks> allData = tasksFunction.GetAllTasks();
            return allData.Select(AppMapper.TaskToDto).ToList();
        }

        //-----------------------------------GetTaskById-----------------------------------
        public static tasksDTO? GetTaskById(int id)
        {
            tasks? task = tasksFunction.GetTaskById(id);
            if (task == null)
                return null;
            return AppMapper.TaskToDto(task);
        }

        //-----------------------------------AddNewTask-----------------------------------
        public static tasksDTO AddNewTask(tasksDTO newTaskDto)
        {
            // המרה מ-DTO לישות
            tasks newTask = AppMapper.DtoToTask(newTaskDto);

            // קריאה ל-DAL
            tasks savedTask = tasksFunction.AddNewTask(newTask);

            // החזרת ה-DTO של המשימה החדשה
            return AppMapper.TaskToDto(savedTask);
        }


        //-----------------------------------UpdateTask-----------------------------------
        public static List<tasksDTO> UpdateTask(int idTask, tasksDTO newTask)
        {
            tasks newTaskTBL = AppMapper.DtoToTask(newTask);
            List<tasks> allData = tasksFunction.UpdateTask(idTask, newTaskTBL);
            return allData.Select(AppMapper.TaskToDto).ToList();
        }

        //-----------------------------------DeleteTask-----------------------------------
        public static List<tasksDTO> DeleteTask(int idTask)
        {
            List<tasks> allData = tasksFunction.DeleteTask(idTask);
            return allData.Select(AppMapper.TaskToDto).ToList();
        }

        //-----------------------------------GetPreviousWeekDates-----------------------------------
        /// <summary>
        /// Returns an array of DateTime objects representing all days of the previous week.
        /// Week starts on Sunday and ends on Saturday.
        /// </summary>
        /// <returns>Array of 7 DateTime objects for the previous week</returns>
        public static DateTime[] GetPreviousWeekDates()
        {
            DateTime today = DateTime.Now;
            int daysToSunday = (int)today.DayOfWeek;
            DateTime currentWeekStart = today.AddDays(-daysToSunday);
            DateTime previousWeekStart = currentWeekStart.AddDays(-7);

            return Enumerable.Range(0, 7)
                    .Select(i => previousWeekStart.AddDays(i))
                    .ToArray();
        }

        //-----------------------------------GetCurrentWeekDates-----------------------------------
        /// <summary>
        /// Returns an array of DateTime objects representing all days of the current week.
        /// Week starts on Sunday and ends on Saturday.
        /// </summary>
        /// <returns>Array of 7 DateTime objects for the current week</returns>
        public static DateTime[] GetCurrentWeekDates()
        {
            DateTime today = DateTime.Now;
            int daysToSunday = (int)today.DayOfWeek;
            DateTime currentWeekStart = today.AddDays(-daysToSunday);

            return Enumerable.Range(0, 7)
                    .Select(i => currentWeekStart.AddDays(i))
                    .ToArray();
        }

        //-----------------------------------GetNextWeekDates-----------------------------------
        /// <summary>
        /// Returns an array of DateTime objects representing all days of the next week.
        /// Week starts on Sunday and ends on Saturday.
        /// </summary>
        /// <returns>Array of 7 DateTime objects for the next week</returns>
        public static DateTime[] GetNextWeekDates()
        {
            DateTime today = DateTime.Now;
            int daysToSunday = (int)today.DayOfWeek;
            DateTime currentWeekStart = today.AddDays(-daysToSunday);
            DateTime nextWeekStart = currentWeekStart.AddDays(7);

            return Enumerable.Range(0, 7)
                 .Select(i => nextWeekStart.AddDays(i))
                 .ToArray();
        }

        //-----------------------------------GetTasksByWeekAndUser-----------------------------------
        /// <summary>
        /// Retrieves tasks for a specific week and user, organized in a 7-element list by day of week.
        /// Each list index represents a day: 0=Sunday, 1=Monday, 2=Tuesday, 3=Wednesday, 4=Thursday, 5=Friday, 6=Saturday
        /// </summary>
        /// <param name="weekType">Week type: 0 = Previous Week, 1 = Current Week, 2 = Next Week</param>
        /// <param name="userId">The user ID whose tasks to retrieve</param>
        /// <returns>List of 7 tasksDTO lists, one for each day of the week. Days with no tasks contain empty lists.</returns>
        public static List<List<tasksDTO>> GetTasksByWeekAndUser(int weekType, int userId)
        {
            // Get the appropriate week's dates
            DateTime[] weekDates = weekType switch
            {
                0 => GetPreviousWeekDates(),
                1 => GetCurrentWeekDates(),
                2 => GetNextWeekDates(),
                _ => GetCurrentWeekDates() // Default to current week
            };

            // Initialize 7-element list to hold task lists for each day
            List<List<tasksDTO>> weekTasks = new List<List<tasksDTO>>();
            for (int i = 0; i < 7; i++)
            {
                weekTasks.Add(new List<tasksDTO>());
            }

            // Get all tasks for the user
            List<tasksDTO> userTasks = GetAllTasks()
                .Where(t => t.user_id == userId)
                .ToList();

            // Organize tasks by day of week
            for (int dayIndex = 0; dayIndex < 7; dayIndex++)
            {
                DateTime dayStart = weekDates[dayIndex].Date;
                DateTime dayEnd = dayStart.AddDays(1);

                // Find tasks that fall on this specific day
                var tasksForDay = userTasks
                    .Where(t => t.Task_Date >= dayStart && t.Task_Date < dayEnd)
                    .ToList();

                weekTasks[dayIndex] = tasksForDay;
            }

            return weekTasks;
        }
    }
}
