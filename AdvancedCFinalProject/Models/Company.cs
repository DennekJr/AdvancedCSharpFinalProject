using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedCFinalProject.Models
{
	public class Company
	{
		[Key]
		public int CompanyId { get; set; }
		public string Title { get; set; }
		public ICollection<Project> Projects { get; set; }
		public Company()
		{
			Projects = new HashSet<Project>();
		}
	}
}

