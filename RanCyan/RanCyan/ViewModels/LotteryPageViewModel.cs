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
    /// <summary>(ViewModel)各抽選用ページ</summary>
    public class LotteryPageViewModel : ViewModelBase {
        public ReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveCollection<LotteryCategorySelectionViewModel> LotteryCategorySelectionViewModels { get; }
        public ReadOnlyReactiveCollection<LotteryCategoryToDrawViewModel> LotteryCategoryToDrawViewModels { get; }
        public ReactiveProperty<int> ContentPageWidth { get; }
        public ReactiveProperty<int> SelectionViewWidth { get; }
        public ReactiveProperty<int> ToDrawButtonWidth { get; }
        public LotteryPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            var lotteryPageModel = coreModel.LotteryPageModel;
            Title = lotteryPageModel.ObserveProperty(x => x.Title).ToReactiveProperty().AddTo(this.Disposable);
            LotteryCategorySelectionViewModels = lotteryPageModel.CategoryModels.ToReadOnlyReactiveCollection(x => new LotteryCategorySelectionViewModel(x));
            LotteryCategoryToDrawViewModels = lotteryPageModel.CategoryModels.ToReadOnlyReactiveCollection(x => new LotteryCategoryToDrawViewModel(x));
            SelectionViewWidth = lotteryPageModel.ObserveProperty(x => x.CategoryModelsCount).Select(x => x * 100).ToReactiveProperty().AddTo(this.Disposable);
            //ToDrawButtonWidth = lotteryPageModel.ObserveProperty(x => x.CategoryModelsCount).Select(x => x / 400).ToReactiveProperty().AddTo(this.Disposable);
        }
    }
}
