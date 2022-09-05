using Android.Content;
using Android.Views;
using AndroidX.Core.View;
using litio.preventscreenshot;
using litio.preventscreenshot.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(ScreenshootSafePage), typeof(ScreenshotSafePageRenderer))]
namespace litio.preventscreenshot.Droid
{
  public class ScreenshotSafePageRenderer : PageRenderer, IScreenshotBlockerService
  {
    public ScreenshotSafePageRenderer(Context context) : base(context)
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
      if (apply)
      {
        window.AddFlags(WindowManagerFlags.Secure);
      }
      else
      {
        window.ClearFlags(WindowManagerFlags.Secure);
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