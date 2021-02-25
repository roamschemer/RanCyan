using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

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
        /// <summary>ラベルの情報</summary>
        public ReactiveProperty<LotteryLabelModel> LotteryLabelModel { get; }
        /// <summary>複数抽選ラベルの情報</summary>
        public ReadOnlyReactiveCollection<LotteryLabelModel> LotteryLabelModels { get; }
        /// <summary>ショートカットキー</summary>
        public ReactiveProperty<string> AccessKey { get; }
        /// <summary>抽選数</summary>
        public ReactiveProperty<int> LotteryNumber { get; }
        /// <summary>抽選数picker選択用のコレクション</summary>
        public ObservableCollection<int> LotteryNumberList { get; set; }


        public LotteryCategorySelectionViewModel(CoreModel coreModel, LotteryCategoryModel lotteryCategoryModel) {
            //選択リスト
            LotteryModels = lotteryCategoryModel.LotteryModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            SelectionCommand = new ReactiveCommand<LotteryModel>().WithSubscribe(x => {
                x.SelectionState();
                coreModel.SaveLotteryPage();
            }).AddTo(this.Disposable);
            //抽選ボタン
            CategoryTitle = lotteryCategoryModel.ObserveProperty(x => x.Title).ToReactiveProperty().AddTo(this.Disposable);
            ToDrawCommand = new ReactiveCommand();
            ToDrawCommand.Where(_ => !lotteryCategoryModel.InLottery).Subscribe(async _ => {
                coreModel.SelectionLotteryPageModel.IsAllToDraw = false;
                coreModel.RanCyanModel.LotteryRancyanImageAsync();
                await lotteryCategoryModel.ToDrawAsync(coreModel.SelectionLotteryPageModel, LotteryNumber.Value);
            }).AddTo(this.Disposable);
            //ラベル情報
            LotteryLabelModel = lotteryCategoryModel.ObserveProperty(x => x.LotteryLabelModel).ToReactiveProperty().AddTo(this.Disposable);
            LotteryLabelModels = lotteryCategoryModel.LotteryLabelModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            //抽選数
            LotteryNumber = new ReactiveProperty<int>(1);
            LotteryNumberList = new ObservableCollection<int>(Enumerable.Range(1, LotteryModels.Count));
            LotteryModels.CollectionChangedAsObservable().Subscribe(_ => {
                LotteryNumberList.Clear();
                foreach (var i in Enumerable.Range(1, LotteryModels.Count)) {
                    LotteryNumberList.Add(i);
                }
                LotteryNumber.Value = 1;
            });
            //AccessKey
            var accessKey = coreModel.SelectionLotteryPageModel.LotteryCategoryModels.Select((model, index) => (model, index)).First(x => x.model == lotteryCategoryModel);
            var keyItems = new[] { "Z", "X", "C", "V", "B", "N", "M", ".", "/", "\\" };
            AccessKey = new ReactiveProperty<string>(accessKey.index < keyItems.Count() ? keyItems[accessKey.index] : string.Empty);
        }
        //後始末
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public void Dispose() => this.Disposable.Dispose();
    }
}
