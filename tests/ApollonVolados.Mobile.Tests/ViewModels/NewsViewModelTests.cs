using System.ComponentModel;
using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.Services;
using ApollonVolados.Mobile.Tests.Fakes;
using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Tests.ViewModels;

public class NewsViewModelTests
{
    private readonly FakeWordPressService _service = new();
    private readonly FakeNavigationService _navigation = new();

    private NewsViewModel CreateSut() => new(_service, _navigation);

    private void Returns(params int[] ids) =>
        _service.Handler = _ => Task.FromResult(ids.Select(id => TestPosts.Create(id)).ToList());

    private void Fails() =>
        _service.Handler = _ => Task.FromException<List<WpPost>>(new HttpRequestException("offline"));

    [Fact]
    public void Starts_empty_and_idle()
    {
        var sut = CreateSut();

        Assert.Empty(sut.Posts);
        Assert.False(sut.IsBusy);
        Assert.False(sut.HasError);
        Assert.Null(sut.ErrorMessage);
    }

    [Fact]
    public async Task Load_populates_posts_in_order()
    {
        Returns(3, 1, 2);
        var sut = CreateSut();

        await sut.LoadAsync();

        Assert.Equal([3, 1, 2], sut.Posts.Select(p => p.Id));
        Assert.False(sut.IsBusy);
        Assert.False(sut.HasError);
    }

    [Fact]
    public async Task Load_uses_cache_and_refresh_forces_network()
    {
        var sut = CreateSut();

        await sut.LoadAsync();
        await sut.RefreshAsync();

        Assert.Equal([false, true], _service.Calls);
    }

    [Fact]
    public async Task Load_replaces_previous_posts()
    {
        var sut = CreateSut();
        Returns(1, 2);
        await sut.LoadAsync();

        Returns(9);
        await sut.LoadAsync();

        Assert.Equal([9], sut.Posts.Select(p => p.Id));
    }

    [Fact]
    public async Task Load_failure_sets_error_and_does_not_throw()
    {
        Fails();
        var sut = CreateSut();

        await sut.LoadAsync();

        Assert.True(sut.HasError);
        Assert.Equal(NewsViewModel.LoadErrorMessage, sut.ErrorMessage);
        Assert.False(sut.IsBusy);
    }

    [Fact]
    public async Task Failed_refresh_keeps_posts_already_shown()
    {
        var sut = CreateSut();
        Returns(1, 2);
        await sut.LoadAsync();

        Fails();
        await sut.RefreshAsync();

        Assert.Equal([1, 2], sut.Posts.Select(p => p.Id));
        Assert.True(sut.HasError);
    }

    [Fact]
    public async Task Successful_load_clears_previous_error()
    {
        var sut = CreateSut();
        Fails();
        await sut.LoadAsync();

        Returns(1);
        await sut.LoadAsync();

        Assert.False(sut.HasError);
        Assert.Null(sut.ErrorMessage);
    }

    [Fact]
    public async Task HasError_change_is_notified()
    {
        Fails();
        var sut = CreateSut();
        var changed = new List<string?>();
        sut.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        await sut.LoadAsync();

        Assert.Contains(nameof(NewsViewModel.ErrorMessage), changed);
        Assert.Contains(nameof(NewsViewModel.HasError), changed);
    }

    [Fact]
    public async Task IsBusy_is_true_while_loading()
    {
        var gate = new TaskCompletionSource<List<WpPost>>();
        _service.Handler = _ => gate.Task;
        var sut = CreateSut();

        var load = sut.LoadAsync();
        Assert.True(sut.IsBusy);

        gate.SetResult([TestPosts.Create(1)]);
        await load;
        Assert.False(sut.IsBusy);
    }

    [Fact]
    public async Task IsBusy_change_is_notified_twice()
    {
        var sut = CreateSut();
        var busyChanges = 0;
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NewsViewModel.IsBusy)) busyChanges++;
        };

        await sut.LoadAsync();

        Assert.Equal(2, busyChanges);
    }

    [Fact]
    public async Task Concurrent_loads_are_ignored()
    {
        var gate = new TaskCompletionSource<List<WpPost>>();
        _service.Handler = _ => gate.Task;
        var sut = CreateSut();

        var first = sut.LoadAsync();
        await sut.LoadAsync();
        await sut.RefreshAsync();

        gate.SetResult([TestPosts.Create(1)]);
        await first;

        Assert.Single(_service.Calls);
        Assert.Single(sut.Posts);
    }

    [Fact]
    public async Task Load_command_runs_load()
    {
        Returns(4);
        var sut = CreateSut();

        await sut.LoadCommand.ExecuteAsync(null);

        Assert.Equal([4], sut.Posts.Select(p => p.Id));
    }

    [Fact]
    public async Task Refresh_command_forces_network()
    {
        var sut = CreateSut();

        await sut.RefreshCommand.ExecuteAsync(null);

        Assert.Equal([true], _service.Calls);
    }

    [Fact]
    public async Task OpenPost_navigates_to_detail_with_url()
    {
        var sut = CreateSut();
        var item = new NewsItemViewModel(TestPosts.Create(1, link: "https://apollonvolados.gr/a/"));

        await sut.OpenPostAsync(item);

        var (route, parameters) = Assert.Single(_navigation.Navigations);
        Assert.Equal(Routes.NewsDetail, route);
        Assert.Equal("https://apollonvolados.gr/a/", parameters![Routes.UrlParameter]);
    }

    [Fact]
    public async Task OpenPost_command_navigates()
    {
        var sut = CreateSut();

        await sut.OpenPostCommand.ExecuteAsync(new NewsItemViewModel(TestPosts.Create(1)));

        Assert.Single(_navigation.Navigations);
    }

    [Fact]
    public async Task OpenPost_ignores_null()
    {
        await CreateSut().OpenPostAsync(null);

        Assert.Empty(_navigation.Navigations);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task OpenPost_ignores_items_without_link(string link)
    {
        var post = TestPosts.Create(1);
        post.Link = link;

        await CreateSut().OpenPostAsync(new NewsItemViewModel(post));

        Assert.Empty(_navigation.Navigations);
    }

    [Fact]
    public async Task OpenPost_ignores_null_link()
    {
        var post = TestPosts.Create(1);
        post.Link = null;

        await CreateSut().OpenPostAsync(new NewsItemViewModel(post));

        Assert.Empty(_navigation.Navigations);
    }

    [Fact]
    public async Task Only_the_first_post_is_featured()
    {
        Returns(5, 6, 7);
        var sut = CreateSut();

        await sut.LoadAsync();

        Assert.Equal([true, false, false], sut.Posts.Select(p => p.IsFeatured));
    }

    [Fact]
    public async Task Reload_keeps_a_single_featured_post()
    {
        var sut = CreateSut();
        Returns(1, 2);
        await sut.LoadAsync();
        Returns(3, 4);
        await sut.RefreshAsync();

        Assert.Equal(3, Assert.Single(sut.Posts, p => p.IsFeatured).Id);
    }

    [Fact]
    public async Task Pull_to_refresh_indicator_is_reset_after_success()
    {
        var sut = CreateSut();
        sut.IsRefreshing = true; // set by RefreshView when the user pulls

        await sut.RefreshCommand.ExecuteAsync(null);

        Assert.False(sut.IsRefreshing);
    }

    [Fact]
    public async Task Pull_to_refresh_indicator_is_reset_after_failure()
    {
        Fails();
        var sut = CreateSut();
        sut.IsRefreshing = true;

        await sut.RefreshAsync();

        Assert.False(sut.IsRefreshing);
        Assert.True(sut.HasError);
    }

    [Fact]
    public async Task Pull_during_a_running_load_is_reset_when_that_load_finishes()
    {
        var gate = new TaskCompletionSource<List<ApollonVolados.Mobile.Models.WpPost>>();
        _service.Handler = _ => gate.Task;
        var sut = CreateSut();

        var load = sut.LoadAsync();
        sut.IsRefreshing = true;
        await sut.RefreshAsync(); // ignored: already busy
        Assert.True(sut.IsRefreshing);

        gate.SetResult([TestPosts.Create(1)]);
        await load;

        Assert.False(sut.IsRefreshing);
    }

    [Fact]
    public void Is_observable()
    {
        Assert.IsAssignableFrom<INotifyPropertyChanged>(CreateSut());
    }
}
