using Pulse.Pages;
using Pulse.utils;

namespace Pulse.core;

public class App
{
    private readonly PageManager _pageManager;

    public App()
    {
        _pageManager = new PageManager();
        
        // register pages
        _pageManager.RegisterPage("Home", () => new HomePage(_pageManager));
        _pageManager.RegisterPage("BMI Calculator", () => new BMIPage());
        _pageManager.RegisterPage("Healthy Habit Todos", () => new HabitTodoPage());
        _pageManager.RegisterPage("Mental Health Tracker", () => new MentalHealthPage());
        _pageManager.RegisterPage("Report", () => new ReportPage());
        
    }

    public void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        _pageManager.Navigate("Home");
        test();
    }

    private void test()
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {
                "user", new Dictionary<string, object>
                {
                    { "name", "Alice" } 
                    
                } 
                
            }
        };

        var writer = new JSONFileWriter("user.json");
        writer.Write(data);
    }
}