using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Functions;
using DTO.Models;

namespace tasks_project.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
  //-------------
        //שליפה
        //-------------
        [HttpGet("GetAllTasks")]
        public IActionResult GetAllTasks()
        {
            return Ok(tasksBLL.GetAllTasks());
        }

        //-------------------
 // שליפה לפי קוד
 //-------------------
        [HttpGet("GetTaskById/{taskId}")]
        public IActionResult GetTaskById(int taskId)
        {
      return Ok(tasksBLL.GetTaskById(taskId));
        }

        //-------------
        //הוספה
        //-------------
        [HttpPut]
        public IActionResult AddNewTask([FromBody] tasksDTO taskDTO)
        {

            // מחזיר סטטוס 201 (Created) יחד עם המשימה החדשה
            return Ok(tasksBLL.AddNewTask(taskDTO));
        }


        //-------------
        //עדכון
        //-------------
        [HttpPost("UpdateTask/{taskId}")]
        public IActionResult UpdateTask(int taskId, [FromBody] tasksDTO taskDTO)
        {
            return Ok(tasksBLL.UpdateTask(taskId, taskDTO));
 }

        //-------------
//מחיקה
      //-------------
        [HttpDelete("DeleteTask/{taskId}")]
  public IActionResult DeleteTask(int taskId)
     {
     return Ok(tasksBLL.DeleteTask(taskId));
        }
    }
}
