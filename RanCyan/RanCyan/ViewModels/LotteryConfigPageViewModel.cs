using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace RanCyan.ViewModels {
    /// <summary>設定画面</summary>
    public class LotteryConfigPageViewModel : ViewModelBase {
        public ReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveCollection<LotteryCategoryModel> LotteryCategoryModels { get; }
        public ReactiveProperty<LotteryCategoryModel> SelectedLotteryCategoryModel { get; }
        public ReadOnlyReactiveCollection<string> ImageBackColorList { get; }
        public ReactiveProperty<string> ImageBackColor { get; }
        public ReadOnlyReactiveCollection<int> SelectionViewWidthList { get; }
        public ReactiveProperty<int> SelectionViewWidth { get; }
        public LotteryConfigPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            var lotteryPageModel = coreModel.SelectionLotteryPageModel;
            Title = lotteryPageModel.ObserveProperty(x => x.Title).Select(x => $"Config [{x}]").ToReactiveProperty().AddTo(this.Disposable);
            LotteryCategoryModels = lotteryPageModel.LotteryCategoryModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            SelectedLotteryCategoryModel = new ReactiveProperty<LotteryCategoryModel>(LotteryCategoryModels.First());
            ImageBackColorList = coreModel.ImageBackColorList.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            ImageBackColor = coreModel.ToReactivePropertyAsSynchronized(x => x.BackColor).AddTo(this.Disposable);
            SelectionViewWidthList = coreModel.SelectionViewWidthList.ToReadOnlyReactiveCollection(x => x).AddTo(this.Disposable);
            SelectionViewWidth = coreModel.ToReactivePropertyAsSynchronized(x=>x.SelectionViewWidth).AddTo(this.Disposable);
        }
    }
}
