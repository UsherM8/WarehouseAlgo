namespace EventFlowClassLibrary
{
    public class Visitor
    {
        public string Name { get; set; }
        public List<Activity> ReadingPreferences { get; set; } = new List<Activity>();
        public List<Activity> ProductPresentationPreferences { get; set; } = new List<Activity>();
        public Schedule Schedule { get; set; } = new Schedule();
    }

}
