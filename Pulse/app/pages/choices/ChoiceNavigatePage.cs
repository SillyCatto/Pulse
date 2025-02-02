using Pulse.core;

namespace Pulse.app.pages.choices;

public class ChoiceNavigatePage : IChoiceAdapter
{
    private readonly PageManager _pageManager;
    private readonly string _pageName;

    public ChoiceNavigatePage(PageManager pageManager, string pageName)
    {
        _pageManager = pageManager;
        _pageName = pageName;
    }

    public void Exec()
    {
        _pageManager.Navigate(_pageName);
    }
}