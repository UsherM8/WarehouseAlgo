using EventFlowClassLibrary.enums;

namespace EventFlowClassLibrary
{
    public class EventManager
    {
        public List<ActivityLocation> activityLocations { get; set; } = new List<ActivityLocation>();
        private DateTime EventStartTime { get; set; }
        private DateTime EventEndTime { get; set; }

        #region Location Initialization
        public EventManager()
        {
            EventStartTime = new DateTime(2024, 12, 20, 9, 0, 0);
            EventEndTime = new DateTime(2024, 12, 20, 17, 0, 0);

            InitializeLocations();
            GenerateActivitySlots();
        }

        private void InitializeLocations()
        {
            // Initialize all activity locations
            List<ActivityLocation> locations =
            [
                // Food Courts
                new ActivityLocation { Name = "FoodCourt 1", Type = ActivityType.FoodCourt, Capacity = 500 },
                new ActivityLocation { Name = "FoodCourt 2", Type = ActivityType.FoodCourt, Capacity = 500 },

                // Exhibition Halls
                new ActivityLocation { Name = "ExhibitionHall 1", Type = ActivityType.Exhibition, Capacity = 500 },
                new ActivityLocation { Name = "ExhibitionHall 2", Type = ActivityType.Exhibition, Capacity = 500 },
                new ActivityLocation { Name = "ExhibitionHall 3", Type = ActivityType.Exhibition, Capacity = 500 },

                // Product Presentations
                new ActivityLocation { Name = "ProductPresentation 1", Type = ActivityType.ProductPresentation, Capacity = 100 },
                new ActivityLocation { Name = "ProductPresentation 2", Type = ActivityType.ProductPresentation, Capacity = 100 },
                new ActivityLocation { Name = "ProductPresentation 3", Type = ActivityType.ProductPresentation, Capacity = 100 },
                new ActivityLocation { Name = "ProductPresentation 4", Type = ActivityType.ProductPresentation, Capacity = 100 },
                new ActivityLocation { Name = "ProductPresentation 5", Type = ActivityType.ProductPresentation, Capacity = 100 },

                // Readings
                new ActivityLocation { Name = "Reading 1", Type = ActivityType.Reading, Capacity = 50 },
                new ActivityLocation { Name = "Reading 2", Type = ActivityType.Reading, Capacity = 50 },
                new ActivityLocation { Name = "Reading 3", Type = ActivityType.Reading, Capacity = 50 },
                new ActivityLocation { Name = "Reading 4", Type = ActivityType.Reading, Capacity = 50 },
                new ActivityLocation { Name = "Reading 5", Type = ActivityType.Reading, Capacity = 50 },
                new ActivityLocation { Name = "Reading 6", Type = ActivityType.Reading, Capacity = 50 },
                new ActivityLocation { Name = "Reading 7", Type = ActivityType.Reading, Capacity = 50 },
                new ActivityLocation { Name = "Reading 8", Type = ActivityType.Reading, Capacity = 50 },
                new ActivityLocation { Name = "Reading 9", Type = ActivityType.Reading, Capacity = 50},
                new ActivityLocation { Name = "Reading 10", Type = ActivityType.Reading, Capacity = 50 }
            ];
            
            activityLocations.AddRange(locations);
        }

        private void GenerateActivitySlots()
        {
            List<string> readings = new List<string>
         {
        "Reading 1", "Reading 2", "Reading 3", "Reading 4", "Reading 5",
        "Reading 6", "Reading 7", "Reading 8", "Reading 9", "Reading 10",
        "Reading 11", "Reading 12", "Reading 13", "Reading 14", "Reading 15",
        "Reading 16", "Reading 17", "Reading 18", "Reading 19", "Reading 20"
         };

            foreach (ActivityLocation location in activityLocations)
            {
                if (location.Type == ActivityType.FoodCourt)
                {
                    location.GenerateSlots(location, 6, 30, ActivityType.FoodCourt, EventStartTime, EventEndTime);
                }
                else if (location.Type == ActivityType.Exhibition)
                {
                    location.GenerateSlots(location, 48, 10, ActivityType.Exhibition, EventStartTime, EventEndTime);
                }
                else if (location.Type == ActivityType.ProductPresentation)
                {
                    location.GenerateSlots(location, 12, 40, ActivityType.ProductPresentation, EventStartTime, EventEndTime);
                }
                else if (location.Type == ActivityType.Reading)
                {
                    if (readings.Count >= 2)
                    {
                        var reading1 = readings[0];
                        var reading2 = readings[1];

                        // Remove the two readings from the list after assignment
                        readings.RemoveAt(0);
                        readings.RemoveAt(0);

                        location.GenerateSlots(location, reading1, reading2, 8, 60, EventStartTime, EventEndTime);
                    }
                    else
                    {
                        throw new InvalidOperationException("Not enough readings left to assign to a location.");
                    }
                }
            }
        }
        #endregion

        public List<Visitor> FillInActivities(List<Visitor> visitors)
        {
            List<Visitor> rerun = new List<Visitor>();
            int rerunCount = 0;
            int maxRerunAttempts = 10; // Maximum number of rerun attempts to avoid infinite loops
            List<Visitor> plannedInVisitor = new List<Visitor>();

            // Initial pass: Assign activities to all visitors
            FillFoodCourts(visitors);

            int visitorIndex = 0;
            while (visitorIndex < visitors.Count)
            {
                Visitor visitor = visitors[visitorIndex];
                AssignVisitorsToCategory(visitor, ActivityType.ProductPresentation, 2, 2);
                AssignVisitorsToCategory(visitor, ActivityType.Exhibition, 9, 9);
                AssignVisitorsToCategory(visitor, ActivityType.Reading, 1, 1);

                // Add visitors with no assigned activities to rerun list
                if (visitor.Schedule.GetAllActivities().Count == 0 && !rerun.Contains(visitor))
                {
                    rerun.Add(visitor);
                }

                visitorIndex++;
            }

            // Rerun logic: Retry visitors in the rerun list
            while (rerun.Count > 0 && rerunCount < maxRerunAttempts)
            {
                List<Visitor> remainingVisitors = new List<Visitor>();
                int rerunIndex = 0;

                while (rerunIndex < rerun.Count)
                {
                    var visitor = rerun[rerunIndex];

                    // Retry assigning activities to the visitor
                    AssignVisitorsToCategory(visitor, ActivityType.ProductPresentation, 2, 2);
                    AssignVisitorsToCategory(visitor, ActivityType.Exhibition, 9, 9);
                    AssignVisitorsToCategory(visitor, ActivityType.Reading, 1, 1);

                    // If the visitor still has no activities, keep them in the rerun list
                    if (visitor.Schedule.GetAllActivities().Count == 0)
                    {
                        remainingVisitors.Add(visitor);
                    }

                    rerunIndex++;
                }

                // Update rerun list and increment rerun count
                rerun = remainingVisitors;
                rerunCount++;
            }

            // Iterate through all activity locations
            int locationIndex = 0;
            while (locationIndex < activityLocations.Count)
            {
                var location = activityLocations[locationIndex];
                int activityIndex = 0;

                while (activityIndex < location.Activities.Count)
                {
                    var activity = location.Activities[activityIndex];
                    if (activity.Type == ActivityType.FoodCourt)
                    {
                        int visitorInActivityIndex = 0;

                        while (visitorInActivityIndex < activity.Visitors.Count)
                        {
                            var visitor = activity.Visitors[visitorInActivityIndex];
                            plannedInVisitor.Add(visitor);
                            visitorInActivityIndex++;
                        }
                    }

                    activityIndex++;
                }

                locationIndex++;
            }

            return plannedInVisitor;
        }


        private void FillFoodCourts(List<Visitor> visitors)
        {
            List<Activity> foodCourts = activityLocations
                .Where(location => location.Type == ActivityType.FoodCourt)
                .SelectMany(location => location.Activities)
                .ToList();

            int visitorIndex = 0;

            while (visitorIndex < visitors.Count)
            {
                Visitor visitor = visitors[visitorIndex];
                bool assigned = false;
                int foodCourtIndex = 0;

                while (!assigned && foodCourtIndex < foodCourts.Count)
                {
                    Activity foodCourt = foodCourts[foodCourtIndex];
                    if (foodCourt.Visitors.Count < foodCourt.Capacity)
                    {
                        foodCourt.Visitors.Add(visitor);
                        visitor.Schedule.AddActivity(foodCourt);
                        assigned = true;
                    }
                    foodCourtIndex++;
                }

                visitorIndex++;
            }
        }

        private void AssignVisitorsToCategory(Visitor visitor, ActivityType type, int minAssignments, int maxAssignments)
        {
            var activitiesGroupedByName = activityLocations
                .Where(loc => loc.Type == type)
                .SelectMany(loc => loc.Activities)
                .Where(activity => activity.Visitors.Count < activity.Capacity)
                .GroupBy(activity => activity.Name)
                .ToDictionary(group => group.Key, group => group.OrderBy(a => a.StartTime).ToList());

            int assigned = 0;

            if (type == ActivityType.Exhibition)
            {
                int totalSlotsRequired = maxAssignments * activityLocations.Count(loc => loc.Type == ActivityType.Exhibition);

                int locationIndex = 0;
                while (locationIndex < activityLocations.Count)
                {
                    ActivityLocation activityLocation = activityLocations[locationIndex];
                    if (activityLocation.Type == ActivityType.Exhibition)
                    {
                        int assignedInLocation = 0;
                        int activityIndex = 0;

                        while (activityIndex < activityLocation.Activities.Count)
                        {
                            Activity activity = activityLocation.Activities[activityIndex];
                            if (activity.Visitors.Count < activity.Capacity)
                            {
                                if (assigned < totalSlotsRequired && assignedInLocation < maxAssignments)
                                {
                                    if (!CheckTimeConflict(visitor, activity.StartTime, activity.EndTime))
                                    {
                                        activity.Visitors.Add(visitor);
                                        visitor.Schedule.AddActivity(activity);
                                        assigned++;
                                        assignedInLocation++;
                                    }
                                }
                            }
                            activityIndex++;
                        }
                    }
                    locationIndex++;
                }
            }
            else
            {
                List<string> preferences;

                if (type == ActivityType.ProductPresentation)
                {
                    preferences = visitor.ProductPresentationPreferences.Select(pref => pref.Name).ToList();
                }
                else if (type == ActivityType.Reading)
                {
                    preferences = visitor.ReadingPreferences.Select(pref => pref.Name).ToList();
                }
                else
                {
                    preferences = new List<string>();
                }

                int preferenceIndex = 0;
                while (preferenceIndex < preferences.Count)
                {
                    string preferredName = preferences[preferenceIndex];
                    if (activitiesGroupedByName.TryGetValue(preferredName, out var activities))
                    {
                        int activityIndex = 0;
                        while (activityIndex < activities.Count)
                        {
                            Activity activity = activities[activityIndex];
                            if (activity.Visitors.Count < activity.Capacity && assigned < maxAssignments)
                            {
                                if (!CheckTimeConflict(visitor, activity.StartTime, activity.EndTime))
                                {
                                    activity.Visitors.Add(visitor);
                                    visitor.Schedule.AddActivity(activity);
                                    assigned++;
                                }
                            }
                            activityIndex++;
                        }
                    }
                    preferenceIndex++;
                }

                if (type == ActivityType.ProductPresentation && assigned < minAssignments)
                {
                    List<Activity> allActivities = activitiesGroupedByName.Values.SelectMany(group => group).ToList();
                    int activityIndex = 0;

                    while (activityIndex < allActivities.Count)
                    {
                        Activity activity = allActivities[activityIndex];
                        if (activity.Visitors.Count < activity.Capacity && assigned < maxAssignments)
                        {
                            if (!CheckTimeConflict(visitor, activity.StartTime, activity.EndTime))
                            {
                                activity.Visitors.Add(visitor);
                                visitor.Schedule.AddActivity(activity);
                                assigned++;
                            }
                        }
                        activityIndex++;
                    }
                }
            }

            if (assigned < minAssignments)
            {
                int locationIndex = 0;
                while (locationIndex < activityLocations.Count)
                {
                    ActivityLocation activityLocation = activityLocations[locationIndex];
                    int activityIndex = 0;

                    while (activityIndex < activityLocation.Activities.Count)
                    {
                        Activity activity = activityLocation.Activities[activityIndex];
                        activity.Visitors.Remove(visitor);
                        activityIndex++;
                    }

                    visitor.Schedule.Exhibitions.Clear();
                    visitor.Schedule.FoodCourts.Clear();
                    visitor.Schedule.ProductPresentations.Clear();
                    visitor.Schedule.Readings.Clear();

                    locationIndex++;
                }
            }
        }

        private bool CheckTimeConflict(Visitor visitor, DateTime eventStart, DateTime eventEnd)
        {
            List<Activity> visitorActivities = visitor.Schedule.GetAllActivities();

            if (visitorActivities.Count == 0)
            {
                return false;
            }

            foreach (var activity in visitorActivities)
            {
                if (eventStart < activity.EndTime && eventEnd > activity.StartTime)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
