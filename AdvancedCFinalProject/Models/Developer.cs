using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedCFinalProject.Models
{
	public class Developer
	{
		[Key]
		public int DeveloperId { get; set; }
		public string Title { get; set; }
		public int DeveloperTaskId { get; set; }
		public DeveloperTask Task { get; set; } 
		public Developer()
		{
		}
	}
}

