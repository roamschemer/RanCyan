﻿using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace RanCyan.Models {
    /// <summary>カテゴリークラス</summary>
    public class LotteryCategoryModel : BindableBase {

        /// <summary>カテゴリ名称(ボタンの名称にも使用)</summary>
        public string Name { get => name; set => SetProperty(ref name, value); }
        private string name;

        /// <summary>ループする回数</summary>
        public int NumberOfLoops { get => numberOfLoops; set => SetProperty(ref numberOfLoops, value); }
        private int numberOfLoops = 10;

        /// <summary>全ループ合計時間(msec)</summary>
        public int TotalTimeOfAllLoops { get => totalTimeOfAllLoops; set => SetProperty(ref totalTimeOfAllLoops, value); }
        private int totalTimeOfAllLoops = 1000;

        /// <summary>抽選中</summary>
        public bool InLottery { get => inLottery; set => SetProperty(ref inLottery, value); }
        private bool inLottery;

        /// <summary>抽選モデルのコレクション</summary>
        public ObservableCollection<LotteryModel> LotteryModels { get; private set; }

        /// <summary>コンストラクタ</summary>
        public LotteryCategoryModel() {
            ResetModels();
        }

        /// <summary>
        /// 見本生成
        /// </summary>
        private void ResetModels() {
            var items = Enumerable.Range(0, 6).Select(x => new LotteryModel() { Id = x, Name=$"select{x}" });
            LotteryModels = new ObservableCollection<LotteryModel>(items);
        }
    }
}