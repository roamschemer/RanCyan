using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism;
using Prism.Mvvm;

namespace RanCyan.Models {
    /// <summary>カテゴリークラス</summary>
    public class CategoryModel {
        public ObservableCollection<LotteryModel> LotteryModels { get; }
    }
}
