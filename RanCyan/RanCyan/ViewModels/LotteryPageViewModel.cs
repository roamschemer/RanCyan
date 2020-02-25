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
    public class LotteryPageViewModel : ViewModelBase {
        public ReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveCollection<LotteryCategorySelectionViewModel> LotteryCategorySelectionViewModels { get; }
        public LotteryPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            var lotteryPageModel = coreModel.LotteryPageModel;
            Title = lotteryPageModel.ObserveProperty(x => x.Title).ToReactiveProperty().AddTo(this.Disposable);
            LotteryCategorySelectionViewModels = lotteryPageModel.CategoryModels.ToReadOnlyReactiveCollection(x => new LotteryCategorySelectionViewModel(x));
        }
    }
}
