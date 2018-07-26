namespace JakeBladt.AssignmentAPI.Domain
{
    public class AssignmentTag
    {
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
