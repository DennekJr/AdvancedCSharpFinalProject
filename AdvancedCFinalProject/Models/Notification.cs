namespace AdvancedCFinalProject.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsOpned { get; set; }
        public int? projectId { get; set; }

        public Project? Project { get; set; }
        public int? TaskId { get; set; }
        public DeveloperTask? DeveloperTask { get; set; }

        public Developer? Developer { get; set; }
        public int? DeveloperId { get; set; }

    }
}
