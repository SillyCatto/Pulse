namespace Pulse.app.pages.choices.TodoPageChoices;

public class AddTodo : IChoiceAdapter
{
    private readonly HabitTodoPage _page;

    public AddTodo(HabitTodoPage page)
    {
        _page = page;
    }

    public void Exec()
    {
        throw new NotImplementedException();
    }
}