using JakeBladt.AssignmentAPI.Domain;

namespace JakeBladt.AssignmentAPI.ViewModels
{
    public class AssignmentViewmodel : Assignment
    {
        public string Self { get; set; }
        public string[] Tags { get; set; }

        private static string SelfLink(int id)
        {
            return $"/api/v0.1/assignments/{id}";
        }

        public static AssignmentViewmodel FromAssignment(Assignment assignment)
        {
            return new AssignmentViewmodel
            {
                AssignmentId = assignment.AssignmentId,
                Name = assignment.Name,
                Description = assignment.Description,
                Title = assignment.Title,
                AssignmentType = assignment.AssignmentType,
                Duration = assignment.Duration,
                Self = SelfLink(assignment.AssignmentId)
            };
        }
    }
}
