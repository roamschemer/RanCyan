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
        public ReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveCollection<LotteryCategorySelectionViewModel> LotteryCategorySelectionViewModels { get; }
        public ReactiveProperty<int> SelectionViewWidth { get; }
        public ReactiveProperty<int> CategoryModelsCount { get; }
        public ReadOnlyReactiveProperty<int> SelectionViewOneWidth { get; }
        public ReactiveProperty<string> RanCyanImage { get; }
        public ReactiveProperty<bool> IsImageActive { get; }
        public ReactiveProperty<LotteryCategoryModel> LotteryCategorySelectedModel { get; }
        //public ReactiveProperty<string> LotteryLabelText { get; }
        //public ReactiveProperty<string> LotteryLabelColor { get; }
        //public ReactiveProperty<bool> LotteryLabelVisible { get; }
        public AsyncReactiveCommand<object> ConfigPageNavigationCommand { get; }
        public ReactiveCommand<object> AllToDrawCommand { get; }
        public ReactiveProperty<bool> IsAllToDraw { get; }

        public ReadOnlyReactiveCollection<LotteryCategoryModel> LotteryCategoryModels { get; }

        public LotteryPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            //乱ちゃんイメージ
            var ranCyanModel = coreModel.RanCyanModel;
            ranCyanModel.StartRancyanImage();
            RanCyanImage = ranCyanModel.ObserveProperty(x => x.RanCyanImage).ToReactiveProperty().AddTo(this.Disposable);
            IsImageActive = ranCyanModel.ObserveProperty(x => x.IsImageActive).ToReactiveProperty().AddTo(this.Disposable);
            var imageTimer = new ReactiveTimer(TimeSpan.FromSeconds(10));
            imageTimer.Subscribe(_ => {
                ranCyanModel.WaitingRancyanImage();
                imageTimer.Stop();
            });
            imageTimer.Start(TimeSpan.FromSeconds(0.5));
            //ContentView
            var lotteryPageModel = coreModel.SelectionLotteryPageModel;
            LotteryCategorySelectionViewModels = lotteryPageModel.LotteryCategoryModels.ToReadOnlyReactiveCollection(x => new LotteryCategorySelectionViewModel(coreModel, x)).AddTo(this.Disposable);
            //表示系
            Title = lotteryPageModel.ObserveProperty(x => x.Title).ToReactiveProperty().AddTo(this.Disposable);
            SelectionViewWidth = coreModel.ObserveProperty(x => x.SelectionViewWidth).ToReactiveProperty().AddTo(this.Disposable);
            CategoryModelsCount = lotteryPageModel.ObserveProperty(x => x.CategoryModelsCount).Select(x => (x > 0) ? x : 1).ToReactiveProperty().AddTo(this.Disposable);
            SelectionViewOneWidth = SelectionViewWidth.CombineLatest(CategoryModelsCount, (x, y) => x / y).ToReadOnlyReactiveProperty().AddTo(this.Disposable);
            LotteryCategorySelectedModel = lotteryPageModel.ObserveProperty(x => x.SelectionLotteryCategoryModel).Select(x => { return x; }).ToReactiveProperty().AddTo(this.Disposable);
            //LotteryLabelText = LotteryCategorySelectedModel.Value.ObserveProperty(x => x.LotteryLabelModel.Text).Select(x => { return x; }).ToReactiveProperty().AddTo(this.Disposable);
            //LotteryLabelColor = LotteryCategorySelectedModel.Value.ObserveProperty(x => x.LotteryLabelModel.Color).Select(x => { return x; }).ToReactiveProperty().AddTo(this.Disposable);
            //LotteryLabelVisible = LotteryCategorySelectedModel.Value.ObserveProperty(x => x.LotteryLabelModel.Visible).Select(x => { return x; }).ToReactiveProperty().AddTo(this.Disposable);
            LotteryCategoryModels = lotteryPageModel.LotteryCategoryModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            IsAllToDraw = lotteryPageModel.ObserveProperty(x => x.IsAllToDraw).ToReactiveProperty().AddTo(this.Disposable);
            //発火系
            AllToDrawCommand = new ReactiveCommand().WithSubscribe(_ => {
                ranCyanModel.LotteryRancyanImageAsync();
                lotteryPageModel.AllToDraw();
            }).AddTo(this.Disposable);
            ConfigPageNavigationCommand = new AsyncReactiveCommand<object>().WithSubscribe(async _ => {
                await navigationService.NavigateAsync(nameof(LotteryConfigPage));
            }).AddTo(this.Disposable);
        }
    }
}
