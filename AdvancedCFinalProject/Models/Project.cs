using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedCFinalProject.Models
{
	public class Project
	{
		[Key]
		public int ProjectId { get; set; }

		public string Title { get; set; }

		[StringLength(150, MinimumLength = 3)]
		public string Content { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd")]
		public DateTime CreatedTime { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd")]
		public DateTime Deadline { get; set; }

		public int CompanyId { get; set; }

		public bool IsComplete { get; set; }

		public Company? Company { get; set; }

		public Priority? Priority { get; set; }

		public ICollection<DeveloperTask> Tasks { get; set; }
		public ICollection<Notification> Notifications { get; set; }

		public Project()
		{
			Deadline = System.DateTime.Now;
			CreatedTime = System.DateTime.Now;
			Tasks = new HashSet<DeveloperTask>();
			Notifications = new HashSet<Notification>();
		}


	}
	public enum Priority
	{
		None = 0,
		Low = 1,
		Medium = 2,
		High = 3
	}
}

