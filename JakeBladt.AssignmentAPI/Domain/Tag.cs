using System.Collections.Generic;

namespace JakeBladt.AssignmentAPI.Domain
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public List<AssignmentTag> AssignmentTags { get; set; } = new List<AssignmentTag>();
    }
}
