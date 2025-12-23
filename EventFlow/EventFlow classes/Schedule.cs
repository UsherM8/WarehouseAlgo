using EventFlowClassLibrary.enums;

namespace EventFlowClassLibrary
{
    public class Schedule
    {
        // Properties for each activity type
        public List<Activity> Readings { get; } = new List<Activity>();
        public List<Activity> Exhibitions { get; } = new List<Activity>();
        public List<Activity> FoodCourts { get; } = new List<Activity>();
        public List<Activity> ProductPresentations { get; } = new List<Activity>();

        public void AddActivity(Activity activity)
        {
            if (activity.Type == ActivityType.Reading && !Readings.Contains(activity))
            {
                Readings.Add(activity);
            }
            else if (activity.Type == ActivityType.Exhibition && !Exhibitions.Contains(activity))
            {
                Exhibitions.Add(activity);
            }
            else if (activity.Type == ActivityType.FoodCourt && !FoodCourts.Contains(activity))
            {
                FoodCourts.Add(activity);
            }
            else if (activity.Type == ActivityType.ProductPresentation && !ProductPresentations.Contains(activity))
            {
                ProductPresentations.Add(activity);
            }
        }

        public List<Activity> GetAllActivities()
        {
            // Combine all activity lists into a single list
            return Readings
                .Concat(Exhibitions)
                .Concat(FoodCourts)
                .Concat(ProductPresentations)
                .ToList();
        }
    }
}
