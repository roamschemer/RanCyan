using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RanCyan.Models {
    /// <summary>コアクラス</summary>
    public class CoreModel : BindableBase {

        /// <summary>透過用背景色</summary>
        public string BackColor { get => backColor; set => SetProperty(ref backColor, value); }
        private string backColor;

        /// <summary>ページモデルのコレクション</summary>
        public ObservableCollection<LotteryPageModel> LotteryPageModels { get; private set; }

        /// <summary>コンストラクタ</summary>
        public CoreModel() {
            ResetModels();
        }

        /// <summary>
        /// 見本作成する
        /// </summary>
        private void ResetModels() {
            var items = Enumerable.Range(0, 1).Select(x => new LotteryPageModel() { Title = $"NewPage" });
            LotteryPageModels = new ObservableCollection<LotteryPageModel>(items);
        }
    }
}
