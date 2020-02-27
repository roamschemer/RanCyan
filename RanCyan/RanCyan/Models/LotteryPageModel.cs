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

        public LotteryLabelModel LotteryLabelModel { get => lotteryLabelModel; set => SetProperty(ref lotteryLabelModel, value); }
        private LotteryLabelModel lotteryLabelModel;

        /// <summary>コンストラクタ</summary>
        public LotteryPageModel() {
            ResetModels();
            LotteryLabelModel = new LotteryLabelModel();
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

    }
}
