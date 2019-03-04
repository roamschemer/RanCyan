using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            //ResetBanner();

            ListView = new ReactiveCollection<MenuItem>
            {
                new MenuItem {Title="乱屍",Target="RanShikaMainPage",Image="RanCyan.Images.Ranshika.png" },
                //new MenuItem {Title="著作権情報",Target="RanMemoMainPage",Image="RanCyan.Images.Ranshika.png" },
                new MenuItem {Title="広告が見られるページ!!!!!!",Target="AdDisplayMainPage",Image="RanCyan.Images.Ranshika.png" },
            };

            //LinQで書く
            ListTapped.Subscribe(async (x) =>
            {
                await navigationService.NavigateAsync(x.Target);
            });

        }
        /// <summary>
        /// バナー広告をリセットします。
        /// </summary>
        public void ResetBanner()
        {
            //var depemdemcy = DependencyService.Get<IBannerCtrl>();
            //depemdemcy.ResetBanner();
        }
    }
}
