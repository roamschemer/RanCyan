using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RanCyan.ViewModels {
    /// <summary>各ページの設定画面</summary>
    public class LotteryConfigPageViewModel : ViewModelBase {
        public ReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveCollection<LotteryCategoryModel> LotteryCategoryModels { get; }
        public ReactiveProperty<LotteryCategoryModel> SelectedLotteryCategoryModel { get; }
        public LotteryConfigPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            var lotteryPageModel = coreModel.SelectionLotteryPageModel;
            Title = lotteryPageModel.ObserveProperty(x => x.Title).ToReactiveProperty().AddTo(this.Disposable);
            LotteryCategoryModels = lotteryPageModel.LotteryCategoryModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            SelectedLotteryCategoryModel = new ReactiveProperty<LotteryCategoryModel>(LotteryCategoryModels.First());
        }
    }
}
