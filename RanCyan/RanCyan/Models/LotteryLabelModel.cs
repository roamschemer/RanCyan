using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace RanCyan.Models {
    /// <summary>抽選ラベルの情報</summary>
    public class LotteryLabelModel : BindableBase {

        /// <summary>ラベルの文字</summary>
        public string Text { get => text; set => SetProperty(ref text, value); }
        private string text;

        /// <summary>ラベルの文字色</summary>
        public string Color { get => color; set => SetProperty(ref color, value); }
        private string color;

    }
}
