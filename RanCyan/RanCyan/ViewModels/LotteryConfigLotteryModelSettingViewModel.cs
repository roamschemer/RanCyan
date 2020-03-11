using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace RanCyan.ViewModels {
    /// <summary>カテゴリ編集</summary>
    public class LotteryConfigLotteryModelSettingViewModel : IDisposable {
        public LotteryConfigLotteryModelSettingViewModel(LotteryModel lotteryModel) {

        }
        //後始末
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public void Dispose() => this.Disposable.Dispose();
    }
}
