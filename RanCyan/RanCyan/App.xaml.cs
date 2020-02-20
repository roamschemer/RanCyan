using Prism;
using Prism.Ioc;
using Prism.Mvvm;
using RanCyan.Models;
using RanCyan.ViewModels;
using RanCyan.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RanCyan
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<CoreModel>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<GenericRandomMainPage, GenericRandomPageViewModel>();
            containerRegistry.RegisterForNavigation<GenericRandomUWPPage, GenericRandomPageViewModel>();
            containerRegistry.RegisterForNavigation<LotteryUwpPage, LotteryUwpPageViewModel>();
        }

    }
}
