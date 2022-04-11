using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedCFinalProject.Models
{
	public class Comment
	{
		[Key]
		public int CommentId {get; set;}
		public string Description { get; set; }
		public Project? Project { get; set; }
		public int? ProjectId { get; set; }
		public DeveloperTask? Task { get; set; }
		public int? TaskId { get; set; }
		public int? DeveloperId { get; set; }
		public Developer? Developer { get; set; }
		public int? DeveloperTaskId { get; set; }
		public Comment()
		{

		}
	}
}

