namespace Pulse.app.pages.choices.TodoPageChoices;

public class UpdateTodo : IChoiceAdapter
{
    private readonly HabitTodoPage _page;

    public UpdateTodo(HabitTodoPage page)
    {
        _page = page;
    }

    public void Exec()
    {
        throw new NotImplementedException();
    }
}