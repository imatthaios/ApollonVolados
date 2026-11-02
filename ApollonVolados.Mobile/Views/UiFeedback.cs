namespace ApollonVolados.Mobile.Views;

/// <summary>
/// Platform-appropriate tap feedback (docs/DESIGN.md §4): a light selection haptic on iOS;
/// Android relies on the ripple and needs no extra VIBRATE permission.
/// </summary>
internal static class UiFeedback
{
    public static void Selection()
    {
#if IOS
        try
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }
        catch (FeatureNotSupportedException)
        {
            // Simulator / older hardware: no haptics, nothing to do.
        }
#endif
    }
}
