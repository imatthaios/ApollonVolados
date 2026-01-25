using Foundation;
using UIKit;

namespace ApollonVolados.Mobile.Platforms.iOS;

[Register("SceneDelegate")]
public class SceneDelegate : UIResponder, IUIWindowSceneDelegate
{
    public UIWindow Window { get; set; }
}