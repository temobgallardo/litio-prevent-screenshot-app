using System;
using Xamarin.Forms;

namespace litio.preventscreenshot
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
    }

    private void GoToScreenshotSafePage(object sender, EventArgs e)
    {
      this.Navigation.PushAsync(new ArticlesPage());
    }
  }
}