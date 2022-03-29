using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedCFinalProject.Models
{
	public enum Priority
	{
		None = 0,
		Low = 1,
		Medium = 2,
		High = 3
	}
	public class Project
	{        
        public int Id { get; set; }
        public string Name { get; set; }
        [StringLength(150, MinimumLength = 3)]
        public string Content { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd")]
        public DateTime CreatedTime { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd")]
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public double Budget { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd")]
        public DateTime FinishedTime { get; set; }
        public double TotalCost { get; set; }

        public Project()
		{
            Deadline = System.DateTime.Now;
            CreatedTime = System.DateTime.Now;
        }
	}
}

