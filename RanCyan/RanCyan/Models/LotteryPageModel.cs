﻿using Prism.Mvvm;
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
        public ObservableCollection<LotteryCategoryModel> CategoryModels {
            get => categoryModels;
            set {
                SetProperty(ref categoryModels, value);
                CategoryModelsCount = categoryModels.Count();
            }
        }
        private ObservableCollection<LotteryCategoryModel> categoryModels;

        /// <summary>コンストラクタ</summary>
        public LotteryPageModel() {
            ResetModels();
        }

        /// <summary>
        /// 見本作成
        /// </summary>
        private void ResetModels() {
            var items = Enumerable.Range(0, 4).Select(x => new LotteryCategoryModel() { Title = $"Category{x}" });
            CategoryModels = new ObservableCollection<LotteryCategoryModel>(items);
        }


    }
}
