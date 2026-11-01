using Foundation;

namespace ApollonVolados.Mobile.Platforms.iOS;

/// <summary>
/// Scene delegate referenced by <c>UIApplicationSceneManifest</c> in Info.plist.
/// Apps built with the iOS 27 SDK must adopt the UIScene life cycle or they fail to launch;
/// <see cref="MauiUISceneDelegate"/> creates and manages the MAUI window for each scene.
/// </summary>
[Register("SceneDelegate")]
public class SceneDelegate : MauiUISceneDelegate
{
}
