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

        public ReadOnlyReactiveCollection<MenuModel> MenuModels { get; }
        public AsyncReactiveCommand<MenuModel> Command { get; }
        public AsyncReactiveCommand<object> CreateCommand { get; }

        public MainPageViewModel(INavigationService navigationService, MainPageModel mainPageModel, CoreModel coreModel) : base(navigationService) {
            mainPageModel.CoreModelInjection(coreModel);
            MenuModels = mainPageModel.MenuModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            Command = new AsyncReactiveCommand<MenuModel>().WithSubscribe(async (x) => {
                if (x.IsWebPage) {
                    await Browser.OpenAsync(x.ViewAddress, BrowserLaunchMode.SystemPreferred);
                    return;
                }
                if (x.LotteryPageIndex != null) coreModel.SelectionModel(coreModel.LotteryPageModels[(int)x.LotteryPageIndex]);
                await navigationService.NavigateAsync(x.ViewAddress);
            }).AddTo(this.Disposable);
            CreateCommand = new AsyncReactiveCommand().WithSubscribe(async _ => {
                var select = await Application.Current.MainPage.DisplayAlert("新規追加", "新規追加しますか？", "いいよ", "待った");
                if (select) mainPageModel.LastAddLotteryPageMode(coreModel);
            }).AddTo(this.Disposable);
        }
    }
}
