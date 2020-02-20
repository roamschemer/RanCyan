using Prism.Navigation;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RanCyan.ViewModels {
    public class MainPageViewModel : ViewModelBase {

        public ReadOnlyReactiveCollection<MenuModel> MenuModels { get; }
        public AsyncReactiveCommand<MenuModel> Command { get; }
        public AsyncReactiveCommand<object> CreateCommand { get; }

        public MainPageViewModel(INavigationService navigationService, MainPageModel mainPageModel, CoreModel coreModel) : base(navigationService) {
            mainPageModel.CoreModelInjection(coreModel);
            MenuModels = mainPageModel.MenuModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            Command = new AsyncReactiveCommand<MenuModel>().WithSubscribe(async (x) => {
                if (x.IsWebPage) {
                    //Device.OpenUri(new Uri(x.ViewAddress)); //←今は使われていないらしい
                    await Browser.OpenAsync(x.ViewAddress, BrowserLaunchMode.SystemPreferred);
                    return;
                }
                if (x.LotteryPageIndex != null) {
                    coreModel.SelectModel((int)x.LotteryPageIndex);
                }
                await navigationService.NavigateAsync(x.ViewAddress);
            });
            CreateCommand = new AsyncReactiveCommand().WithSubscribe(async _ => {
                var select = await Application.Current.MainPage.DisplayAlert("新規追加", "新規追加しますか？", "いいよ", "待った");
                if (select) { 
                    mainPageModel.LastAddLotteryPageMode(coreModel); 
                }
            });
        }

        //以下旧コード

        //public class MenuItem {
        //    public string Title { get; set; }
        //    public string Target { get; set; }
        //    public string Image { get; set; }
        //    public string Id { get; set; }
        //}

        //public ReactiveCollection<MenuItem> ListView { get; set; } = new ReactiveCollection<MenuItem>();
        //public AsyncReactiveCommand<MenuItem> ListTapped { get; set; } = new AsyncReactiveCommand<MenuItem>();
        //public ReactiveCommand AdDisplay { get; } = new ReactiveCommand();
        //public ReactiveProperty<String> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");

        //public MainPageViewModel(INavigationService navigationService) : base(navigationService) {
        //    ResetBanner();

        //    PageListSet();

        //    ListTapped.Subscribe(async (x) => {
        //        if (x.Target.Substring(0, 4) == "http") {
        //            Device.OpenUri(new Uri(x.Target));
        //        } else {
        //            ShowBanner();
        //            var p = new NavigationParameters
        //            {
        //                {"Id", x.Id},
        //            };
        //            await navigationService.NavigateAsync(x.Target, p);
        //        }
        //    });

        //}

        //private void PageListSet() {
        //    string genericRandomPageGet() {
        //        if (Device.RuntimePlatform == Device.UWP || Device.RuntimePlatform == Device.WPF) return "GenericRandomUWPPage";
        //        return "GenericRandomMainPage";
        //    }
        //    var genericRandomPage = genericRandomPageGet();

        //    var pageInfomations = new List<PageInfomation>();
        //    foreach (var i in Enumerable.Range(1, 10)) {
        //        pageInfomations.Add(new PageInfomation(i));
        //    }

        //    ListView = new ReactiveCollection<MenuItem>
        //    {
        //        new MenuItem {Title="取説(外部ページへ飛びます)",Target="https://www.gunshi.info/rancyanproject",Image="resource://RanCyan.Images.Ranshika.png" },
        //        new MenuItem {Title=pageInfomations[0].PageTitle, Target=genericRandomPage, Id="01", Image="resource://RanCyan.Images.MiniMikoRanCyan.png" },
        //        new MenuItem {Title=pageInfomations[1].PageTitle, Target=genericRandomPage, Id="02", Image="resource://RanCyan.Images.MiniKowashiyaRanCyan.png" },
        //        new MenuItem {Title=pageInfomations[2].PageTitle, Target=genericRandomPage, Id="03", Image="resource://RanCyan.Images.MiniMikoRanCyan.png" },
        //        new MenuItem {Title=pageInfomations[3].PageTitle, Target=genericRandomPage, Id="04", Image="resource://RanCyan.Images.MiniKowashiyaRanCyan.png" },
        //        new MenuItem {Title=pageInfomations[4].PageTitle, Target=genericRandomPage, Id="05", Image="resource://RanCyan.Images.MiniMikoRanCyan.png" },
        //        new MenuItem {Title=pageInfomations[5].PageTitle, Target=genericRandomPage, Id="06", Image="resource://RanCyan.Images.MiniKowashiyaRanCyan.png" },
        //        new MenuItem {Title=pageInfomations[6].PageTitle, Target=genericRandomPage, Id="07", Image="resource://RanCyan.Images.MiniMikoRanCyan.png" },
        //        new MenuItem {Title=pageInfomations[7].PageTitle, Target=genericRandomPage, Id="08", Image="resource://RanCyan.Images.MiniKowashiyaRanCyan.png" },
        //        new MenuItem {Title=pageInfomations[8].PageTitle, Target=genericRandomPage, Id="09", Image="resource://RanCyan.Images.MiniMikoRanCyan.png" },
        //        new MenuItem {Title=pageInfomations[9].PageTitle, Target=genericRandomPage, Id="10", Image="resource://RanCyan.Images.MiniKowashiyaRanCyan.png" },
        //        //new MenuItem {Title="著作権情報",Target="RanMemoMainPage",Image="RanCyan.Images.Ranshika.png" },
        //    };
        //}

        ///// <summary>
        ///// 遷移してきて最初に通るとこ
        ///// </summary>
        ///// <param name="parameters"></param>
        //public override void OnNavigatedTo(INavigationParameters parameters) {
        //    PageListSet();
        //}

        ///// <summary>
        ///// バナー広告をリセットします。
        ///// </summary>
        //private void ResetBanner() {
        //    var depemdemcy = DependencyService.Get<IBannerCtrl>();
        //    depemdemcy.ResetBanner();
        //}
        ///// <summary>
        ///// バナー広告を表示します。
        ///// </summary>
        //public void ShowBanner() {
        //    var depemdemcy = DependencyService.Get<IBannerCtrl>();
        //    depemdemcy.ShowBanner();
        //}
    }
}
