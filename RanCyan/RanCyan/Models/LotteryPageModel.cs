using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RanCyan.Models {
    /// <summary>ページクラス</summary>
    public class LotteryPageModel : BindableBase {

        /// <summary>ページタイトル</summary>
        public string Title { get => title; set => SetProperty(ref title, value); }
        private string title;

        /// <summary>全体抽選の実施</summary>
        [JsonIgnore]
        public bool IsAllToDraw { get => isAllToDraw; set => SetProperty(ref isAllToDraw, value); }
        private bool isAllToDraw;

        /// <summary>全体抽選の動作時間差(msec)</summary>
        public int AllToDrawTimeDifference { get => allToDrawTimeDifference; set => SetProperty(ref allToDrawTimeDifference, value); }
        private int allToDrawTimeDifference;

        /// <summary>カテゴリーのリスト</summary>
        public ObservableCollection<LotteryCategoryModel> LotteryCategoryModels {
            get => lotteryCategoryModels;
            set => SetProperty(ref lotteryCategoryModels, value);
        }
        private ObservableCollection<LotteryCategoryModel> lotteryCategoryModels;

        /// <summary>全体抽選の動作時間差(msec)リスト</summary>
        [JsonIgnore]
        public ObservableCollection<int> AllToDrawTimeDifferenceList { get; }

        /// <summary>選択された抽選カテゴリ</summary>
        [JsonIgnore]
        public LotteryCategoryModel SelectionLotteryCategoryModel { get => selectionLotteryCategoryModel; set => SetProperty(ref selectionLotteryCategoryModel, value); }
        private LotteryCategoryModel selectionLotteryCategoryModel;

        /// <summary>メニュー表示用情報</summary>
        public MenuModel MenuModel { get => menuModel; set => SetProperty(ref menuModel, value); }
        private MenuModel menuModel;

        /// <summary>コンストラクタ</summary>
        public LotteryPageModel() {
            SelectionLotteryCategoryModel = new LotteryCategoryModel();
            SelectionLotteryCategoryModel.ResetModels();
            AllToDrawTimeDifferenceList = new ObservableCollection<int>(Enumerable.Range(0, 11).Select(x => x * 100));
            ResetModels();
            ConfigRead();
        }

        /// <summary>アプリ設定の読み込み</summary>
        private void ConfigRead() {
            AllToDrawTimeDifference = AllToDrawTimeDifferenceList.Skip(5).First();
        }

        /// <summary>
        /// 見本作成
        /// </summary>
        private void ResetModels() {
            var items = Enumerable.Range(0, 1).Select(x => {
                var model = new LotteryCategoryModel() { Title = $"Category{x}" };
                model.ResetModels();
                return model;
            });
            LotteryCategoryModels = new ObservableCollection<LotteryCategoryModel>(items);
        }

        /// <summary>
        /// 新規にLotteryCategoryModelを追加する
        /// </summary>
        public void Cleate() {
            var model = new LotteryCategoryModel() { Title = $"Category{LotteryCategoryModels.Count()}" };
            model.ResetModels();
            LotteryCategoryModels.Add(model);
        }

        /// <summary>カテゴリの前移動</summary>
        public void Up(LotteryCategoryModel m) {
            var item = LotteryCategoryModels.Select((model, index) => (model, index)).First(x => x.model == m);
            if (item.index == 0) return;
            LotteryCategoryModels.Move(item.index, item.index - 1);
        }

        /// <summary>カテゴリの後移動</summary>
        public void Down(LotteryCategoryModel m) {
            var item = LotteryCategoryModels.Select((model, index) => (model, index)).First(x => x.model == m);
            if (item.index == LotteryCategoryModels.Count - 1) return;
            LotteryCategoryModels.Move(item.index, item.index + 1);
        }

        /// <summary>抽選モデルの削除</summary>
        public void Clear(LotteryCategoryModel m) {
            if (LotteryCategoryModels.Count == 1) return;
            LotteryCategoryModels.Remove(m);
        }

        /// <summary>
        /// ショートカットからの抽選実施
        /// </summary>
        /// <param name="key">ショートカットキー</param>
        public async Task LotteryShortcutAsync(CoreModel coreModel, string key) {
            if (key == "A") {
                coreModel.RanCyanModel.LotteryRancyanImageAsync();
                AllToDraw();
                return;
            }
            var keyItems = new[] { "Z", "X", "C", "V", "B", "N", "M", ".", "/", "\\" };
            if (!keyItems.Contains(key)) return;
            var index = keyItems.Select((value, i) => (value, i)).Where(x => key == x.value).Select(x => x.i).FirstOrDefault();
            if (index < LotteryCategoryModels.Count()) {
                coreModel.SelectionLotteryPageModel.IsAllToDraw = false;
                coreModel.RanCyanModel.LotteryRancyanImageAsync();
                await LotteryCategoryModels[index].ToDrawAsync(coreModel.SelectionLotteryPageModel);
            }
        }

        /// <summary>全項目抽選の実施</summary>
        public async void AllToDraw() {
            IsAllToDraw = true;
            foreach (var (x, i) in LotteryCategoryModels.Select((x, i) => (x, i))) {
                if (i > 0) await Task.Delay(AllToDrawTimeDifference);
                _ = x.ToDrawAsync(this);
            }
        }

        /// <summary>
        /// このページの設定を乱屍にする
        /// </summary>
        public void ChangeRanshika() {
            Title = "乱屍";
            LotteryCategoryModels.Clear();
            LotteryCategoryModel model;
            model = new LotteryCategoryModel() {
                Title = "進言", NumberOfLoops = 10, TotalTimeOfAllLoops = 500, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel(){ Name = "進言1" },
                    new LotteryModel(){ Name = "進言2" },
                    new LotteryModel(){ Name = "進言3" },
                }
            };
            LotteryCategoryModels.Add(model);
            model = new LotteryCategoryModel() {
                Title = "交神", NumberOfLoops = 50, TotalTimeOfAllLoops = 5000, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel(){ Name = "火の神様" },
                    new LotteryModel(){ Name = "水の神様" },
                    new LotteryModel(){ Name = "風の神様" },
                    new LotteryModel(){ Name = "土の神様" },
                }
            };
            LotteryCategoryModels.Add(model);
            model = new LotteryCategoryModel() {
                Title = "職業", NumberOfLoops = 50, TotalTimeOfAllLoops = 5000, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel(){ Name = "剣士" },
                    new LotteryModel(){ Name = "薙刀士" },
                    new LotteryModel(){ Name = "弓使い" },
                    new LotteryModel(){ Name = "槍使い" },
                    new LotteryModel(){ Name = "拳法家" },
                    new LotteryModel(){ Name = "壊し屋" },
                    new LotteryModel(){ Name = "大筒士" },
                    new LotteryModel(){ Name = "踊り屋" },
                }
            };
            LotteryCategoryModels.Add(model);
            model = new LotteryCategoryModel() {
                Title = "討伐先", NumberOfLoops = 50, TotalTimeOfAllLoops = 5000, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel(){ Name = "鳥居千万宮" },
                    new LotteryModel(){ Name = "相翼院" },
                    new LotteryModel(){ Name = "九重楼" },
                    new LotteryModel(){ Name = "白骨城" },
                    new LotteryModel(){ Name = "忘我流水道" },
                    new LotteryModel(){ Name = "親王鎮魂墓" },
                    new LotteryModel(){ Name = "紅蓮の祠" },
                    new LotteryModel(){ Name = "大江山" },
                    new LotteryModel(){ Name = "地獄巡り" },
                }
            };
            LotteryCategoryModels.Add(model);
        }

        /// <summary>
        /// このページの設定を乱メモ1にする
        /// </summary>
        public void ChangeRanmemo1() {
            Title = "乱メモ1";
            LotteryCategoryModels.Clear();
            LotteryCategoryModel model;
            model = new LotteryCategoryModel() {
                Title = "平日", NumberOfLoops = 10, TotalTimeOfAllLoops = 500, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel(){ Name = "文学" },
                    new LotteryModel(){ Name = "理学" },
                    new LotteryModel(){ Name = "芸術" },
                    new LotteryModel(){ Name = "運動" },
                    new LotteryModel(){ Name = "部活" ,IsSelected = true},
                    new LotteryModel(){ Name = "遊び" },
                    new LotteryModel(){ Name = "容姿" },
                    new LotteryModel(){ Name = "休憩" ,Ratio = 2 },
                }
            };
            LotteryCategoryModels.Add(model);
            model = new LotteryCategoryModel() {
                Title = "休日", NumberOfLoops = 10, TotalTimeOfAllLoops = 500, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel(){ Name = "文学" },
                    new LotteryModel(){ Name = "理学" },
                    new LotteryModel(){ Name = "芸術" },
                    new LotteryModel(){ Name = "運動" },
                    new LotteryModel(){ Name = "部活" ,IsSelected = true},
                    new LotteryModel(){ Name = "遊び" },
                    new LotteryModel(){ Name = "容姿" },
                    new LotteryModel(){ Name = "休憩" },
                    new LotteryModel(){ Name = "電話" ,Ratio = 2},
                }
            };
            LotteryCategoryModels.Add(model);
            model = new LotteryCategoryModel() {
                Title = "下校", NumberOfLoops = 10, TotalTimeOfAllLoops = 500, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel(){ Name = "一緒に帰る" },
                    new LotteryModel(){ Name = "俺様は忙しい" },
                }
            };
            LotteryCategoryModels.Add(model);
            model = new LotteryCategoryModel() {
                Title = "攻略対象", NumberOfLoops = 50, TotalTimeOfAllLoops = 5000, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel { Name = "藤崎詩織" },
                    new LotteryModel { Name = "如月未緒" },
                    new LotteryModel { Name = "紐緒結奈" },
                    new LotteryModel { Name = "片桐彩子" },
                    new LotteryModel { Name = "虹野沙希" },
                    new LotteryModel { Name = "古式ゆかり" },
                    new LotteryModel { Name = "清川望" },
                    new LotteryModel { Name = "鏡魅羅" },
                    new LotteryModel { Name = "朝日奈夕子" },
                    new LotteryModel { Name = "美樹原愛" },
                    new LotteryModel { Name = "早乙女優美" },
                    new LotteryModel { Name = "館林見晴" },
                    new LotteryModel { Name = "伊集院レイ" ,IsSelected = true},                }
            };
            LotteryCategoryModels.Add(model);
        }

        /// <summary>
        /// このページの設定を乱メモ1にする
        /// </summary>
        public void ChangeRanmemo2() {
            Title = "乱メモ2";
            LotteryCategoryModels.Clear();
            LotteryCategoryModel model;
            model = new LotteryCategoryModel() {
                Title = "平日", NumberOfLoops = 10, TotalTimeOfAllLoops = 500, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel(){ Name = "文学" },
                    new LotteryModel(){ Name = "理学" },
                    new LotteryModel(){ Name = "芸術" },
                    new LotteryModel(){ Name = "運動" },
                    new LotteryModel(){ Name = "部活" ,IsSelected = true},
                    new LotteryModel(){ Name = "遊び" },
                    new LotteryModel(){ Name = "容姿" },
                    new LotteryModel(){ Name = "休憩" ,Ratio = 2 },
                }
            };
            LotteryCategoryModels.Add(model);
            model = new LotteryCategoryModel() {
                Title = "休日", NumberOfLoops = 10, TotalTimeOfAllLoops = 500, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel(){ Name = "文学" },
                    new LotteryModel(){ Name = "理学" },
                    new LotteryModel(){ Name = "芸術" },
                    new LotteryModel(){ Name = "運動" },
                    new LotteryModel(){ Name = "部活" ,IsSelected = true},
                    new LotteryModel(){ Name = "遊び" },
                    new LotteryModel(){ Name = "容姿" },
                    new LotteryModel(){ Name = "休憩" },
                    new LotteryModel(){ Name = "電話" ,Ratio = 2},
                }
            };
            LotteryCategoryModels.Add(model);
            model = new LotteryCategoryModel() {
                Title = "攻略対象", NumberOfLoops = 50, TotalTimeOfAllLoops = 5000, LotteryModels = new ObservableCollection<LotteryModel>() {
                    new LotteryModel { Name = "陽ノ下光" },
                    new LotteryModel { Name = "水無月琴子" },
                    new LotteryModel { Name = "寿美幸" },
                    new LotteryModel { Name = "一文字茜" },
                    new LotteryModel { Name = "白雪美帆" },
                    new LotteryModel { Name = "赤井ほむら" },
                    new LotteryModel { Name = "八重花桜梨" },
                    new LotteryModel { Name = "佐倉楓子" },
                    new LotteryModel { Name = "伊集院メイ" },
                    new LotteryModel { Name = "麻生華澄" },
                    new LotteryModel { Name = "白雪真帆" },
                    new LotteryModel { Name = "九段下舞佳" },
                    new LotteryModel { Name = "野咲すみれ" ,IsSelected = true},
                }
            };
            LotteryCategoryModels.Add(model);
        }

    }
}
