namespace EnterpriseApp.Mobile.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new EnterpriseApp.Mobile.App());
        }
    }
}