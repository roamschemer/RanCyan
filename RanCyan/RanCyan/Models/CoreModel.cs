using Prism.Mvvm;
using RanCyan.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using static RanCyan.Models.MenuModel;

namespace RanCyan.Models {
    /// <summary>コアクラス</summary>
    public class CoreModel : BindableBase {

        /// <summary>透過用背景色のリスト</summary>
        public ObservableCollection<string> ImageBackColorList { get; }

        /// <summary>透過用背景色</summary>
        public string BackColor {
            get => backColor;
            set {
                SetProperty(ref backColor, value);
                Application.Current.Properties[$"{GetType().Name}/{nameof(BackColor)}"] = backColor;
            }
        }
        private string backColor;

        /// <summary>乱ちゃんの情報</summary>
        public RanCyanModel RanCyanModel { get => ranCyanModel; set => SetProperty(ref ranCyanModel, value); }
        private RanCyanModel ranCyanModel;

        /// <summary>選択された抽選ページ</summary>
        public LotteryPageModel SelectionLotteryPageModel { get => selectionLotteryPageModel; set => SetProperty(ref selectionLotteryPageModel, value); }
        private LotteryPageModel selectionLotteryPageModel;

        /// <summary>ページモデルのコレクション</summary>
        public ObservableCollection<LotteryPageModel> LotteryPageModels { get; private set; }

        /// <summary>jsonファイル名</summary>
        private readonly string _fileName = "LotteryPage.json";

        /// <summary>コンストラクタ</summary>
        public CoreModel() {
            ImageBackColorList = new ObservableCollection<string>() { "White", "Blue", "Lime", "DodgerBlue", "CornflowerBlue", "Chartreuse", "ForestGreen", "Yellow" };
            RanCyanModel = new RanCyanModel();
            ConfigRead();
            InfomationPageSet();
            LoadLotteryPage();
        }

        /// <summary>アプリ設定の読み込み</summary>
        private void ConfigRead() {
            string s = $"{GetType().Name}/{nameof(BackColor)}";
            BackColor = (Application.Current.Properties.ContainsKey(s)) ? Application.Current.Properties[s].ToString() : ImageBackColorList.First();
        }

        /// <summary>選択されたモデルを保有する</summary>
        /// <param name="model">選択されたモデル</param>
        public void SelectModel(LotteryPageModel model) => SelectionLotteryPageModel = model;

        /// <summary>
        /// 見本作成する
        /// </summary>
        private void ResetModels() {
            CleateNewLotteryPageModel();
            LotteryPageModels.Last().ChangeRanshika();
            CleateNewLotteryPageModel();
            LotteryPageModels.Last().ChangeRanmemo1();
            CleateNewLotteryPageModel();
            LotteryPageModels.Last().ChangeRanmemo2();
        }

        private void InfomationPageSet() {
            var items = new List<LotteryPageModel>(){
                new LotteryPageModel() {
                    Title = "取説(外部ページへ飛びます)",
                    MenuModel = new MenuModel() {
                        ImageAddress = "resource://RanCyan.Images.Ranshika.png",
                        ViewAddress = "https://www.gunshi.info/rancyanproject",
                        PageType = MenuModel.PageTypeEnum.Web
                    }
                },
                new LotteryPageModel() {
                    Title = "ライセンス情報",
                    MenuModel = new MenuModel() {
                        ImageAddress = "resource://RanCyan.Images.Ranshika.png",
                        ViewAddress = "",
                        PageType = MenuModel.PageTypeEnum.Other
                    }
                },
            };
            LotteryPageModels = new ObservableCollection<LotteryPageModel>(items);
        }

        /// <summary>
        /// 新規にLotteryPageModelを追加する
        /// </summary>
        public void CleateNewLotteryPageModel() {
            var images = new[] {
                "resource://RanCyan.Images.MiniMikoRanCyan.png",
                "resource://RanCyan.Images.MiniKowashiyaRanCyan.png"
            };
            var index = LotteryPageModels.Count(x => x.MenuModel.PageType == MenuModel.PageTypeEnum.Lottery);
            var model = new LotteryPageModel() {
                Title = $"NewPage{index}",
                MenuModel = new MenuModel() {
                    ViewAddress = Device.RuntimePlatform == Device.UWP ? nameof(LotteryUwpPage) : nameof(LotteryMobilePage),
                    ImageAddress = images[index % images.Count()],
                    PageType = MenuModel.PageTypeEnum.Lottery
                }
            };
            LotteryPageModels.Add(model);
            SaveLotteryPage();
        }

        /// <summary>
        /// 現在のLotteryPageModelを削除する
        /// </summary>
        public void DeleteLotteryPageModel() {
            LotteryPageModels.Remove(SelectionLotteryPageModel);
            if (LotteryPageModels.Count(x => x.MenuModel.PageType == MenuModel.PageTypeEnum.Lottery) == 0) CleateNewLotteryPageModel();
            SaveLotteryPage();
        }

        /// <summary>各ページ以下の情報をjsonで保存</summary>
        public void SaveLotteryPage() {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _fileName);
            var options = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            var models = new ObservableCollection<LotteryPageModel>(LotteryPageModels);
            var json = JsonSerializer.Serialize(models.Where(x => x.MenuModel.PageType == PageTypeEnum.Lottery), options);
            File.WriteAllText(fileName, json);
        }

        /// <summary>各ページ以下の情報をjsonから復元</summary>
        private void LoadLotteryPage() {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _fileName);
            if (!File.Exists(fileName)) {
                ResetModels();
                return;
            }
            var json = File.ReadAllText(fileName);
            var models = JsonSerializer.Deserialize<ObservableCollection<LotteryPageModel>>(json);
            foreach (var x in models) LotteryPageModels.Add(x);
        }

    }
}
