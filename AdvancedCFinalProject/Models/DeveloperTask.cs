using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedCFinalProject.Models
{
	public class DeveloperTask
	{
		[Key]
		public int TaskId { get; set; }
		public string Title { get; set; }
		public DeveloperTask()
		{
		}
	}
}

