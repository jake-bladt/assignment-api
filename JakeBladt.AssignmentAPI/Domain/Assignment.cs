using System.Collections.Generic;

namespace JakeBladt.AssignmentAPI.Domain
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignmentType { get; set; }
        public string Duration { get; set; }
        public List<AssignmentTag> AssignmentTags { get; set; } = new List<AssignmentTag>();
    }
}
