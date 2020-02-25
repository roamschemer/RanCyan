using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Reactive.Disposables;

namespace RanCyan.ViewModels {
    /// <summary>(ContentView/ViewModel)各抽選カテゴリ抽選実施ボタンリスト</summary>
    public class LotteryCategoryToDrawViewModel : IDisposable {
        /// <summary>抽選実施コマンド</summary>
        public ReactiveCommand<object> Command { get; }
        /// <summary>ボタン名称</summary>
        public ReactiveProperty<string> Title { get; }
        public LotteryCategoryToDrawViewModel(LotteryCategoryModel lotteryCategoryModel) {
            Title = lotteryCategoryModel.ObserveProperty(x => x.Title).ToReactiveProperty().AddTo(this.Disposable);
            Command = new ReactiveCommand().WithSubscribe(_ => lotteryCategoryModel.ToDrawAsync());
        }
        //後始末
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public void Dispose() => this.Disposable.Dispose();
    }
}
