using EventFlowClassLibrary;
using EventFlowClassLibrary.enums;
using System.Diagnostics;

public class Program
{
    private static readonly Random random = new Random();
    public static readonly EventManager eventManager = new EventManager();

    static void Main(string[] args)
    {
        int amount = 2000;
        var visitors = GenerateVisitors(amount);

        // Perform the planning process
        eventManager.FillInActivities(visitors);

        Console.WriteLine("\nAfter Planning:");
        VerifyActivityVisitorCounts();
        // VerifyVisitorSchedules(visitors);
        int plannedinvistors = 0;
        foreach (var location in eventManager.activityLocations)
        {
            foreach (var activity in location.Activities)
            {
                if (activity.Type == ActivityType.FoodCourt)
                {
                    plannedinvistors += activity.Visitors.Count; // Use += to accumulate the count
                }
            }
        }


        amount -= plannedinvistors;

        Console.WriteLine("\nPlanning completed!");
        Console.WriteLine($"Amount of visitors planned in:{plannedinvistors}");
        Console.WriteLine($"Amount of visitors canceled:{amount}");
    }

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



    private static List<EventFlowClassLibrary.Activity> RandomPreferences(ActivityType type)
    {
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
    private static void VerifyActivityVisitorCounts()
    {
        foreach (var location in eventManager.activityLocations)
        {
            Console.WriteLine($"Location: {location.Name}");
            foreach (var activity in location.Activities)
            {
                Console.WriteLine($"{activity.Name} Visitors Amount: {activity.Visitors.Count} Starttime: {activity.StartTime} EndTime: {activity.EndTime}");
            }
        }
    }

}







