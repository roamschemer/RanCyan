using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace RanCyan.ViewModels {
    /// <summary>(ContentView/ViewModel)各抽選カテゴリ除外選択リスト</summary>
    public class LotteryCategorySelectionViewModel : IDisposable {
        /// <summary>抽選モデルのコレクション</summary>
        public ReadOnlyReactiveCollection<LotteryModel> LotteryModels { get; }
        /// <summary>抽選リストからの一時除外コマンド</summary>
        public ReactiveCommand<LotteryModel> Command { get; }
        public LotteryCategorySelectionViewModel(LotteryCategoryModel lotteryCategoryModel) {
            LotteryModels = lotteryCategoryModel.LotteryModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            Command = new ReactiveCommand<LotteryModel>().WithSubscribe(x => x.SelectionState());
        }
        //後始末
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public void Dispose() => this.Disposable.Dispose();
    }
}
