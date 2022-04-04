using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedCFinalProject.Models
{
	public class Comment
	{
		[Key]
		public int CommentId {get; set;}
		public string Description { get; set; }
		public int DeveloperId { get; set; }
		public Developer Developer { get; set; }
		public int DeveloperTaskId { get; set; }
		public DeveloperTask DeveloperTask { get; set; }
		public Comment()
		{

		}
	}
}

