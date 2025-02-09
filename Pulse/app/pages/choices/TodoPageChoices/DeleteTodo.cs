namespace Pulse.app.pages.choices.TodoPageChoices;

public class DeleteTodo : IChoiceAdapter
{
    private readonly HabitTodoPage _page;

    public DeleteTodo(HabitTodoPage page)
    {
        _page = page;
    }

    public void Exec()
    {
        throw new NotImplementedException();
    }
}