using EventFlowClassLibrary.enums;

namespace EventFlowClassLibrary
{
    public class Activity
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        public ActivityType Type { get; set; }
        public List<Visitor> Visitors { get; set; } = new List<Visitor>();
    }
}
