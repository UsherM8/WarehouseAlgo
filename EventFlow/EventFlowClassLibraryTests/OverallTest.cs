using EventFlowClassLibrary.enums;
using System;
namespace EventFlowClassLibrary.Tests
{
    [TestClass]
    public class OverallTest
    {
        private static readonly Random random = new Random();
        private static List<Visitor> GenerateVisitors(int count)
        {
            var visitors = new List<Visitor>();

            for (int i = 1; i <= count; i++)
            {
                visitors.Add(new Visitor
                {
                    Name = $"Visitor {i}",
                    ReadingPreferences = RandomPreferences(ActivityType.Reading),
                    ProductPresentationPreferences = RandomPreferences(ActivityType.ProductPresentation),
                    Schedule = new Schedule()
                });
            }

            return visitors;
        }

        private static List<Activity> RandomPreferences(ActivityType type)
        {
            var eventManager = new EventManager();
            var activities = eventManager.activityLocations
                .SelectMany(location => location.Activities)
                .Where(activity => activity.Type == type)
                .GroupBy(activity => activity.Name) // Group by activity name to eliminate duplicates
                .Select(group => group.First())    // Take one activity per name
            .OrderBy(activity => random.Next())       // Shuffle activities
                .Take(3)                           // Take up to 3 unique preferences
                .ToList();

            return activities;
        }




        [TestMethod]
        public void EventFlow_OverallTest()
        {
            // Arrange
            var eventManager = new EventManager();
            var visitors = GenerateVisitors(2000);
            var eventStartTime = new DateTime(2024, 12, 20, 9, 0, 0);
            var eventEndTime = new DateTime(2024, 12, 20, 17, 0, 0);
            List<Visitor> PlannedInvisitors = new List<Visitor>();

            // Act
            var plannedInVisitors = eventManager.FillInActivities(visitors);

            // Assert

            Assert.IsTrue(PlannedInvisitors.All(v => v.Schedule.GetAllActivities().All(a => a.StartTime >= eventStartTime && a.EndTime <= eventEndTime)));// Activities are within the vent time frame
            Assert.IsTrue(plannedInVisitors.All(v => !v.Schedule.GetAllActivities().Any(a1 => v.Schedule.GetAllActivities().Any(a2 => a1 != a2 && a1.StartTime < a2.EndTime && a1.EndTime > a2.StartTime))), "Some activities overlap within a visitor's schedule.");

            foreach (var location in eventManager.activityLocations)
            {
                foreach (var activity in location.Activities)
                {
                    Assert.IsTrue(activity.Visitors.Count <= location.Capacity, $"Activity {activity.Name} at {location.Name} exceeds capacity.");
                }
            }
            foreach (var visitor in plannedInVisitors)
            {
                Assert.IsTrue(visitor.Schedule.ProductPresentations.Count == 2);
            }
            foreach (var visitor in plannedInVisitors)
            {
                Assert.IsTrue(visitor.Schedule.Exhibitions.Count == 27);

            }
            foreach (var visitor in plannedInVisitors)
            {
                Assert.IsTrue(visitor.Schedule.Readings.Count == 1);

            }
            foreach (var visitor in plannedInVisitors)
            {
                Assert.IsTrue(visitor.Schedule.FoodCourts.Count == 1);

            }
            foreach (var visitor in plannedInVisitors)
            {
                foreach (var activity in visitor.Schedule.Readings)
                {
                    Assert.IsTrue(visitor.ReadingPreferences.Any(p => p.Name == activity.Name),
                        $"{visitor.Name} was assigned a reading ({activity.Name}) that is not in their preferences.");
                }
            }

        }
    }
}
