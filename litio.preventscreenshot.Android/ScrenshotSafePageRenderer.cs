using Android.Content;
using Android.Views;
using AndroidX.Core.View;
using Xamarin.Forms.Platform.Android;

namespace litio.preventscreenshot.Droid
{
  public class ScrenshotSafePageRenderer : PageRenderer, IScreenshotBlockerService
  {
    public ScrenshotSafePageRenderer(Context context) : base(context)
    {
    }

    /// <summary>
    /// This will add or remoe the FLAG_SECURE flag which protects the view from been papparatzied.
    /// </summary>
    /// <param name="apply">adds/removes FLAG_SECURE from Activity this Fragment is attached to/from</param>
    public void BlockScreenshoot(bool apply)
    {
      var window = this.Context.GetActivity().Window;
      var windowManger = this.Context.GetActivity().WindowManager;

      // is change needed?
      var flags = window.Attributes.Flags;
      if (apply && (flags & WindowManagerFlags.Secure) != 0)
      {
        // already set, change is not needed.
        return;
      }
      else if (!apply && (flags & WindowManagerFlags.Secure) == 0)
      {
        // already cleared, change is not needed.
        return;
      }

      // apply (or clear) the FLAG_SECURE flag to/from Activity this Fragment is attached to.
      bool flagsChanged;
      if (apply)
      {
        window.AddFlags(WindowManagerFlags.Secure);
        flagsChanged = true;
      }
      else
      {
        window.ClearFlags(WindowManagerFlags.Secure);
        flagsChanged = true;
      }

      // Re-apply (re-draw) Window's DecorView so the change to the Window flags will be in place immediately.
      if (flagsChanged && ViewCompat.IsAttachedToWindow(window.DecorView))
      {
        // FIXME Removing the View and attaching it back makes visible re-draw on Android 4.x, 5+ is good.
        windowManger.RemoveViewImmediate(window.DecorView);
        windowManger.AddView(window.DecorView, window.Attributes);
      }
    }

    protected override void OnAttachedToWindow()
    {
      base.OnAttachedToWindow();

      this.BlockScreenshoot(true);
    }

    protected override void OnDetachedFromWindow()
    {
      base.OnDetachedFromWindow();

      this.BlockScreenshoot(false);
    }
  }
}