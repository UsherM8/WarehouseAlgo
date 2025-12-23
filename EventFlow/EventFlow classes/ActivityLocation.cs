using EventFlowClassLibrary.enums;

namespace EventFlowClassLibrary
{
    public class ActivityLocation
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public ActivityType Type { get; set; }
        public List<Activity> Activities { get; set; } = new List<Activity>();

        public List<Activity> GenerateSlots(ActivityLocation location, int slotCount, int slotDuration, ActivityType type, DateTime startTime, DateTime endTime)
        {
            List<Activity> activities = new List<Activity>();
            TimeSpan slotDurationTimeSpan = TimeSpan.FromMinutes(slotDuration);

            // Adjust start time to 11:00 AM for FoodCourt
            DateTime currentStartTime = type == ActivityType.FoodCourt ? startTime.Date.AddHours(11) : startTime;

            int i = 0;
            while (i < slotCount && currentStartTime + slotDurationTimeSpan <= endTime)
            {
                DateTime currentEndTime = currentStartTime + slotDurationTimeSpan;

                activities.Add(new Activity
                {
                    Name = $"Activity {i + 1}",
                    StartTime = currentStartTime,
                    EndTime = currentEndTime,
                    Capacity = location.Capacity,
                    Type = type
                });

                // Update to the next slot
                currentStartTime = currentEndTime;
                i++;
            }

            location.Activities = activities;
            return activities;
        }

        public List<Activity> GenerateSlots(ActivityLocation location, string reading1, string reading2, int slotCount, int slotDuration, DateTime startTime, DateTime endTime)
        {
            List<Activity> activities = new List<Activity>();
            TimeSpan slotDurationTimeSpan = TimeSpan.FromMinutes(slotDuration);
            DateTime currentStartTime = startTime;

            bool isReading1 = true; // Toggle between Reading 1 and Reading 2
            int currentSlot = 0;    // Track the number of slots created

            while (currentSlot < slotCount && currentStartTime + slotDurationTimeSpan <= endTime)
            {
                DateTime currentEndTime = currentStartTime + slotDurationTimeSpan;

                activities.Add(new Activity
                {
                    Name = isReading1 ? reading1 : reading2,
                    StartTime = currentStartTime,
                    EndTime = currentEndTime,
                    Capacity = location.Capacity,
                    Type = ActivityType.Reading
                });

                // Toggle to the next reading
                isReading1 = !isReading1;

                // Move to the next time slot
                currentStartTime = currentEndTime;
                currentSlot++;
            }

            location.Activities = activities;
            return activities;
        }



    }
}
