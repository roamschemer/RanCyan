using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RanCyan.Models {
    /// <summary>ページクラス</summary>
    public class LotteryPageModel : BindableBase {

        /// <summary>ページタイトル</summary>
        public string Title { get => title; set => SetProperty(ref title, value); }
        private string title;

        /// <summary>カテゴリーの数</summary>
        public int CategoryModelsCount { get => categoryModelsCount; set => SetProperty(ref categoryModelsCount, value); }
        private int categoryModelsCount;

        /// <summary>カテゴリーのリスト</summary>
        public ObservableCollection<LotteryCategoryModel> LotteryCategoryModels {
            get => lotteryCategoryModels;
            set {
                SetProperty(ref lotteryCategoryModels, value);
                CategoryModelsCount = lotteryCategoryModels.Count();
            }
        }
        private ObservableCollection<LotteryCategoryModel> lotteryCategoryModels;

        /// <summary>選択された抽選カテゴリ</summary>
        public LotteryCategoryModel SelectionLotteryCategoryModel { get => selectionLotteryCategoryModel; set => SetProperty(ref selectionLotteryCategoryModel, value); }
        private LotteryCategoryModel selectionLotteryCategoryModel;

        /// <summary>抽選結果表示用ラベルの文字</summary>
        public string LotteryLabelText { get => lotteryLabelText; set => SetProperty(ref lotteryLabelText, value); }
        private string lotteryLabelText;

        /// <summary>抽選結果表示用ラベルの文字色</summary>
        public string LotteryLabelColor { get => lotteryLabelColor; set => SetProperty(ref lotteryLabelColor, value); }
        private string lotteryLabelColor;

        /// <summary>抽選結果表示用ラベルの文字表示</summary>
        public bool LotteryLabelVisible { get => lotteryLabelVisible; set => SetProperty(ref lotteryLabelVisible, value); }
        private bool lotteryLabelVisible;

        /// <summary>コンストラクタ</summary>
        public LotteryPageModel() {
            ResetModels();
        }

        /// <summary>
        /// 見本作成
        /// </summary>
        private void ResetModels() {
            var items = Enumerable.Range(0, 4).Select(x => new LotteryCategoryModel() { Title = $"Category{x}" });
            LotteryCategoryModels = new ObservableCollection<LotteryCategoryModel>(items);
        }

    }
}
