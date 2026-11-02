namespace ApollonVolados.Mobile.Services;

/// <summary>
/// Thin abstraction over Shell navigation so view models can navigate without
/// touching <see cref="Shell.Current"/> directly (which makes them unit-testable).
/// </summary>
public interface INavigationService
{
    /// <summary>Navigates to <paramref name="route"/>, passing optional query parameters.</summary>
    Task GoToAsync(string route, IDictionary<string, object>? parameters = null);

    /// <summary>Pops the current page.</summary>
    Task GoBackAsync();
}

/// <summary>Production <see cref="INavigationService"/> backed by <see cref="Shell.Current"/>.</summary>
public class ShellNavigationService : INavigationService
{
    public Task GoToAsync(string route, IDictionary<string, object>? parameters = null) =>
        parameters is null
            ? Shell.Current.GoToAsync(route)
            : Shell.Current.GoToAsync(route, parameters);

    public Task GoBackAsync() => Shell.Current.GoToAsync("..");
}
