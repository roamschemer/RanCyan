using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RanCyan.Models
{
    /// <summary>
    /// 全ページ共通の情報クラス
    /// </summary>
    /// [Obsolete("大幅改造により削除予定")]
    public class AllPageInfomation : BindableBase {

        /// <summary>
        /// バックカラー
        /// </summary>
        public string BackColor {
            get => backColor;
            set {
                SetProperty(ref backColor, value);
                var s = "AllPageBackColor";
                Application.Current.Properties[s] = backColor; //永続化
            }
        }
        private string backColor;

        /// <summary>
        /// スライダー
        /// </summary>
        public int ImageGridWidth {
            get => imageGridWidth;
            set {
                SetProperty(ref imageGridWidth, value);
                var s = "AllImageGridWidth";
                Application.Current.Properties[s] = imageGridWidth; //永続化
            }
        }
        private int imageGridWidth;

        public AllPageInfomation() {
            AllPageSet();
        }

        public void AllPageSet() {
            var s = "AllPageBackColor";
            if (Application.Current.Properties.ContainsKey(s)) {
                BackColor = Application.Current.Properties[s].ToString();
            } else {
                BackColor = "White";
            }
            s = "AllImageGridWidth";
            if (Application.Current.Properties.ContainsKey(s)) {
                ImageGridWidth = int.Parse(Application.Current.Properties[s].ToString());
            } else {
                ImageGridWidth = 180;
            }
        }
    }
}
