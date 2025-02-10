namespace Pulse.app.pages.choices.ExerciseTimerChoice;

public class StartTimer : IChoiceAdapter
{
    private readonly ExerciseTimerPage _page;

    public StartTimer(ExerciseTimerPage page)
    {
        _page = page;
    }
    public void Exec()
    {
        throw new NotImplementedException();
    }
}