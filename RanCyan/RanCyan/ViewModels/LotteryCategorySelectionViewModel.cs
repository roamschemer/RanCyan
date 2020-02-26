using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

namespace RanCyan.ViewModels {
    /// <summary>(ContentView/ViewModel)各抽選カテゴリ除外選択リスト</summary>
    public class LotteryCategorySelectionViewModel : IDisposable {
        /// <summary>抽選モデルのコレクション</summary>
        public ReadOnlyReactiveCollection<LotteryModel> LotteryModels { get; }
        /// <summary>抽選リストからの一時除外コマンド</summary>
        public ReactiveCommand<LotteryModel> SelectionCommand { get; }
        /// <summary>抽選実施コマンド</summary>
        public ReactiveCommand<object> ToDrawCommand { get; }
        /// <summary>ボタン名称</summary>
        public ReactiveProperty<string> CategoryTitle { get; }
        public LotteryCategorySelectionViewModel(CoreModel coreModel, LotteryCategoryModel lotteryCategoryModel) {
            //選択リスト
            LotteryModels = lotteryCategoryModel.LotteryModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            SelectionCommand = new ReactiveCommand<LotteryModel>().WithSubscribe(x => x.SelectionState()).AddTo(this.Disposable);
            //抽選ボタン
            CategoryTitle = lotteryCategoryModel.ObserveProperty(x => x.Title).ToReactiveProperty().AddTo(this.Disposable);
            ToDrawCommand = new ReactiveCommand();
            ToDrawCommand.Where(_ => !lotteryCategoryModel.InLottery).Subscribe(_ => {
                //coreModel.SelectionLotteryPageModel.SelectModel(lotteryCategoryModel);
                coreModel.LotteryRancyanImageAsync();
                lotteryCategoryModel.ToDrawAsync(coreModel.SelectionLotteryPageModel);
            }).AddTo(this.Disposable);
        }
        //後始末
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public void Dispose() => this.Disposable.Dispose();
    }
}
