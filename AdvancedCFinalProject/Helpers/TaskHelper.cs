using System;
using AdvancedCFinalProject.Data;
using AdvancedCFinalProject.Models;

namespace AdvancedCFinalProject.Helpers
{
	public class TaskHelper
	{
		private readonly ApplicationDbContext db;

		public TaskHelper(ApplicationDbContext _db)
		{
			db = _db;
		}

        public void NewTask(string? title)
        {
            try
            {
                DeveloperTask task = new DeveloperTask { Title = title };
                db.Tasks.Add(task);
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }

        public void DeleteTask(int? id)
        {
            try
            {
                DeveloperTask TaskToDelete = db.Tasks.First(p => p.TaskId == id);
                db.Tasks.Remove(TaskToDelete);
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }

        public void UpdateTask(int? id, string? title, Priority? priority, bool? isComplete)
        {
           
            try
            {
                DeveloperTask TaskToUpdate = db.Tasks.First(p => p.TaskId == id);
                TaskToUpdate.Title = title;
                TaskToUpdate.Priority = (Priority)priority;
                TaskToUpdate.IsComplete = (bool)isComplete;
            }
            catch (Exception ex) { }

        }
        public void AssignTask(int? id, int? devId)
        {
            DeveloperTask TaskToAsssign = db.Tasks.First(p => p.TaskId == id);
            Developer developer = db.Developer.First(p => p.DeveloperId == devId);
            developer.Tasks.Add(TaskToAsssign);
            TaskToAsssign.Developer = developer;

        }
        public TaskHelper()
		{
		}
	}
}

