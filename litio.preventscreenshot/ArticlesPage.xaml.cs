using Xamarin.Forms.Xaml;

namespace litio.preventscreenshot
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ArticlesPage : ScreenshootSafePage
  {
    public ArticlesPage()
    {
      InitializeComponent();
    }
  }
}