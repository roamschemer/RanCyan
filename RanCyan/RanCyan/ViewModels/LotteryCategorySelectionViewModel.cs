using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace RanCyan.ViewModels {
    /// <summary>
    /// 各カテゴリ選択用ColectionViewの中身
    /// </summary>
    public class LotteryCategorySelectionViewModel : IDisposable {
        public ReadOnlyReactiveCollection<LotteryModel> LotteryModels { get; }
        public ReactiveCommand<LotteryModel> ItemTapped { get; }
        public LotteryCategorySelectionViewModel(LotteryCategoryModel lotteryCategoryModel) {
            LotteryModels = lotteryCategoryModel.LotteryModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            ItemTapped = new ReactiveCommand<LotteryModel>().WithSubscribe(x => x.SelectionState());
        }

        //後始末
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public void Dispose() => this.Disposable.Dispose();
    }
}
