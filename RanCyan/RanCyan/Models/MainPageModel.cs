using Prism.Mvvm;
using RanCyan.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace RanCyan.Models {
    /// <summary>メインページクラス</summary>
    public class MainPageModel : BindableBase {

        /// <summary>メニューコレクション</summary>
        public ObservableCollection<MenuModel> MenuModels { get; }

        public MainPageModel() {
            //CoreModel情報を先に作成しておく
            var images = new[] {
                "resource://RanCyan.Images.MiniMikoRanCyan.png",
                "resource://RanCyan.Images.MiniKowashiyaRanCyan.png"
            };
            var items = new CoreModel().LotteryPageModels
                                       .Select((x, i) => new MenuModel() {
                                           Title = x.Title,
                                           ViewAddress = nameof(LotteryUwpPageViewModel),
                                           ImageAddress = images[i % images.Count()],
                                           LotteryPageIndex = i
                                       });
            //コレクション化する
            MenuModels = new ObservableCollection<MenuModel>(items);
            //その他ページを追加する
            MenuModels.Insert(0, new MenuModel() {
                Title = "取説(外部ページへ飛びます)",
                ImageAddress = "resource://RanCyan.Images.Ranshika.png",
                ViewAddress = "https://www.gunshi.info/rancyanproject",
                IsWebPage = true,
            });
            MenuModels.Add(new MenuModel() {
                Title = "ライセンス情報",
                ImageAddress = "resource://RanCyan.Images.Ranshika.png",
                ViewAddress = ""
            });
        }
    }
}
