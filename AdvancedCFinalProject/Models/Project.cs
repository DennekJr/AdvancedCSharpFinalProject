using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedCFinalProject.Models
{
	public class Project
	{
		[Key]
		public int ProjectId { get; set; }
		public string Title { get; set; }
		public bool IsComplete { get; set; }
		public ICollection<DeveloperTask> Tasks { get; set; }

		public Project()
		{
			Tasks = new HashSet<DeveloperTask>();
		}
	}
}

