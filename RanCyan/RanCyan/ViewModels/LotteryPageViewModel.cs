using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using RanCyan.Models;
using RanCyan.Views;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace RanCyan.ViewModels {
    /// <summary>(ViewModel)各抽選用ページ</summary>
    public class LotteryPageViewModel : ViewModelBase {
        private readonly CoreModel coreModel;
        public ReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveCollection<LotteryCategorySelectionViewModel> LotteryCategorySelectionViewModels { get; }
        public ReactiveProperty<int> SelectionViewWidth { get; }
        public ReactiveProperty<string> RanCyanImage { get; }
        public ReactiveProperty<bool> IsImageActive { get; }
        public ReactiveProperty<string> LotteryLabel { get; }
        public ReactiveProperty<string> LotteryLabelColor { get; }
        public ReactiveProperty<bool> LotteryLabelVisible { get; }
        public AsyncReactiveCommand<object> ConfigPageNavigationCommand { get; }

        public LotteryPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            this.coreModel = coreModel;
            coreModel.StartRancyanImage();
            RanCyanImage = coreModel.ObserveProperty(x => x.RanCyanImage).ToReactiveProperty().AddTo(this.Disposable);
            IsImageActive = coreModel.ObserveProperty(x => x.IsImageActive).ToReactiveProperty().AddTo(this.Disposable);
            var imageTimer = new ReactiveTimer(TimeSpan.FromSeconds(10));
            imageTimer.Subscribe(_ => {
                coreModel.WaitingRancyanImage();
                coreModel.IsImageActive = true; 
                imageTimer.Stop(); 
            });
            imageTimer.Start(TimeSpan.FromSeconds(0.5));
            var lotteryPageModel = coreModel.SelectionLotteryPageModel;
            Title = lotteryPageModel.ObserveProperty(x => x.Title).ToReactiveProperty().AddTo(this.Disposable);
            LotteryCategorySelectionViewModels = lotteryPageModel.LotteryCategoryModels.ToReadOnlyReactiveCollection(x => new LotteryCategorySelectionViewModel(coreModel, x)).AddTo(this.Disposable);
            SelectionViewWidth = lotteryPageModel.ObserveProperty(x => x.CategoryModelsCount).Select(x => x * 100).ToReactiveProperty().AddTo(this.Disposable);
            LotteryLabel = lotteryPageModel.ObserveProperty(x => x.LotteryLabelText).ToReactiveProperty().AddTo(this.Disposable);
            LotteryLabelColor = lotteryPageModel.ObserveProperty(x => x.LotteryLabelColor).ToReactiveProperty().AddTo(this.Disposable);
            LotteryLabelVisible = lotteryPageModel.ObserveProperty(x => x.LotteryLabelVisible).ToReactiveProperty().AddTo(this.Disposable);
            ConfigPageNavigationCommand = new AsyncReactiveCommand<object>().WithSubscribe(async _ => {
                await navigationService.NavigateAsync(nameof(LotteryConfigPage));
            }).AddTo(this.Disposable);
        }
        public override void OnNavigatedTo(INavigationParameters parameters) {
            //何故か遷移後にgifを選択しないと動かない。Xamarinのバージョンを上げるとこうなった。2回目の表示以降はこれでもダメだがしゃーない。
            //coreModel.WaitingRancyanImage();
        }
    }
}
