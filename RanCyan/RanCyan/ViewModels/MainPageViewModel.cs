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

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            ResetBanner();

            string ranShikaPage = RanShikaPageGet();
            ListView = new ReactiveCollection<MenuItem>
            {
                new MenuItem {Title="乱屍",Target=ranShikaPage,Image="RanCyan.Images.Ranshika.png" },
                new MenuItem {Title="サイコロ",Target="DiceMainPage",Image="RanCyan.Images.Ranshika.png" },
                //new MenuItem {Title="説明",Target="InfomationMainPage",Image="RanCyan.Images.Ranshika.png" },
                //new MenuItem {Title="著作権情報",Target="RanMemoMainPage",Image="RanCyan.Images.Ranshika.png" },
                //new MenuItem {Title="広告が見られるページ!!!!!!",Target="AdDisplayMainPage",Image="RanCyan.Images.Ranshika.png" },
            };

            //LinQで書く
            ListTapped.Subscribe(async (x) =>
            {
                ShowBanner();
                await navigationService.NavigateAsync(x.Target);
            });

        }

        /// <summary>
        /// 乱屍の遷移先取得
        /// </summary>
        /// <returns>遷移先View</returns>
        private string RanShikaPageGet()
        {
            if (Device.RuntimePlatform == Device.Android) return "RanShikaMainPage";
            if (Device.RuntimePlatform == Device.iOS) return "RanShikaMainPage";
            if (Device.RuntimePlatform == Device.UWP) return "RanShikaUWPPage";
            if (Device.RuntimePlatform == Device.WPF) return "RanShikaUWPPage";
            return "";
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
