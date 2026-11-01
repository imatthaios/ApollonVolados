using System.Net;
using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

namespace ApollonVolados.Mobile.Tests.Fakes;

/// <summary>Records navigation requests instead of touching Shell.</summary>
public class FakeNavigationService : INavigationService
{
    public List<(string Route, IDictionary<string, object>? Parameters)> Navigations { get; } = [];
    public int BackCount { get; private set; }

    public Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
    {
        Navigations.Add((route, parameters));
        return Task.CompletedTask;
    }

    public Task GoBackAsync()
    {
        BackCount++;
        return Task.CompletedTask;
    }
}

/// <summary>Records alerts instead of displaying them.</summary>
public class FakeDialogService : IDialogService
{
    public List<(string Title, string Message)> Alerts { get; } = [];

    public Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        Alerts.Add((title, message));
        return Task.CompletedTask;
    }
}

/// <summary>Configurable <see cref="ILauncher"/> that records opened URIs.</summary>
public class FakeLauncher : ILauncher
{
    public List<Uri> Opened { get; } = [];
    public bool Result { get; set; } = true;
    public Exception? ThrowOnOpen { get; set; }

    public Task<bool> CanOpenAsync(Uri uri) => Task.FromResult(Result);

    public Task<bool> OpenAsync(Uri uri)
    {
        if (ThrowOnOpen != null) throw ThrowOnOpen;
        Opened.Add(uri);
        return Task.FromResult(Result);
    }

    public Task<bool> OpenAsync(OpenFileRequest request) => Task.FromResult(Result);

    public Task<bool> TryOpenAsync(Uri uri) => OpenAsync(uri);
}

/// <summary>Configurable <see cref="IPhoneDialer"/> that records dialled numbers.</summary>
public class FakePhoneDialer : IPhoneDialer
{
    public bool IsSupported { get; set; }
    public List<string> Dialled { get; } = [];
    public Exception? ThrowOnOpen { get; set; }

    public void Open(string number)
    {
        if (ThrowOnOpen != null) throw ThrowOnOpen;
        Dialled.Add(number);
    }
}

/// <summary>In-memory <see cref="IPreferences"/>.</summary>
public class FakePreferences : IPreferences
{
    public Dictionary<string, object?> Values { get; } = [];

    public bool ContainsKey(string key, string? sharedName = null) => Values.ContainsKey(key);
    public void Remove(string key, string? sharedName = null) => Values.Remove(key);
    public void Clear(string? sharedName = null) => Values.Clear();
    public void Set<T>(string key, T value, string? sharedName = null) => Values[key] = value;

    public T Get<T>(string key, T defaultValue, string? sharedName = null) =>
        Values.TryGetValue(key, out var value) && value is T typed ? typed : defaultValue;
}

/// <summary>Records theme changes instead of touching Application.Current.</summary>
public class FakeThemeService : IThemeService
{
    public ThemePreference Current { get; set; }
    public List<ThemePreference> Changes { get; } = [];

    public void Set(ThemePreference preference)
    {
        Current = preference;
        Changes.Add(preference);
    }

    public void ApplySaved() { }
}

/// <summary>In-memory <see cref="IWordPressService"/> with controllable results.</summary>
public class FakeWordPressService : IWordPressService
{
    public Func<bool, Task<List<WpPost>>> Handler { get; set; } = _ => Task.FromResult(new List<WpPost>());
    public List<bool> Calls { get; } = [];

    public Task<List<WpPost>> GetLatestPostsAsync(bool forceRefresh = false, CancellationToken cancellationToken = default)
    {
        Calls.Add(forceRefresh);
        return Handler(forceRefresh);
    }
}

/// <summary><see cref="HttpMessageHandler"/> that answers every request with a delegate.</summary>
public class StubHttpHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _respond;

    public StubHttpHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> respond)
    {
        _respond = respond;
    }

    public List<HttpRequestMessage> Requests { get; } = [];

    public static StubHttpHandler Json(string json, HttpStatusCode status = HttpStatusCode.OK) =>
        new((_, _) => Task.FromResult(new HttpResponseMessage(status)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        }));

    public static StubHttpHandler Throws(Exception ex) =>
        new((_, _) => Task.FromException<HttpResponseMessage>(ex));

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(request);
        return _respond(request, cancellationToken);
    }
}

/// <summary>Creates and deletes a unique temporary directory for file-system tests.</summary>
public sealed class TempDirectory : IDisposable
{
    public string Path { get; } =
        System.IO.Path.Combine(System.IO.Path.GetTempPath(), "apollon-tests-" + Guid.NewGuid().ToString("N"));

    public TempDirectory() => Directory.CreateDirectory(Path);

    public void Dispose()
    {
        if (Directory.Exists(Path))
            Directory.Delete(Path, recursive: true);
    }
}

/// <summary>Builders for test posts.</summary>
public static class TestPosts
{
    public static WpPost Create(int id, string title = "Title", string? link = null, string? image = null, DateTime? date = null) =>
        new()
        {
            Id = id,
            Title = new RenderedText { Rendered = title },
            Excerpt = new RenderedText { Rendered = "<p>Excerpt</p>" },
            Link = link ?? $"https://apollonvolados.gr/post-{id}/",
            Date = date ?? new DateTime(2025, 3, 5, 18, 30, 0),
            Embedded = image is null
                ? null
                : new Embedded { FeaturedMedia = [new FeaturedMedia { SourceUrl = image }] }
        };
}
