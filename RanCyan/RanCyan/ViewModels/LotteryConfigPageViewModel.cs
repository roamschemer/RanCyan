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
    /// <summary>設定画面</summary>
    public class LotteryConfigPageViewModel : ViewModelBase {
        //全体設定
        public ReadOnlyReactiveCollection<string> ImageBackColorList { get; }
        public ReactiveProperty<string> ImageBackColor { get; }
        public ReadOnlyReactiveCollection<int> SelectionViewWidthList { get; }
        public ReactiveProperty<int> SelectionViewWidth { get; }
        //ページ設定
        public ReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveCollection<int> AllToDrawTimeDifferenceList { get; }
        public ReactiveProperty<int> AllToDrawTimeDifference { get; }
        public ReactiveCommand<object> DeletePage { get; }
        //カテゴリ設定
        public ReadOnlyReactiveCollection<LotteryCategoryModel> LotteryCategoryModels { get; }
        public ReactiveProperty<LotteryCategoryModel> SelectedLotteryCategoryModel { get; }
        public ReactiveCommand<object> CreateCategoryCommand { get; }
        public ReadOnlyReactiveCollection<LotteryModel> LotteryCategoryModes { get; }
        public ReadOnlyReactiveCollection<LotteryConfigLotteryModelSettingViewModel> LotteryConfigLotteryModelSettingViewModels { get; private set; }

        public LotteryConfigPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            //全体設定
            ImageBackColorList = coreModel.ImageBackColorList.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            ImageBackColor = coreModel.ToReactivePropertyAsSynchronized(x => x.BackColor).AddTo(this.Disposable);
            //ページ設定
            var lotteryPageModel = coreModel.SelectionLotteryPageModel;
            Title = lotteryPageModel.ToReactivePropertyAsSynchronized(x => x.Title).AddTo(this.Disposable);
            AllToDrawTimeDifferenceList = lotteryPageModel.AllToDrawTimeDifferenceList.ToReadOnlyReactiveCollection(x => x).AddTo(this.Disposable);
            AllToDrawTimeDifference = lotteryPageModel.ToReactivePropertyAsSynchronized(x => x.AllToDrawTimeDifference).AddTo(this.Disposable);
            DeletePage = new ReactiveCommand<object>().WithSubscribe(async _ => {
                var select = await Application.Current.MainPage.DisplayAlert("削除", "このページを削除しますか？\r\n※この操作は戻せません", "いいよ", "待った");
                if (select) {
                    coreModel.DeleteLotteryPageModel();
                    await NavigationService.GoBackToRootAsync();
                }
            }).AddTo(this.Disposable);
            //カテゴリ設定
            CreateCategoryCommand = new ReactiveCommand<object>().WithSubscribe(async _ => {
                var select = await Application.Current.MainPage.DisplayAlert("新規追加", "新規追加しますか？", "いいよ", "待った");
                if (select) lotteryPageModel.CleateNewLotteryCategoryModel();
            }).AddTo(this.Disposable);
            LotteryCategoryModels = lotteryPageModel.LotteryCategoryModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            SelectedLotteryCategoryModel = new ReactiveProperty<LotteryCategoryModel>(LotteryCategoryModels.First());
            SelectedLotteryCategoryModel.Subscribe(x => {
                LotteryConfigLotteryModelSettingViewModels = SelectedLotteryCategoryModel.Value.LotteryModels
                    .ToReadOnlyReactiveCollection(y => new LotteryConfigLotteryModelSettingViewModel(y)).AddTo(this.Disposable);
                this.RaisePropertyChanged(nameof(LotteryConfigLotteryModelSettingViewModels));
            });
        }
    }
}
