using Prism.Mvvm;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace RanCyan.ViewModels {
    /// <summary>抽選項目編集</summary>
    public class LotteryConfigLotteryModelSettingViewModel : BindableBase, IDisposable {

        public ReactiveProperty<string> Name { get; }
        public ReactiveProperty<int> Ratio { get; }
        public ReadOnlyReactiveCollection<int> RatioItems { get; }
        public ReactiveCommand<object> Up { get; }
        public ReactiveCommand<object> Down { get; }
        public ReactiveCommand<object> Clear { get; }

        public LotteryConfigLotteryModelSettingViewModel(LotteryCategoryModel lotteryCategoryModel, LotteryModel lotteryModel) {
            Name = lotteryModel.ToReactivePropertyAsSynchronized(x => x.Name).AddTo(this.Disposable);
            Ratio = lotteryModel.ToReactivePropertyAsSynchronized(x => x.Ratio).AddTo(this.Disposable);
            RatioItems = lotteryModel.RatioItems.ToReadOnlyReactiveCollection(x => x).AddTo(this.Disposable);
            Up = new ReactiveCommand<object>().WithSubscribe(_ => lotteryCategoryModel.Up(lotteryModel));
            Down = new ReactiveCommand<object>().WithSubscribe(_ => lotteryCategoryModel.Down(lotteryModel));
            Clear = new ReactiveCommand<object>().WithSubscribe(_ => lotteryCategoryModel.Clear(lotteryModel);
        }
        //後始末
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public void Dispose() => this.Disposable.Dispose();
    }
}
