using System;
namespace AdvancedCFinalProject.Models
{
	public class Urgent : Comment
	{

		public bool IsURgent { get; set; }
		public string UrgentNote { get; set; }
		public Urgent()
		{
		}
	}
}

