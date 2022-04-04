using AdvancedCFinalProject.Data;
using AdvancedCFinalProject.Models;

namespace AdvancedCFinalProject.Helpers
{
    public class ProjectHelper
    {
        private readonly ApplicationDbContext db;

        public ProjectHelper(ApplicationDbContext _db)
        {
            db = _db;
        }

        public void NewProject(string? title)
        {
            try
            {
                Project project = new Project { Title = title, IsComplete = false};
                db.Project.Add(project);
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }

        public void DeleteProject(int? id)
        {
            try
            {
                Project projectToDelete = db.Project.First(p => p.ProjectId == id);
                db.Project.Remove(projectToDelete);
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }

        public void UpdateProject(int? id, string? title, bool? IsComplete)
        {

            try
            {
                Project ProjectToUpdate = db.Project.First(p => p.ProjectId == id);
                ProjectToUpdate.Title = title;
                ProjectToUpdate.IsComplete = (bool)IsComplete;
            }
            catch (Exception ex) { }

        }
    }
}
