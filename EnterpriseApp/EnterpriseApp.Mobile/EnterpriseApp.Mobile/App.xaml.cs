using EnterpriseApp.Core;
using EnterpriseApp.Mobile.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static EnterpriseApp.Mobile.ApplicationDbContextFactory;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace EnterpriseApp.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DatabaseInit();

            Context.TodoItems.Add(new TodoItem { Title = "Item 1" });
            Context.TodoItems.Add(new TodoItem { Title = "Item 2" });
            Context.TodoItems.Add(new TodoItem { Title = "Item 3" });
            Context.TodoItems.Add(new TodoItem { Title = "Item 4" });
            Context.TodoItems.Add(new TodoItem { Title = "Item 5" });

            Context.SaveChanges();

            MainPage = new MainPage();
        }

        // Handle when your app starts
        protected override void OnStart()
        {
            // AppCenter.Start("replace with app center secret", typeof(Analytics), typeof(Crashes));
        }

        // Handle when your app sleeps
        protected override void OnSleep() { }

        // Handle when your app resumes
        protected override void OnResume() { }
    }
}