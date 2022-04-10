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
		public Priority Priority { get; set; }
		public string? stringComment { get; set; }
		public string? UrgentComment { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd")]
		public DateTime CreatedTime { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd")]
		public DateTime Deadline { get; set; }
		public Comment? Comment { get; set; }
		public Urgent? UrgentNote { get; set; }
		public int? DeveloperId { get; set; }
		public Developer? Developer { get; set; }
		public int? ProjectId { get; set; }
		public Project? Project { get; set; }
		public ICollection<Notification> Notification { get; set; }
		public DeveloperTask()
		{
			Deadline = System.DateTime.Now;
			CreatedTime = System.DateTime.Now;
			Notification = new HashSet<Notification>();
		}
	}
}

