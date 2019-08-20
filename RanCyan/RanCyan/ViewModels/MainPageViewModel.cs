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
            public string Id { get; set; }
        }

        public ReactiveCollection<MenuItem> ListView { get; set; } = new ReactiveCollection<MenuItem>();
        public AsyncReactiveCommand<MenuItem> ListTapped { get; set; } = new AsyncReactiveCommand<MenuItem>();
        public ReactiveCommand AdDisplay { get; } = new ReactiveCommand();
        public ReactiveProperty<String> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            ResetBanner();

            string genericRandomPageGet()
            {
                if (Device.RuntimePlatform == Device.UWP || Device.RuntimePlatform == Device.WPF) return "GenericRandomUWPPage";
                return "GenericRandomMainPage";
            }
            var genericRandomPage = genericRandomPageGet();
            ListView = new ReactiveCollection<MenuItem>
            {
                new MenuItem {Title="取説(外部ページへ飛びます)",Target="https://www.gunshi.info/",Image="resource://RanCyan.Images.Ranshika.png" },
                new MenuItem {Title="Project01", Target=genericRandomPage, Id="01", Image="resource://RanCyan.Images.MiniMikoRanCyan.png" },
                new MenuItem {Title="Project02", Target=genericRandomPage, Id="02", Image="resource://RanCyan.Images.MiniKowashiyaRanCyan.png" },
                new MenuItem {Title="Project03", Target=genericRandomPage, Id="03", Image="resource://RanCyan.Images.MiniMikoRanCyan.png" },
                new MenuItem {Title="Project04", Target=genericRandomPage, Id="04", Image="resource://RanCyan.Images.MiniKowashiyaRanCyan.png" },
                new MenuItem {Title="Project05", Target=genericRandomPage, Id="05", Image="resource://RanCyan.Images.MiniMikoRanCyan.png" },
                new MenuItem {Title="Project06", Target=genericRandomPage, Id="06", Image="resource://RanCyan.Images.MiniKowashiyaRanCyan.png" },
                new MenuItem {Title="Project07", Target=genericRandomPage, Id="07", Image="resource://RanCyan.Images.MiniMikoRanCyan.png" },
                new MenuItem {Title="Project08", Target=genericRandomPage, Id="08", Image="resource://RanCyan.Images.MiniKowashiyaRanCyan.png" },
                new MenuItem {Title="Project09", Target=genericRandomPage, Id="09", Image="resource://RanCyan.Images.MiniMikoRanCyan.png" },
                new MenuItem {Title="Project10", Target=genericRandomPage, Id="10", Image="resource://RanCyan.Images.MiniKowashiyaRanCyan.png" },
                //new MenuItem {Title="著作権情報",Target="RanMemoMainPage",Image="RanCyan.Images.Ranshika.png" },
            };

            ListTapped.Subscribe(async (x) =>
            {
                if (x.Target.Substring(0, 4) == "http")
                {
                    Device.OpenUri(new Uri(x.Target));
                }
                else
                {
                    ShowBanner();
                    var p = new NavigationParameters
                    {
                        {"Id", x.Id},
                    };
                    await navigationService.NavigateAsync(x.Target, p);
                }
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
