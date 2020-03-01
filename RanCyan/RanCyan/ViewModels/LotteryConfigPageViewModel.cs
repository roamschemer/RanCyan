﻿using Prism.Commands;
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
        //全体設定
        public ReadOnlyReactiveCollection<string> ImageBackColorList { get; }
        public ReactiveProperty<string> ImageBackColor { get; }
        public ReadOnlyReactiveCollection<int> SelectionViewWidthList { get; }
        public ReactiveProperty<int> SelectionViewWidth { get; }
        //ページ設定
        public ReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveCollection<int> AllToDrawTimeDifferenceList { get; }
        public ReactiveProperty<int> AllToDrawTimeDifference { get; }
        //カテゴリ設定
        public ReadOnlyReactiveCollection<LotteryCategoryModel> LotteryCategoryModels { get; }
        public ReactiveProperty<LotteryCategoryModel> SelectedLotteryCategoryModel { get; }
        public ReactiveCommand<object> CreateCategoryCommand { get; }

        public LotteryConfigPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            //全体設定
            ImageBackColorList = coreModel.ImageBackColorList.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            ImageBackColor = coreModel.ToReactivePropertyAsSynchronized(x => x.BackColor).AddTo(this.Disposable);
            SelectionViewWidthList = coreModel.SelectionViewWidthList.ToReadOnlyReactiveCollection(x => x).AddTo(this.Disposable);
            SelectionViewWidth = coreModel.ToReactivePropertyAsSynchronized(x => x.SelectionViewWidth).AddTo(this.Disposable);
            //ページ設定
            var lotteryPageModel = coreModel.SelectionLotteryPageModel;
            Title = lotteryPageModel.ToReactivePropertyAsSynchronized(x => x.Title).AddTo(this.Disposable);
            AllToDrawTimeDifferenceList = lotteryPageModel.AllToDrawTimeDifferenceList.ToReadOnlyReactiveCollection(x => x).AddTo(this.Disposable);
            AllToDrawTimeDifference = lotteryPageModel.ToReactivePropertyAsSynchronized(x => x.AllToDrawTimeDifference).AddTo(this.Disposable);
            //カテゴリ設定
            LotteryCategoryModels = lotteryPageModel.LotteryCategoryModels.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            SelectedLotteryCategoryModel = new ReactiveProperty<LotteryCategoryModel>(LotteryCategoryModels.First());
            CreateCategoryCommand = new ReactiveCommand<object>().WithSubscribe(_ => lotteryPageModel.CleateNewLotteryCategoryModel()).AddTo(this.Disposable);
        }
    }
}
