using Prism.Navigation;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RanCyan.ViewModels {
    /// <summary>(ViewModel)起動直後に展開するメインページ</summary>
    public class MainPageViewModel : ViewModelBase {
        public ReadOnlyReactiveCollection<LotteryPageModel> LotteryPageModels { get; }
        public AsyncReactiveCommand<LotteryPageModel> Command { get; }
        public AsyncReactiveCommand<object> CreateCommand { get; }
        public MainPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            LotteryPageModels = coreModel.LotteryPageModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            Command = new AsyncReactiveCommand<LotteryPageModel>().WithSubscribe(async (x) => {
                if (x.MenuModel.PageType == MenuModel.PageTypeEnum.Web) {
                    await Browser.OpenAsync(x.MenuModel.ViewAddress, BrowserLaunchMode.SystemPreferred);
                    return;
                }
                coreModel.SelectModel(x);
                await navigationService.NavigateAsync(x.MenuModel.ViewAddress);
            }).AddTo(this.Disposable);
            CreateCommand = new AsyncReactiveCommand().WithSubscribe(async _ => {
                var select = await Application.Current.MainPage.DisplayAlert("新規追加", "新規追加しますか？", "いいよ", "待った");
                if (select) coreModel.CleateNewLotteryPageModel();
            }).AddTo(this.Disposable);
        }
    }
}
