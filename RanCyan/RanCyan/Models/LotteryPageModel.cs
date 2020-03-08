﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RanCyan.Models {
    /// <summary>ページクラス</summary>
    public class LotteryPageModel : BindableBase {

        /// <summary>ページタイトル</summary>
        public string Title { get => title; set => SetProperty(ref title, value); }
        private string title;

        /// <summary>全体抽選の実施</summary>
        public bool IsAllToDraw { get => isAllToDraw; set => SetProperty(ref isAllToDraw, value); }
        private bool isAllToDraw;

        /// <summary>全体抽選の動作時間差(msec)</summary>
        public int AllToDrawTimeDifference { get => allToDrawTimeDifference; set => SetProperty(ref allToDrawTimeDifference, value); }
        private int allToDrawTimeDifference;

        /// <summary>全体抽選の動作時間差(msec)リスト</summary>
        public ObservableCollection<int> AllToDrawTimeDifferenceList { get; }

        /// <summary>カテゴリーのリスト</summary>
        public ObservableCollection<LotteryCategoryModel> LotteryCategoryModels {
            get => lotteryCategoryModels;
            set => SetProperty(ref lotteryCategoryModels, value);
        }
        private ObservableCollection<LotteryCategoryModel> lotteryCategoryModels;

        /// <summary>選択された抽選カテゴリ</summary>
        public LotteryCategoryModel SelectionLotteryCategoryModel { get => selectionLotteryCategoryModel; set => SetProperty(ref selectionLotteryCategoryModel, value); }
        private LotteryCategoryModel selectionLotteryCategoryModel;

        /// <summary>メニュー表示用情報</summary>
        public MenuModel MenuModel { get => menuModel; set => SetProperty(ref menuModel, value); }
        private MenuModel menuModel;

        /// <summary>コンストラクタ</summary>
        public LotteryPageModel() {
            SelectionLotteryCategoryModel = new LotteryCategoryModel();
            AllToDrawTimeDifferenceList = new ObservableCollection<int>(Enumerable.Range(0, 11).Select(x => x * 100));
            ResetModels();
            ConfigRead();
        }

        /// <summary>アプリ設定の読み込み</summary>
        private void ConfigRead() {
            AllToDrawTimeDifference = AllToDrawTimeDifferenceList.Skip(5).First();
        }

        /// <summary>
        /// 見本作成
        /// </summary>
        private void ResetModels() {
            var items = Enumerable.Range(0, 4).Select(x => new LotteryCategoryModel() { Title = $"Category{x}" });
            LotteryCategoryModels = new ObservableCollection<LotteryCategoryModel>(items);
        }

        /// <summary>
        /// 新規にLotteryCategoryModelを追加する
        /// </summary>
        public void CleateNewLotteryCategoryModel() => LotteryCategoryModels.Add(new LotteryCategoryModel() { Title = $"Category{LotteryCategoryModels.Count()}" });

        /// <summary>全項目抽選の実施</summary>
        public async void AllToDraw() {
            IsAllToDraw = true;
            foreach (var (x, i) in LotteryCategoryModels.Select((x, i) => (x, i))) {
                if (i > 0) await Task.Delay(AllToDrawTimeDifference);
                x.ToDrawAsync(this);
            }
        }
    }
}
