using Prism.Mvvm;
using RanCyan.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RanCyan.Models {
    /// <summary>メインページクラス</summary>
    [Obsolete("削除予定")]
    public class MainPageModel : BindableBase {

        /// <summary>メニューコレクション</summary>
        public ObservableCollection<MenuModel> MenuModels { get; }

        public MainPageModel() {
            //コレクション化する
            MenuModels = new ObservableCollection<MenuModel>(){
                new MenuModel() {
                    Title = "取説(外部ページへ飛びます)",
                    ImageAddress = "resource://RanCyan.Images.Ranshika.png",
                    ViewAddress = "https://www.gunshi.info/rancyanproject",
                    IsWebPage = true,
                },
                new MenuModel() {
                    Title = "ライセンス情報",
                    ImageAddress = "resource://RanCyan.Images.Ranshika.png",
                    ViewAddress = "",
                }
            };
        }

        /// <summary>
        /// CoreModelを丸ごとメニューリストに追加する
        /// </summary>
        /// <param name="coreModel">CoreModel</param>
        public void CoreModelInjection(CoreModel coreModel) {
            foreach (var (x, i) in coreModel.LotteryPageModels.Select((x, i) => (x, i))) {
                AddMenuModel(x, i);
            }
        }

        /// <summary>
        /// CoreModelにLotteryPageModelを追加して、メニューにも追加する
        /// </summary>
        /// <param name="coreModel">coreModel</param>
        public void LastAddLotteryPageMode(CoreModel coreModel) {
            coreModel.CleateNewLotteryPageModel();
            AddMenuModel(coreModel.LotteryPageModels.Last(), coreModel.LotteryPageModels.Count() - 1);
        }

        /// <summary>
        /// lotteryPageModelをメニューに追加する
        /// </summary>
        /// <param name="lotteryPageModel">lotteryPageModel</param>
        /// <param name="index">index</param>
        private void AddMenuModel(LotteryPageModel lotteryPageModel, int index) {
            var images = new[] {
                "resource://RanCyan.Images.MiniMikoRanCyan.png",
                "resource://RanCyan.Images.MiniKowashiyaRanCyan.png"
            };
            var menuModel = new MenuModel() {
                Title = lotteryPageModel.Title,
                ViewAddress = $"NavigationPage/MainPage/{nameof(LotteryUwpPage)}",
                ImageAddress = images[index % images.Count()],
                LotteryPageIndex = index
            };
            MenuModels.Add(menuModel);
        }
    }
}
