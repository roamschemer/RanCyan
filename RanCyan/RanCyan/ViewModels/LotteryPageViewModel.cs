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
        private readonly CoreModel coreModel;
        public ReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveCollection<LotteryCategorySelectionViewModel> LotteryCategorySelectionViewModels { get; }
        public ReactiveProperty<int> SelectionViewWidth { get; }
        public ReactiveProperty<string> RanCyanImage { get; }
        public ReactiveProperty<string> LotteryLabel { get; }
        public LotteryPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            this.coreModel = coreModel;
            RanCyanImage = coreModel.ObserveProperty(x => x.RanCyanImage).ToReactiveProperty().AddTo(this.Disposable);
            var lotteryPageModel = coreModel.SelectionLotteryPageModel;
            Title = lotteryPageModel.ObserveProperty(x => x.Title).ToReactiveProperty().AddTo(this.Disposable);
            LotteryCategorySelectionViewModels = lotteryPageModel.LotteryCategoryModels.ToReadOnlyReactiveCollection(x => new LotteryCategorySelectionViewModel(coreModel, x)).AddTo(this.Disposable);
            SelectionViewWidth = lotteryPageModel.ObserveProperty(x => x.CategoryModelsCount).Select(x => x * 100).ToReactiveProperty().AddTo(this.Disposable);
            LotteryLabel = lotteryPageModel.ObserveProperty(x => x.ViewLotteryModel).Where(x => x != null).Select(x => x.Name).ToReactiveProperty().AddTo(this.Disposable);
        }
        public override void OnNavigatedTo(INavigationParameters parameters) {
            //何故か遷移後にgifを選択しないと動かない。Xamarinのバージョンを上げるとこうなった。
            coreModel.WaitingRancyanImage();
        }
    }
}
