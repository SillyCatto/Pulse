using Pulse.core;

namespace Pulse.app.pages.choices;

public class ChoiceBackToHome : IChoiceAdapter
{
    private readonly PageManager _pageManager;

    public ChoiceBackToHome(PageManager pageManager)
    {
        _pageManager = pageManager;
    }
    public void Exec()
    {
        _pageManager.Navigate("Home");
    }
}