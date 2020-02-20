using Prism.Mvvm;
using RanCyan.Views;
using System.Collections.ObjectModel;
using System.Linq;

namespace RanCyan.Models {
    /// <summary>メインページクラス</summary>
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
        /// CoreModelをインジェクションしてリストに追加する
        /// </summary>
        /// <param name="coreModel">CoreModel</param>
        public void CoreModelInjection(CoreModel coreModel) {
            var images = new[] {
                "resource://RanCyan.Images.MiniMikoRanCyan.png",
                "resource://RanCyan.Images.MiniKowashiyaRanCyan.png"
            };
            var items = coreModel.LotteryPageModels.Select((x, i) => new MenuModel() {
                Title = x.Title,
                ViewAddress = nameof(LotteryUwpPage),
                ImageAddress = images[i % images.Count()],
                LotteryPageIndex = i
            });
            foreach (var x in items) {
                MenuModels.Insert(MenuModels.Count() - 1, x);
            }
        }
    }
}
