# Apollon Volados

Mobile app for **Α.Σ. Απόλλων Βωλάδος** (founded 1924), built with .NET MAUI for iOS and Android.

| Tab | What it shows |
| --- | --- |
| **Αρχική** (Home) | Club crest and mission statement |
| **Νέα** (News) | Latest posts from the club's WordPress site, cached for offline use |
| **Τμήματα** (Teams) | Club departments; each opens its page on apollonvolados.gr |
| **Σύλλογος** (Club) | History timeline, milestones, contact buttons (phone, email, website, map) and the **appearance switch** (Automatic / Light / Dark) |

## Requirements

- .NET SDK 10.0.2xx or newer (pinned in [`global.json`](global.json), rolls forward to the latest feature band)
- MAUI workloads: `dotnet workload install maui-ios maui-android`
- **iOS:** macOS with an Xcode version matching the installed iOS workload (currently the iOS 27 SDK). Minimum supported iOS version is 15.0.
- **Android:** Android SDK + JDK 17. Minimum supported Android version is API 26.

## Build and run

From the repository root:

```bash
dotnet build ApollonVolados.slnx
```

Run in the iOS Simulator:

```bash
dotnet build ApollonVolados.Mobile -t:Run -f net10.0-ios -r iossimulator-arm64
```

Run on an Android emulator/device:

```bash
dotnet build ApollonVolados.Mobile -t:Run -f net10.0-android
```

Release builds:

```bash
dotnet publish ApollonVolados.Mobile -f net10.0-ios -c Release
```

```bash
dotnet publish ApollonVolados.Mobile -f net10.0-android -c Release
```

The iOS Release configuration signs with the certificate fingerprint in `ApollonVolados.Mobile.csproj` (`CodesignKey`). Override it with `-p:CodesignKey=...` on another machine. VS Code users get the same commands as tasks in [`.vscode/tasks.json`](.vscode/tasks.json).

If the simulator shows an old build (for example after editing `Info.plist`), delete `ApollonVolados.Mobile/bin` and `obj` and rebuild.

## Tests

```bash
dotnet test tests/ApollonVolados.Mobile.Tests
```

The xUnit tests cover models, services and view models without a device or simulator. This works because the app project also has a plain `net10.0` target (a library, never shipped) that the test project references, and because the view models depend on interfaces rather than on `Shell.Current` or MAUI's static platform APIs.

## Publishing

[`docs/PUBLISHING.md`](docs/PUBLISHING.md) is the step-by-step guide to publishing the app to the App Store and Google Play under the club's own non-profit organization accounts: D-U-N-S, Apple's fee waiver, signing, store listings, privacy forms and the pre-submission checklist. Store-ready icons are in `docs/store/`.

## Design system

The visual design is specified in [`docs/DESIGN.md`](docs/DESIGN.md): a "Heritage Editorial" direction with crest green, bronze and parchment colours, a serif/sans type scale, spacing and radius tokens, and the *crest ring* signature. It is implemented in two resource dictionaries:

- `Resources/Styles/Colors.xaml`: colour tokens, each with a Light and Dark value.
- `Resources/Styles/Styles.xaml`: font, spacing and elevation tokens, implicit control styles, the `Text.*` type scale and component styles (`Card`, `Button.Primary`, `CrestDisc`, `CrestRule.*` …).

Pages compose these styles and don't set colours, fonts, sizes or radii inline. For new UI, add or extend a style instead.

Light and dark mode are both supported. By default the app follows the phone's setting; the Club tab's *Εμφάνιση* switch lets users force Light or Dark. The choice is saved with MAUI `Preferences` (`Services/ThemeService.cs`) and applied at startup via `Application.UserAppTheme`, which every `AppThemeBinding` in `Styles.xaml` follows.

The app icon is an adaptive icon: `Resources/AppIcon/appicon.svg` (crest-green background) plus `appicon_fg.png` (the crest in its bronze ring, sized for Android's safe zone). MAUI generates every iOS and Android size from these two files.

## Project layout

```
ApollonVolados.Mobile/
  MauiProgram.cs        Dependency injection: services, view models, pages, fonts
  App.xaml(.cs)         Application resources; creates the window with AppShell
  AppShell.xaml(.cs)    Splash page + four-tab TabBar; registers pushed routes
  Models/               WordPress DTOs (WpPost, …) and static content types
  Services/             WordPressService, NewsCacheService, navigation/dialog abstractions, Routes
  ViewModels/           One per page (CommunityToolkit.Mvvm source generators)
  Views/                XAML pages and reusable Controls/
  Converters/           XAML value converters
  Platforms/            Android / iOS entry points and manifests
  Resources/Styles/     Colors.xaml + Styles.xaml design system
  Resources/            Icons, splash, crest image
tests/
  ApollonVolados.Mobile.Tests/   xUnit tests and fakes
docs/
  DESIGN.md             Design brief and tokens
```

## How it works

### News and caching

`WordPressService` fetches `https://apollonvolados.gr/wp-json/wp/v2/posts?_embed` (`_embed` includes the featured image). `NewsCacheService` stores the result as JSON in the app's cache directory.

1. If a cache younger than 6 hours exists, it is returned and the network is not used.
2. Otherwise the posts are downloaded and written to the cache.
3. If the download fails (offline, timeout, server error or invalid JSON), an **expired** cache is returned instead.
4. If there is no cache at all, the error reaches `NewsViewModel`, which shows a message with a retry button.

The retry button calls `RefreshCommand`, which skips step 1. A corrupt cache file counts as a cache miss, and failing to write the cache never hides freshly downloaded posts.

`WpPost` strips HTML tags and decodes entities for titles and excerpts. Posts without a featured image use the bundled crest (`crest.png`).

### Navigation

View models navigate through `INavigationService` (Shell in the app, a fake in tests). Routes and query-parameter names live in `Services/Routes.cs`. Shell query keys are case-sensitive, so the receiving pages' `[QueryProperty]` attributes use the same constant, and `RoutesTests` checks this.

| Route | Page | Parameter |
| --- | --- | --- |
| `//home` | `HomePage` (tab) | – |
| `NewsDetailPage` | Article in a WebView | `url` |
| `TeamWebPage` | Team page in a WebView | `url` |

### Contact buttons

`ClubViewModel` uses MAUI's `ILauncher` and `IPhoneDialer` (registered from `Launcher.Default` and `PhoneDialer.Default`) and `IDialogService` for alerts. When a device can't call, send email or open a link (for example the simulator), the app shows an alert instead of crashing.

### iOS scene life cycle

Apps built with the iOS 27 SDK must use the UIScene life cycle or they quit at launch. `Platforms/iOS/Info.plist` declares a `UIApplicationSceneManifest` that points to `Platforms/iOS/SceneDelegate.cs`, which derives from MAUI's `MauiUISceneDelegate`.

## Continuous integration

[`.github/workflows/build-ios.yml`](.github/workflows/build-ios.yml) runs the unit tests and then publishes an unsigned iOS `.ipa` as a build artifact on every push to `master`/`main` and on pull requests.
