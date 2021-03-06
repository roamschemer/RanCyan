﻿using Prism.Commands;
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
        //ページ設定
        public ReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveCollection<int> AllToDrawTimeDifferenceList { get; }
        public ReactiveProperty<int> AllToDrawTimeDifference { get; }
        public ReactiveCommand<object> DeletePage { get; }
        public ReactiveCommand<string> ChangeRanshikaCommand { get; }
        public ReactiveCommand<string> ChangeRanmemo1Command { get; }
        public ReactiveCommand<string> ChangeRanmemo2Command { get; }

        //カテゴリ設定
        public ReadOnlyReactiveCollection<LotteryConfigCategoryModelSettingViewModel> LotteryConfigCategoryModelSettingViewModels { get; }
        public ReactiveProperty<LotteryConfigCategoryModelSettingViewModel> LotteryConfigCategoryModelSettingViewModel { get; }
        public ReactiveCommand<object> CreateCategoryCommand { get; }

        private CoreModel CoreModel { get; }

        public LotteryConfigPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            CoreModel = coreModel;
            //全体設定
            ImageBackColorList = coreModel.ImageBackColorList.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            ImageBackColor = coreModel.ToReactivePropertyAsSynchronized(x => x.BackColor).AddTo(this.Disposable);
            //ページ設定
            var lotteryPageModel = coreModel.SelectionLotteryPageModel;
            Title = lotteryPageModel.ToReactivePropertyAsSynchronized(x => x.Title).AddTo(this.Disposable);
            AllToDrawTimeDifferenceList = lotteryPageModel.AllToDrawTimeDifferenceList.ToReadOnlyReactiveCollection(x => x).AddTo(this.Disposable);
            AllToDrawTimeDifference = lotteryPageModel.ToReactivePropertyAsSynchronized(x => x.AllToDrawTimeDifference).AddTo(this.Disposable);
            DeletePage = new ReactiveCommand<object>().WithSubscribe(async _ => {
                var select = await Application.Current.MainPage.DisplayAlert("削除", "このページを削除しますか？", "いいよ", "待った");
                if (select) {
                    coreModel.DeleteLotteryPageModel();
                    await NavigationService.GoBackToRootAsync();
                }
            }).AddTo(this.Disposable);
            //ページ初期化
            ChangeRanshikaCommand = new ReactiveCommand<string>().WithSubscribe(async x => {
                var select = await Application.Current.MainPage.DisplayAlert("初期化", $"このページを乱屍に初期化しますか？", "いいよ", "待った");
                if (select) lotteryPageModel.ChangeRanshika();
            }).AddTo(this.Disposable);
            ChangeRanmemo1Command = new ReactiveCommand<string>().WithSubscribe(async x => {
                var select = await Application.Current.MainPage.DisplayAlert("初期化", $"このページを乱メモ1に初期化しますか？", "いいよ", "待った");
                if (select) lotteryPageModel.ChangeRanmemo1();
            }).AddTo(this.Disposable);
            ChangeRanmemo2Command = new ReactiveCommand<string>().WithSubscribe(async x => {
                var select = await Application.Current.MainPage.DisplayAlert("初期化", $"このページを乱メモ2に初期化しますか？", "いいよ", "待った");
                if (select) lotteryPageModel.ChangeRanmemo2();
            }).AddTo(this.Disposable);


            //カテゴリ設定
            CreateCategoryCommand = new ReactiveCommand<object>().WithSubscribe(_ => lotteryPageModel.Cleate());
            LotteryConfigCategoryModelSettingViewModels = lotteryPageModel.LotteryCategoryModels.ToReadOnlyReactiveCollection(x => new LotteryConfigCategoryModelSettingViewModel(lotteryPageModel, x)).AddTo(this.Disposable);
            LotteryConfigCategoryModelSettingViewModel = new ReactiveProperty<LotteryConfigCategoryModelSettingViewModel>(LotteryConfigCategoryModelSettingViewModels.First());

            //一定間隔で情報を保存
            var saveTimer = new ReactiveTimer(TimeSpan.FromSeconds(5));
            saveTimer.Subscribe(_ => CoreModel.SaveLotteryPage()).AddTo(this.Disposable);
            saveTimer.Start(TimeSpan.FromSeconds(5));
        }

        public override void OnNavigatedFrom(INavigationParameters parameters) {
            CoreModel.SaveLotteryPage();
            base.OnNavigatedFrom(parameters);
        }
    }
}
