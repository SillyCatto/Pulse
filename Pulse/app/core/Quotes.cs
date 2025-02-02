namespace Pulse.core;

public static class Quotes
{
    private static readonly List<string> quotes = new()
    {
        "Take care of your body. It's the only place you have to live. - Jim Rohn",
        "Exercise not only changes your body, it changes your mind, your attitude, and your mood.",
        "The secret of getting ahead is getting started. - Mark Twain",
        "Health is like money, we never have a true idea of its value until we lose it. - Josh Billings",
        "A one-hour workout is 4% of your day. No excuses.",
        "Your body hears everything your mind says. Stay positive!",
        "Movement is a medicine for creating change in a person’s physical, emotional, and mental states. - Carol Welch",
        "The groundwork of all happiness is health. - Leigh Hunt",
        "Don’t limit your challenges. Challenge your limits.",
        "The pain you feel today will be the strength you feel tomorrow.",
        "Mental health is not a destination, but a process. It's about how you drive, not where you're going. - Noam Shpancer",
        "Your present circumstances don’t determine where you can go; they merely determine where you start. - Nido Qubein",
        "Self-care is not a luxury, it’s a necessity.",
        "Health is Wealth",
        "Small steps in the right direction can turn out to be the biggest steps of your life.",
        "You are never too old to set another goal or to dream a new dream. - C.S. Lewis",
        "Do something today that your future self will thank you for.",
        "Your mind is a powerful thing. When you fill it with positive thoughts, your life will start to change.",
        
        "There are two blessings which many people waste: health and free time. - Sahih al-Bukhari:6412",
        "Cleanliness is half of faith. - Sahih Muslim:223"
    };
    
    private static readonly Random random = new();
    
    public static string GetRandom()
    {
        return quotes[random.Next(quotes.Count)];
    }
}