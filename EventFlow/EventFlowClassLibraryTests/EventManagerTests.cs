using EventFlowClassLibrary.enums;

namespace EventFlowClassLibrary.Tests
{
    [TestClass]
    public class EventManagerTests
    {
        [TestMethod]
        public void CheckTimeConflictTestTrue()
        {
            // Arrange
            var eventManager = new EventManager();
            var visitor = new Visitor { Schedule = new Schedule() };

            var eventStartTime = new DateTime(2024, 12, 20, 9, 0, 0);
            var eventEndTime = new DateTime(2024, 12, 20, 17, 0, 0);
            List<Visitor> visitors = new List<Visitor>();
            visitors.Add(visitor);

            // Act
           var plannedInVisitors = eventManager.FillInActivities(visitors);

            // Assert
            Assert.IsTrue(plannedInVisitors.All(v => v.Schedule.GetAllActivities().All(a => a.StartTime >= eventStartTime && a.EndTime <= eventEndTime)));// Activities are within the vent time frame
            Assert.IsTrue(plannedInVisitors.All(v => !v.Schedule.GetAllActivities().Any(a1 => v.Schedule.GetAllActivities().Any(a2 => a1 != a2 && a1.StartTime < a2.EndTime && a1.EndTime > a2.StartTime))), "Some activities overlap within a visitor's schedule.");
        }

        [TestMethod]
        public void PlanIn1Visitor()
        {
            // Arrange
            var eventManager = new EventManager();

            // Create visitors
            var visitors = new List<Visitor>
            {
                new Visitor
               {
                 Name = "Visitor 1",
                 ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 1" }, new Activity { Name = "Activity 2" }, new Activity { Name = "Activity 3" },},
                 ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 1" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 3" } },
                 Schedule = new Schedule()
               },
            };

            // Act
            eventManager.FillInActivities(visitors);

            // Assert

            // Visitor assignement to foodcourt
            Assert.AreEqual(1, eventManager.activityLocations[0].Activities[0].Visitors.Count, "Activity 1 should have exactly one visitor.");
            //visitor assignment to product presentation
            Assert.AreEqual(1, eventManager.activityLocations[5].Activities[0].Visitors.Count, "Activity 2 should have exactly one visitor.");
            Assert.AreEqual(1, eventManager.activityLocations[5].Activities[1].Visitors.Count, "Activity 2 should have exactly one visitor.");

            // Visitor counts per reading
            Assert.AreEqual(1, eventManager.activityLocations[10].Activities[7].Visitors.Count, "Activity 3 should have exactly one visitor.");
            Assert.AreEqual(0, eventManager.activityLocations[10].Activities[1].Visitors.Count, "Activity 4 should have exactly one visitor.");

            Assert.IsTrue(visitors[0].Schedule.GetAllActivities().Count == 31);
        }



        [TestMethod]
        public void AssignVisitorsTospecificProductpresentationPreferences()
        {
            // Arrange
            var eventManager = new EventManager();
            var visitors = new List<Visitor>
            {
              new Visitor
              {
                Name = "Visitor 1",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 7" }, new Activity { Name = "Activity 4" }, new Activity { Name = "Activity 6" },},
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 1" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 3" } },
                Schedule = new Schedule()
              },
              new Visitor
              {
                Name = "Visitor 2",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 1" }, new Activity { Name = "Activity 2" }, new Activity { Name = "Activity 2" } },
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 1" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 3" } },
                Schedule = new Schedule()
              },
              new Visitor
              {
                Name = "Visitor 3",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 3" }, new Activity { Name = "Activity 2" } , new Activity { Name = "Activity 1" }},
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 1" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 3" } },
                Schedule = new Schedule()
              }
            };
            // Act

            eventManager.FillInActivities(visitors);
      
             //Assert
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 1"));
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 2"));
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 3"));// When the visotor their preferences are not in the list of activities or full, they are added to other preferences.
            Assert.IsTrue(visitors.All(visitor => visitor.Schedule.ProductPresentations.Count == 2), "Not all visitors were assigned the correct number of activities."); 
            foreach (var visitor in visitors)
            {
                foreach (var activity in visitor.Schedule.ProductPresentations)
                {
                    Assert.IsTrue(visitor.ProductPresentationPreferences.Any(p => p.Name == activity.Name),
                        $"{visitor.Name} was assigned a product presentation ({activity.Name}) that is not in their preferences.");
                }
            }


        }

        [TestMethod]
        public void AssignVisitorsToSpecificReadingPreferences()
        {
            // Arrange
            var eventManager = new EventManager();
            var visitors = new List<Visitor>
            {
              new Visitor
              {
                Name = "Visitor 1",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "" }, new Activity { Name = "" }, new Activity { Name = "" },},
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 1" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 3" } },
                Schedule = new Schedule()
              },
              new Visitor
              {
                Name = "Visitor 2",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "" }, new Activity { Name = "" }, new Activity { Name = "" } },
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 3" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 1" } },
                Schedule = new Schedule()
              },
              new Visitor
              {
                Name = "Visitor 3",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 3" }, new Activity { Name = "Activity 2" } , new Activity { Name = "Activity 1" }},
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 4" }, new Activity { Name = "Reading 5" }, new Activity { Name = "Reading 7" } },
                Schedule = new Schedule()
              }
            };
            //Act

            eventManager.FillInActivities(visitors);

            // Assert
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 1"));
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 2"));
            Assert.IsTrue(visitors.All(visitor => visitor.Schedule.Readings.Count == 1), "Not all visitors were assigned the correct number of activities.");
            foreach (var visitor in visitors)
            {
                foreach (var activity in visitor.Schedule.Readings)
                {
                    Assert.IsTrue(visitor.ReadingPreferences.Any(p => p.Name == activity.Name),
                        $"{visitor.Name} was assigned a reading ({activity.Name}) that is not in their preferences.");
                }
            }
        }

        [TestMethod]
        public void AssignvisitorWithBothPreferences()
        {
            // Arrange
            var eventManager = new EventManager();
            var visitors = new List<Visitor>
            {
              new Visitor
              {
                Name = "Visitor 1",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 7" }, new Activity { Name = "Activity 4" }, new Activity { Name = "Activity 6" },},
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 1" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 3" } },
                Schedule = new Schedule()
              },
              new Visitor
              {
                Name = "Visitor 2",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 1" }, new Activity { Name = "Activity 2" }, new Activity { Name = "Activity 2" } },
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 3" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 1" } },
                Schedule = new Schedule()
              },
              new Visitor
              {
                Name = "Visitor 3",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 3" }, new Activity { Name = "Activity 2" } , new Activity { Name = "Activity 1" }},
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 4" }, new Activity { Name = "Reading 5" }, new Activity { Name = "Reading 7" } },
                Schedule = new Schedule()
              }
            };
            //Act

            eventManager.FillInActivities(visitors);

           //Assert
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 1"));
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 2"));
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 3"));
            Assert.IsTrue(visitors.All(visitor => visitor.Schedule.Readings.Count == 1), "Not all visitors were assigned the correct number of activities.");
            foreach (var visitor in visitors)
            {
                foreach (var activity in visitor.Schedule.ProductPresentations)
                {
                    Assert.IsTrue(visitor.ProductPresentationPreferences.Any(p => p.Name == activity.Name),
                        $"{visitor.Name} was assigned a product presentation ({activity.Name}) that is not in their preferences.");
                }
            }
            // Validate readings
            foreach (var visitor in visitors)
            {
                foreach (var activity in visitor.Schedule.Readings)
                {
                    Assert.IsTrue(visitor.ReadingPreferences.Any(p => p.Name == activity.Name),
                        $"{visitor.Name} was assigned a reading ({activity.Name}) that is not in their preferences.");
                }
            }

        }

        [TestMethod]
        public void AssignVisitorsToCorrectAmountOfExhibitons()
        {
            //Arrange
            var eventManager = new EventManager();
            var visitors = new List<Visitor>
            {
              new Visitor
              {
                Name = "Visitor 1",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 7" }, new Activity { Name = "Activity 4" }, new Activity { Name = "Activity 6" },},
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 1" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 3" } },
                Schedule = new Schedule()
              },
              new Visitor
              {
                Name = "Visitor 2",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 1" }, new Activity { Name = "Activity 2" }, new Activity { Name = "Activity 2" } },
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 3" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 1" } },
                Schedule = new Schedule()
              },
              new Visitor
              {
                Name = "Visitor 3",
                ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 3" }, new Activity { Name = "Activity 2" } , new Activity { Name = "Activity 1" }},
                ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 4" }, new Activity { Name = "Reading 5" }, new Activity { Name = "Reading 7" } },
                Schedule = new Schedule()
              }
            };

            //Act
            eventManager.FillInActivities(visitors);


            //Assert
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 1"));
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 2"));
            Assert.IsTrue(visitors.All(visitor => visitor.Schedule.Exhibitions.Count == 27), "Not all visitors were assigned the correct number of activities.");
            foreach (var location in eventManager.activityLocations)
            {
                if (location.Type == ActivityType.Exhibition)
                {
                    foreach (var activity in location.Activities)
                    {
                        foreach (var visitor in visitors)
                        {
                            var assignedSlots = location.Activities.Where(activity => activity.Visitors.Contains(visitor)).ToList();
                            Assert.AreEqual(9, assignedSlots.Count, $"The visitor should be assigned to 9 slots in {location.Name}, but was assigned to {assignedSlots.Count}.");
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void AssignVisitorsToCategory_FoodCourt()
        {
            //Arrange
            var eventManager = new EventManager();
            var visitors = new List<Visitor>
            {
                new Visitor
               {
                 Name = "Visitor 1",
                 ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 1" }, new Activity { Name = "Activity 2" }, new Activity { Name = "Activity 3" },},
                 ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 1" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 3" } },
                 Schedule = new Schedule()
               },
                new Visitor
               {
                 Name = "Visitor 2",
                 ProductPresentationPreferences = new List<Activity> { new Activity { Name = "Activity 1" }, new Activity { Name = "Activity 2" }, new Activity { Name = "Activity 3" },},
                 ReadingPreferences = new List<Activity> { new Activity { Name = "Reading 1" }, new Activity { Name = "Reading 2" }, new Activity { Name = "Reading 3" } },
                 Schedule = new Schedule()
               },
            };

            //Act
            eventManager.FillInActivities(visitors);


            //Assert
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 1"));
            Assert.IsTrue(visitors.Any(visitor => visitor.Name == "Visitor 2"));
            Assert.IsTrue(visitors.All(visitor => visitor.Schedule.FoodCourts.Count == 1), "Not all visitors were assigned the correct number of activities.");
        }


    }
}
