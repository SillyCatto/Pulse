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
    
        AnsiConsole.WriteLine();
        if (!AnsiConsole.Confirm($"Start [bold mediumorchid1_1]{minutes} minute[/] timer for [bold mediumorchid1_1]{exercise}[/]?"))
        {
            _page.SetMsg(":cross_mark:  [magenta2_1]Timer cancelled.[/]");
            _page.Run();
        }
    
        AnsiConsole.Clear();
        int elapsedSeconds = StartExerciseTimer(exercise, minutes).GetAwaiter().GetResult();
        
        if (elapsedSeconds < minutes * 60)
        {
            double elapsedMinutes = elapsedSeconds / 60.0;
            string formattedTime = $"{elapsedSeconds / 60:D2}:{elapsedSeconds % 60:D2}";
            
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine(":cross_mark:  [red]Timer cancelled.[/]");
            AnsiConsole.MarkupLine($"[green]{exercise} done for {elapsedMinutes:F2} minutes![/]");

            if (AnsiConsole.Confirm($"Save record: [bold magenta2_1]{exercise}[/] - [bold cyan]{formattedTime}[/]?"))
            {
                SaveRecord(exercise, formattedTime);
                _page.SetMsg(":check_mark:  [green]Record saved![/]");
            }
            else
            {
                _page.SetMsg($":cross_mark:  [yellow]Record discarded: [/][deepskyblue1]{exercise} for {elapsedMinutes:F2} minutes[/]");
            }
        }
        else
        {
            Console.Beep();
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
            var inputTask = Task.Run(() =>
            {
                while (!isStopped)
                {
                    if (Console.KeyAvailable) // Only read if key is pressed
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
    
                                var exerciseName = new Markup($"[green1]{exercise}[/]");
                                var timer = new Markup($"[bold black on turquoise2] {minutesLeft:D2}:{secondsLeft:D2} [/]");
                                string progressBar = $"[{new string('█', (progressPercent * 20) / 100)}{new string('░', 20 - (progressPercent * 20) / 100)}] {progressPercent}%";
                                var instructions = new Markup("Press [bold yellow]P[/] to pause, [bold red]Esc[/] to cancel");
    
                                var layout = new Rows(
                                    Align.Center(new Markup("\n")),
                                    Align.Center(exerciseName),
                                    Align.Center(new Markup("\n")),
                                    Align.Center(timer),
                                    Align.Center(new Markup("\n")),
                                    Align.Center(new Markup(Markup.Escape(progressBar))),
                                    Align.Center(new Markup("\n")),
                                    Align.Center(instructions)
                                );
                                ctx.UpdateTarget(new Panel(layout)
                                    .Expand()
                                    .Border(BoxBorder.Double)
                                    .Header("[bold darkorange] TIMER [/]", Justify.Center)
                                );
    
                                await Task.Delay(1000, cts.Token);
                                elapsedSeconds++;
                            }
                        }
    
                        isStopped = true; // Ensure timer stops
                    });
    
                // timer expired
                if (elapsedSeconds >= totalSeconds)
                {
                    Console.Beep();
                    string formattedTime = $"{minutes:D2}:00";
                    SaveRecord(exercise, formattedTime);
                    _page.SetMsg(":fire: [bold lime]Time's up! Well done![/]");
                }
            }
            catch (TaskCanceledException)
            {
                // timer cancelled
            }
            finally
            {
                isStopped = true;
                cts.Cancel();
                await inputTask; // Ensure input thread exits
            }
        }
    
        return elapsedSeconds;
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
