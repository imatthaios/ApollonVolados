namespace ApollonVolados.Mobile.Services;

/// <summary>Shows simple alerts to the user. Abstracted so view models can be unit-tested.</summary>
public interface IDialogService
{
    Task ShowAlertAsync(string title, string message, string cancel = "OK");
}

/// <summary>Production <see cref="IDialogService"/> that displays alerts on the current Shell page.</summary>
public class ShellDialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message, string cancel = "OK") =>
        Shell.Current.DisplayAlertAsync(title, message, cancel);
}
