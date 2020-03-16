using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace RanCyan.ViewModels {
    /// <summary>カテゴリ編集</summary>
    public class LotteryConfigLotteryModelSettingViewModel : IDisposable {

        public ReactiveProperty<string> Name { get; }
        public ReactiveProperty<int> Ratio { get; }
        public ReadOnlyReactiveCollection<int> RatioItems { get; }

        public LotteryConfigLotteryModelSettingViewModel(LotteryModel lotteryModel) {
            Name = lotteryModel.ToReactivePropertyAsSynchronized(x => x.Name).AddTo(this.Disposable);
            Ratio = lotteryModel.ToReactivePropertyAsSynchronized(x => x.Ratio).AddTo(this.Disposable);
            RatioItems = lotteryModel.RatioItems.ToReadOnlyReactiveCollection(x => x).AddTo(this.Disposable);
        }
        //後始末
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public void Dispose() => this.Disposable.Dispose();
    }
}
