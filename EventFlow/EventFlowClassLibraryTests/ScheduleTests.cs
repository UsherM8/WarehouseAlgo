using EventFlowClassLibrary.enums;
using NuGet.Frameworks;

namespace EventFlowClassLibrary.Tests
{
    [TestClass]
    public class ScheduleTests
    {
        [TestMethod]
        public void AddActivityToVisitor()
        {
            // Arrange 

            var visitor = new Visitor();
            var activity = new Activity(){Type = ActivityType.Reading };
            var activity2 = new Activity() { Type = ActivityType.ProductPresentation };
            var activity3 = new Activity() { Type = ActivityType.Exhibition };
            var activity4 = new Activity() { Type = ActivityType.FoodCourt };

            visitor.Schedule.AddActivity(activity); 
            visitor.Schedule.AddActivity(activity2);
            visitor.Schedule.AddActivity(activity3);
            visitor.Schedule.AddActivity(activity4);
            // Act
            var result = visitor.Schedule.Readings;
            var result2 = visitor.Schedule.ProductPresentations;
            var result3 = visitor.Schedule.Exhibitions;
            var result4 = visitor.Schedule.FoodCourts;

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result2.Count);
            Assert.AreEqual(1, result3.Count);
            Assert.AreEqual(1, result4.Count);
            Assert.IsTrue(result.Contains(activity));
            Assert.IsTrue(result2.Contains(activity2));
            Assert.IsTrue(result3.Contains(activity3));
            Assert.IsTrue(result4.Contains(activity4));
        }

        [TestMethod]
        public void GetAllActivitiesOfUser()
        {
            // Arrange 

            var visitor = new Visitor();
            var activity = new Activity() { Type = ActivityType.Reading };
            var activity2 = new Activity() { Type = ActivityType.ProductPresentation };
            var activity3 = new Activity() { Type = ActivityType.Exhibition };
            var activity4 = new Activity() { Type = ActivityType.FoodCourt };

            visitor.Schedule.AddActivity(activity);
            visitor.Schedule.AddActivity(activity2);
            visitor.Schedule.AddActivity(activity3);
            visitor.Schedule.AddActivity(activity4);
            // Act
            var result = visitor.Schedule.GetAllActivities();

            // Assert
            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result.Contains(activity));
            Assert.IsTrue(result.Contains(activity2));
            Assert.IsTrue(result.Contains(activity3));
            Assert.IsTrue(result.Contains(activity4));
        }


        [TestMethod]
        public void GetAllActivitiesOfUserEmpty()
        {
            // Arrange 
            var visitor = new Visitor();

            // Act
            var result = visitor.Schedule.GetAllActivities();

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}