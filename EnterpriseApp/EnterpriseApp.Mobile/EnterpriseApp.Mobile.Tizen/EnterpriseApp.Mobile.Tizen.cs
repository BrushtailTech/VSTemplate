using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;

namespace EnterpriseApp.Mobile.Tizen
{
    internal class Program : FormsApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            // Call 'LoadApplication(Application application)' here to load your application.
            LoadApplication(new App());
        }

        static void Main(string[] args)
        {
            var app = new Program();
            Forms.Init(app);
            app.Run(args);
        }
    }
}