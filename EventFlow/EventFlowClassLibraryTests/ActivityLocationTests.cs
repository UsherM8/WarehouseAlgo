
using EventFlowClassLibrary.enums;

namespace EventFlowClassLibrary.Tests
{
    [TestClass]
    public class ActivityLocationTests
    {
        [TestMethod]
        public void GenerateSlotsTest()
        {
            // Arrange
            var activityLocation1 = new ActivityLocation()
            {
                Name = "Test",
                Type = enums.ActivityType.Exhibition,
                Activities = new List<Activity>()
            };

           var EventStartTime = new DateTime(2024, 12, 20, 9, 0, 0);
           var EventEndTime = new DateTime(2024, 12, 20, 17, 0, 0);

            // Act
            var result = activityLocation1.GenerateSlots(activityLocation1, 6, 30, activityLocation1.Type, EventStartTime, EventEndTime);
            // Assert
            Assert.AreEqual(6, result.Count); 
            Assert.IsTrue(result.All(x => x.StartTime >= EventStartTime && x.EndTime <= EventEndTime));// All activities are within the event time frame
        }

        [TestMethod]
        public void GenerateSlotsTestOverload()
        {
            // Arrange
            var activityLocation1 = new ActivityLocation()
            {
                Name = "Test",
                Type = ActivityType.Exhibition,
                Activities = new List<Activity>()
            };

            var EventStartTime = new DateTime(2024, 12, 20, 9, 0, 0);
            var EventEndTime = new DateTime(2024, 12, 20, 17, 0, 0);

            // Act
            var result = activityLocation1.GenerateSlots(activityLocation1, 17, 30, activityLocation1.Type, EventStartTime, EventEndTime);
            // Assert
            Assert.AreEqual(16, result.Count);// Even though we requested 17 slots, we can only fit 16 in the time frame provided.
            Assert.IsTrue(result.All(x => x.StartTime >= EventStartTime && x.EndTime <= EventEndTime));// All activities are within the event time frame
        }

        [TestMethod]
        public void GenerateFoodcourtSlots()
        {
            // Arrange
            var activityLocation1 = new ActivityLocation()
            {
                Name = "Test",
                Type = ActivityType.FoodCourt,
                Activities = new List<Activity>()
            };

            var EventStartTime = new DateTime(2024, 12, 20, 9, 0, 0);
            var EventEndTime = new DateTime(2024, 12, 20, 17, 0, 0);

            // Act
            var result = activityLocation1.GenerateSlots(activityLocation1, 6, 30, activityLocation1.Type, EventStartTime, EventEndTime);
            // Assert
            Assert.AreEqual(6, result.Count);// Even though we requested 17 slots, we can only fit 16 in the time frame provided.
            Assert.IsTrue(result[0].StartTime == new DateTime(2024, 12, 20, 11, 0, 0) && result[5].EndTime == new DateTime(2024, 12, 20, 14, 0, 0));//Breaks activities can only start at 11am
            Assert.IsTrue(result.All(x => x.StartTime >= EventStartTime && x.EndTime <= EventEndTime)); // All activities are within the event time frame
        }

        [TestMethod]
        public void GenerateReadingSlots()
        {
            // Arrange
            var activityLocation1 = new ActivityLocation()
            {
                Name = "Test",
                Type = ActivityType.Reading,
                Activities = new List<Activity>()
            };

                var reading1 = "Reading 1";
                var reading2 = "Reading 2";

            var EventStartTime = new DateTime(2024, 12, 20, 9, 0, 0);
            var EventEndTime = new DateTime(2024, 12, 20, 17, 0, 0);

            // Act
            var result = activityLocation1.GenerateSlots(activityLocation1, reading1, reading2, 8, 60, EventStartTime, EventEndTime);
            // Assert
            Assert.AreEqual(8, result.Count);// Even though we requested 17 slots, we can only fit 16 in the time frame provided.
            Assert.IsTrue(result[0].Name == reading1);
            Assert.IsTrue(result[1].Name == reading2);
            Assert.IsTrue(result[2].Name == reading1);
            Assert.IsTrue(result[3].Name == reading2);
            Assert.IsTrue(result[4].Name == reading1);
            Assert.IsTrue(result[5].Name == reading2);//// The activities should alternate between reading1 and reading2
            Assert.IsTrue(result.All(x => x.StartTime >= EventStartTime && x.EndTime <= EventEndTime)); // All activities are within the event time frame
        }
    }
}