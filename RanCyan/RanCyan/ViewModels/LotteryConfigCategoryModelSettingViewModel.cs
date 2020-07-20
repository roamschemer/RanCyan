using Prism.Mvvm;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace RanCyan.ViewModels {
    /// <summary>カテゴリ編集</summary>
    public class LotteryConfigCategoryModelSettingViewModel : BindableBase, IDisposable {
        public ReactiveProperty<string> Title { get; }
        public ReactiveProperty<int> NumberOfLoops { get; }
        public ReadOnlyReactiveCollection<int> NumberOfLoopsSelectList { get; }
        public ReactiveProperty<int> TotalTimeOfAllLoops { get; }
        public ReadOnlyReactiveCollection<int> TotalTimeOfAllLoopsSelectList { get; }

        public ReactiveCommand<object> Up { get; }
        public ReactiveCommand<object> Down { get; }
        public ReactiveCommand<object> Clear { get; }

        public ReadOnlyReactiveCollection<LotteryConfigLotteryModelSettingViewModel> LotteryConfigLotteryModelSettingViewModels { get; }
        public ReactiveProperty<LotteryConfigLotteryModelSettingViewModel> LotteryConfigLotteryModelSettingViewModel { get; }
        public ReactiveCommand<object> CreateSelectCommand { get; }
        public ReactiveCommand<object> ClipboardGetTextCommand { get; }
        public LotteryConfigCategoryModelSettingViewModel(LotteryPageModel lotteryPageModel, LotteryCategoryModel lotteryCategoryModel) {
            Title = lotteryCategoryModel.ToReactivePropertyAsSynchronized(x => x.Title).AddTo(this.Disposable);
            NumberOfLoops = lotteryCategoryModel.ToReactivePropertyAsSynchronized(x => x.NumberOfLoops).AddTo(this.Disposable);
            NumberOfLoopsSelectList = lotteryCategoryModel.NumberOfLoopsSelectList.ToReadOnlyReactiveCollection(x => x).AddTo(this.Disposable);
            TotalTimeOfAllLoops = lotteryCategoryModel.ToReactivePropertyAsSynchronized(x => x.TotalTimeOfAllLoops).AddTo(this.Disposable);
            TotalTimeOfAllLoopsSelectList = lotteryCategoryModel.TotalTimeOfAllLoopsSelectList.ToReadOnlyReactiveCollection(x => x).AddTo(this.Disposable);
            Up = new ReactiveCommand<object>().WithSubscribe(_ => lotteryPageModel.Up(lotteryCategoryModel));
            Down = new ReactiveCommand<object>().WithSubscribe(_ => lotteryPageModel.Down(lotteryCategoryModel));
            Clear = new ReactiveCommand<object>().WithSubscribe(_ => lotteryPageModel.Clear(lotteryCategoryModel));
            //抽選モデルの設定
            LotteryConfigLotteryModelSettingViewModels = lotteryCategoryModel.LotteryModels.ToReadOnlyReactiveCollection(x => new LotteryConfigLotteryModelSettingViewModel(lotteryCategoryModel, x)).AddTo(this.Disposable);
            LotteryConfigLotteryModelSettingViewModel = new ReactiveProperty<LotteryConfigLotteryModelSettingViewModel>(LotteryConfigLotteryModelSettingViewModels.First());
            CreateSelectCommand = new ReactiveCommand<object>().WithSubscribe(_ => lotteryCategoryModel.Create());
            ClipboardGetTextCommand = new ReactiveCommand<object>().WithSubscribe(async _ => {
                var select = await Application.Current.MainPage.DisplayAlert("クリップボードから貼り付け", "クリップボードにある項目を一括追加しますか？", "いいよ", "待った");
                if (select) lotteryCategoryModel.ClipboardGet();
            }).AddTo(this.Disposable);
        }
        //後始末
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public void Dispose() => this.Disposable.Dispose();
    }
}
