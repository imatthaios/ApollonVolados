using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.Services;
using System.Collections.ObjectModel;

namespace ApollonVolados.Mobile.ViewModels;

public partial class NewsViewModel : ObservableObject
{
    private readonly WordPressService _service;

    public ObservableCollection<NewsItemViewModel> Posts { get; } = [];

    [ObservableProperty]
    private bool isBusy;

    public NewsViewModel(WordPressService service)
    {
        _service = service;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Posts.Clear();

            var posts = await _service.GetLatestPostsAsync();

            foreach (var post in posts)
            {
                Posts.Add(new NewsItemViewModel(post));
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}