using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace RanCyan.Models {
    /// <summary>抽選ラベルの情報</summary>
    public class LotteryLabelModel : BindableBase {
        /// <summary>抽選結果表示用ラベルの文字</summary>
        public string Text { get => text; set => SetProperty(ref text, value); }
        private string text;

        /// <summary>抽選結果表示用ラベルの文字色</summary>
        public string Color { get => color; set => SetProperty(ref color, value); }
        private string color;

        /// <summary>抽選結果表示用ラベルの文字表示</summary>
        public bool Visible { get => visible; set => SetProperty(ref visible, value); }
        private bool visible;
    }
}
