using System;
using Android.App;
using Android.Runtime;
using Android.Views;
using AndroidX.Core.View;
using Xamarin.Forms.Platform.Android;

namespace litio.preventscreenshot.Droid
{
  [Activity(Label = "ScreenshotSafeActivity")]
  public class ScreenshotSafeFragment : AndroidX.Fragment.App.Fragment, IScreenshotBlockerService
  {
    protected ScreenshotSafeFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
    }

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
        if (this.Context.GetActivity().GetType() != typeof(ScreenshotSafeFragment))
        {
          return;
        }

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
  }
}