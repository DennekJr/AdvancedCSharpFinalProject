using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AdvancedCFinalProject.Models
{
	public class Developer
	{
		[Key]
		public int DeveloperId { get; set; }
		public string Title { get; set; }
		public ICollection<Comment> Comments { get; set; }
		public ICollection<DeveloperTask> Tasks { get; set; }

		public ICollection<Notification> Notifications { get; set; }
		public Developer()
		{
			Tasks = new HashSet<DeveloperTask>();
			Comments = new HashSet<Comment>();
			Notifications = new HashSet<Notification>();
		}
	}
}

