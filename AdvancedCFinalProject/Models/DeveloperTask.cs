using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedCFinalProject.Models
{
	public class DeveloperTask
	{
		[Key]
		public int TaskId { get; set; }
		public string Title { get; set; }
		public int? CompletionRate { get; set; }
		public bool IsComplete { get; set; } = false;
		public Priority? Priority { get; set; }
		//public Comment? Comment { get; set; }
		public DateTime Deadline { get; set; }
		public int? DeveloperId { get; set; }
		public Developer? Developer { get; set; }
		public int? projectId { get; set; }
		public Project? Project { get; set; }
		public ICollection<Notification> Notification { get; set; }
		public DeveloperTask()
		{
			Notification = new HashSet<Notification>();
		}
	}
}

