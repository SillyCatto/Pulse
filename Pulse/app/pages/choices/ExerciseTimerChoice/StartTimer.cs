using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages.choices.ExerciseTimerChoice;

public class StartTimer : IChoiceAdapter
{
    private readonly ExerciseTimerPage _page;

    public StartTimer(ExerciseTimerPage page)
    {
        _page = page;
    }
    public async void Exec()
    {
        _page.RefreshMsg(":fire: [yellow]Let's start the grind...[/]");
    
        string exercise = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the name of [bold mediumorchid1_1]exercise[/]: (within 20 characters):")
                .PromptStyle(Constants.PromptInputStyle)
                .Validate(w => 
                    w.Length <= 20 
                        ? ValidationResult.Success() 
                        : ValidationResult.Error("[red]Exercise name must be within 20 characters.[/]")
                )
        );
    
        int minutes = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the [bold mediumorchid1_1]time[/] of exercise: (in minutes):")
                .PromptStyle(Constants.PromptInputStyle)
                .ValidationErrorMessage("[red]That's not a number.[/]")
        );
    
        if (!AnsiConsole.Confirm($"Start {minutes} minute timer for {exercise}?"))
        {
            _page.SetMsg("[magenta2_1]Timer cancelled.[/]");
            _page.Run();
        }
    
        AnsiConsole.Clear();
        int elapsedSeconds = StartExerciseTimer(exercise, minutes).GetAwaiter().GetResult();
        
        double elapsedMinutes = elapsedSeconds / 60.0;  // Convert to minutes
        string formattedTime = $"{elapsedSeconds / 60:D2}:{elapsedSeconds % 60:D2}";
    
        if (elapsedSeconds < minutes * 60)
        {
            _page.SetMsg("[red]Timer cancelled midway.[/]");
        }
        else
        {
            Console.Beep();
            _page.SetMsg($"[green]{exercise} done for {elapsedMinutes:F2} minutes![/]");
        }
    
        if (AnsiConsole.Confirm($"Save record for [bold]{exercise}[/] - [cyan]{formattedTime}[/]?"))
        {
            SaveRecord(exercise, formattedTime);
            _page.SetMsg($":check_mark: [green]Record saved![/]");
        }
    
        _page.Run();
    }

    private async Task<int> StartExerciseTimer(string exercise, int minutes)
    {
        int totalSeconds = minutes * 60;
        int elapsedSeconds = 0;
        bool isPaused = false;
        bool isStopped = false;
    
        using (var cts = new CancellationTokenSource())
        {
            // Start listening for pause/stop keys
            var inputTask = Task.Run(() =>
            {
                while (!isStopped)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.P)
                        isPaused = !isPaused;
                    else if (key == ConsoleKey.Escape)
                    {
                        isStopped = true;
                        cts.Cancel();
                    }
                }
            });
    
            try
            {
                await AnsiConsole.Live(new Panel(""))
                    .StartAsync(async ctx =>
                    {
                        while (elapsedSeconds < totalSeconds && !isStopped)
                        {
                            if (!isPaused)
                            {
                                int minutesLeft = (totalSeconds - elapsedSeconds) / 60;
                                int secondsLeft = (totalSeconds - elapsedSeconds) % 60;
                                int progressPercent = (elapsedSeconds * 100) / totalSeconds;
    
                                var timerText = new Markup($"[bold yellow]{exercise} Timer:[/] [bold cyan]{minutesLeft:D2}:{secondsLeft:D2}[/]");
                                string progressBar = $"[{new string('█', (progressPercent * 20) / 100)}{new string('░', 20 - (progressPercent * 20) / 100)}] {progressPercent}%";
                                var instructions = new Markup("Press [bold yellow]P[/] to pause, [bold red]Esc[/] to cancel");
    
                                var layout = new Rows(
                                    Align.Center(timerText),
                                    Align.Center(new Markup("\n")),
                                    Align.Center(new Markup(Markup.Escape(progressBar))),
                                    Align.Center(new Markup("\n")),
                                    Align.Center(instructions)
                                );
                                ctx.UpdateTarget(new Panel(layout).Expand());
    
                                await Task.Delay(1000, cts.Token);
                                elapsedSeconds++;
                            }
                        }
    
                        // Ensure final state update
                        if (!isStopped)
                        {
                            var doneText = Align.Center(new Markup("[bold green]Time's up! Well done![/]"));
                            ctx.UpdateTarget(new Panel(doneText).Expand());
                            Console.Beep();
                        }
                    });
    
            }
            catch (TaskCanceledException)
            {
                // Timer was cancelled
            }
            finally
            {
                // Stop listening for key presses when timer ends
                isStopped = true;
                await inputTask;
            }
        }
    
        return elapsedSeconds; // Return actual elapsed time
    }
    
    private void SaveRecord(string exercise, string time)
    {
        var exerciseModel = _page.GetDataModel();
        var records = exerciseModel.ToDict();
        string newIndex = (records.Count + 1).ToString();
        exerciseModel.AddRecord(newIndex, [exercise, time]);
        exerciseModel.Save();
    }
}