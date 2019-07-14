using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using RanCyan.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace RanCyan.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public class MenuItem
        {
            public string Title { get; set; }
            public string Target { get; set; }
            public string Image { get; set; }
        }

        public ReactiveCollection<MenuItem> ListView { get; set; } = new ReactiveCollection<MenuItem>();
        public AsyncReactiveCommand<MenuItem> ListTapped { get; set; } = new AsyncReactiveCommand<MenuItem>();
        public ReactiveCommand AdDisplay { get; } = new ReactiveCommand();
        public ReactiveProperty<String> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            ResetBanner();

            string ranShikaPageGet()
            {
                if (Device.RuntimePlatform == Device.UWP || Device.RuntimePlatform == Device.WPF) return "RanShikaUWPPage";
                return "RanShikaMainPage";
            }
            var ranShikaPage = ranShikaPageGet();
            string genericRandomPageGet()
            {
                if (Device.RuntimePlatform == Device.UWP || Device.RuntimePlatform == Device.WPF) return "GenericRandomUWPPage";
                return "GenericRandomMainPage";
            }
            var genericRandomPage = genericRandomPageGet();
            ListView = new ReactiveCollection<MenuItem>
            {
                new MenuItem {Title="乱屍",Target=ranShikaPage,Image="RanCyan.Images.Ranshika.png" },
                new MenuItem {Title="汎用",Target=genericRandomPage,Image="RanCyan.Images.Ranshika.png" },
                //new MenuItem {Title="著作権情報",Target="RanMemoMainPage",Image="RanCyan.Images.Ranshika.png" },
            };

            ListTapped.Subscribe(async (x) =>
            {
                ShowBanner();
                await navigationService.NavigateAsync(x.Target);
            });

        }


        /// <summary>
        /// バナー広告をリセットします。
        /// </summary>
        private void ResetBanner()
        {
            var depemdemcy = DependencyService.Get<IBannerCtrl>();
            depemdemcy.ResetBanner();
        }
        /// <summary>
        /// バナー広告を表示します。
        /// </summary>
        public void ShowBanner()
        {
            var depemdemcy = DependencyService.Get<IBannerCtrl>();
            depemdemcy.ShowBanner();
        }
    }
}
