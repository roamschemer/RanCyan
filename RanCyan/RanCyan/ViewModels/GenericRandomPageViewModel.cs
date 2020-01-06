using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RanCyan.ViewModels {
    public class GenericRandomPageViewModel : ViewModelBase {
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public ReactiveProperty<string> RanCyanKowashiyaImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniKowashiyaRanCyan.png");
        public ReactiveProperty<string> RanCyanMikoImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");
        public ReactiveProperty<string> RanCyanMainImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MainRanCyan.png");
        public ReactiveProperty<string> TalkRanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.TalkRanCyan.png");

        public RandomSet FirstSet { get; } = new RandomSet();
        public RandomSet SecondSet { get; } = new RandomSet();
        public RandomSet ThirdSet { get; } = new RandomSet();
        public RandomSet FourthSet { get; } = new RandomSet();

        public PageInfomation PageInfo { get; set; } = new PageInfomation(0);
        public AllPageInfomation AllPageInfo { get; } = new AllPageInfomation();

        public ReactiveProperty<string> Title { get; set; } = new ReactiveProperty<string>();

        private string id;

        public ReactiveCommand<string> InitializationCommand { get; set; } = new ReactiveCommand<string>();
        public ReactiveProperty<int> ImageGridWidth { get; set; } = new ReactiveProperty<int>(180);
        public ReactiveProperty<int> ImageMinimumWidth { get; set; } = new ReactiveProperty<int>(90);

        public ReactiveProperty<string> ImageBackColor { get; set; } = new ReactiveProperty<string>("White");
        public ObservableCollection<string> ImageBackColorList { get; set; }

        public GenericRandomPageViewModel(INavigationService navigationService) : base(navigationService) {

            ImageBackColorList = new ObservableCollection<string> { "White", "Blue", "DodgerBlue", "CornflowerBlue", "Chartreuse", "ForestGreen", "Yellow" };
            FirstSet.RandomList = new RandomList(new ObservableCollection<Item>());
            SecondSet.RandomList = new RandomList(new ObservableCollection<Item>());
            ThirdSet.RandomList = new RandomList(new ObservableCollection<Item>());
            FourthSet.RandomList = new RandomList(new ObservableCollection<Item>());
            var set = new[] { FirstSet, SecondSet, ThirdSet, FourthSet };

            var ranCyanImageItems = new List<string>
            {
                "RanCyan.Images.3D_Taiki2.gif",
                "RanCyan.Images.3D_Taiki5.gif",
                "RanCyan.Images.3D_Taiki6.gif",
                "RanCyan.Images.3D_Taiki7.gif",
                "RanCyan.Images.3D_Taiki8.gif",
            };
            var timer1 = new ReactiveTimer(TimeSpan.FromSeconds(60)); // 秒スパンのタイマー
            var timer2 = new ReactiveTimer(TimeSpan.FromSeconds(3)); // 秒スパンのタイマー

            if (Device.RuntimePlatform == Device.UWP) //WPFはGifが動かない
            {
                timer1.Subscribe(x => {
                    int seed = Environment.TickCount;
                    Random rnd = new System.Random(seed);
                    RanCyanMainImage.Value = ranCyanImageItems[rnd.Next(ranCyanImageItems.Count)];
                });
                timer1.Start();
                timer2.Subscribe(x => {
                    timer1.Start(TimeSpan.FromSeconds(0));
                    timer2.Stop();
                });
            }

            //ページ情報
            Title = PageInfo.ToReactivePropertyAsSynchronized(x => x.PageTitle).AddTo(this.Disposable);

            foreach (var s in set) {
                //レートリスト
                s.RatioList = new ObservableCollection<int>(Enumerable.Range(1, 100).ToList());
                s.LoopTimesList = new ObservableCollection<int>(Enumerable.Range(1, 100).ToList());
                s.LoopTotalTimeList = new ObservableCollection<int>(Enumerable.Range(1, 40).Select(x => x * 500).ToList());
                //ViewModel←Model
                s.Items = s.RandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
                s.Label = s.RandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
                s.Color = s.RandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
                //ViewModel←→Model
                s.LoopTimes = s.RandomList.ToReactivePropertyAsSynchronized(x => x.LoopTimes).AddTo(this.Disposable);
                s.LoopTotalTime = s.RandomList.ToReactivePropertyAsSynchronized(x => x.LoopTotalTime).AddTo(this.Disposable);
                s.RanCommandButtonText = s.RandomList.ToReactivePropertyAsSynchronized(x => x.RanCommandButtonText).AddTo(this.Disposable);
                //Button
                s.ItemTapped.Where(_ => !s.RandomList.InRundom).Subscribe(x => {
                    x.IsSelected = !x.IsSelected;
                    s.RandomList.DBDataWrite();
                });
                s.RanCommand.Where(_ => !s.RandomList.InRundom).Subscribe(_ => {
                    if (Device.RuntimePlatform == Device.UWP) //WPFはGifが動かない
                    {
                        RanCyanMainImage.Value = "RanCyan.Images.3D_Jamp1.gif";
                        timer2.Start(TimeSpan.FromSeconds(4));
                        timer1.Stop();
                    }
                    set.Select(x => x.IsVisible.Value = false).ToList();
                    s.IsVisible.Value = true;
                    s.RandomList.RandomAction();
                });
                s.SelectedItem.Where(x => x != null).Subscribe(x => {
                    s.InputName.Value = x.Name;
                    s.InputRatio.Value = x.Ratio;
                });
                s.InsertTapped.Subscribe(_ => s.RandomList.Insert(s.InputName.Value, s.InputRatio.Value));
                s.UpDateTapped.Subscribe(_ => s.RandomList.UpDate(s.SelectedItem.Value, s.InputName.Value, s.InputRatio.Value));
                s.DeleteTapped.Subscribe(_ => s.RandomList.Delete(s.SelectedItem.Value));
                s.AllDeleteTapped.Subscribe(async _ => {
                    var select = await Application.Current.MainPage.DisplayAlert("全消去", "リストを全て消去するけど構わない？", "いいよ", "待った");
                    if (select) s.RandomList.AllDelete();
                });

            }
            InitializationCommand.Subscribe(async x => await InitializationAsync(x));
        }
        public override void Destroy() {
            Thread.Sleep(100);
            this.Disposable.Dispose();
        }
        public async Task InitializationAsync(string ranType) {
            var select = await Application.Current.MainPage.DisplayAlert("初期化", ranType + "に初期化するけど構わない？", "いいよ", "待った");
            if (select) {
                Title.Value = ranType;
                if (ranType == "乱屍") {
                    ObservableCollection<Item> items;
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "進言 1" , Ratio=1 },
                        new Item { Name = "進言 2" , Ratio=1 },
                        new Item { Name = "進言 3" , Ratio=1 },
                    };
                    FirstSet.RandomList.Initialization(items);
                    FirstSet.LoopTimes.Value = 10;
                    FirstSet.LoopTotalTime.Value = 1000;
                    FirstSet.RanCommandButtonText.Value = "進言";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "剣士" , Ratio=1 },
                        new Item { Name = "薙刀士" , Ratio=1 },
                        new Item { Name = "弓使い" , Ratio=1 },
                        new Item { Name = "槍使い" , Ratio=1 },
                        new Item { Name = "拳法家" , Ratio=1 },
                        new Item { Name = "壊し屋" , Ratio=1 },
                        new Item { Name = "大筒士" , Ratio=1 },
                        new Item { Name = "踊り屋" , Ratio=1 },
                    };
                    SecondSet.RandomList.Initialization(items);
                    SecondSet.LoopTimes.Value = 40;
                    SecondSet.LoopTotalTime.Value = 10000;
                    SecondSet.RanCommandButtonText.Value = "職業";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "火の神様" , Ratio=1 },
                        new Item { Name = "水の神様" , Ratio=1 },
                        new Item { Name = "風の神様" , Ratio=1 },
                        new Item { Name = "土の神様" , Ratio=1 },
                    };
                    ThirdSet.RandomList.Initialization(items);
                    ThirdSet.LoopTimes.Value = 40;
                    ThirdSet.LoopTotalTime.Value = 10000;
                    ThirdSet.RanCommandButtonText.Value = "交神";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "鳥居" , Ratio=1 },
                        new Item { Name = "相翼院" , Ratio=1 },
                        new Item { Name = "九重楼" , Ratio=1 },
                        new Item { Name = "白骨城" , Ratio=1 ,IsSelected=true},
                        new Item { Name = "流水道" , Ratio=1 ,IsSelected=true},
                        new Item { Name = "鎮魂墓" , Ratio=1 ,IsSelected=true},
                        new Item { Name = "紅蓮" , Ratio=1 ,IsSelected=true},
                        new Item { Name = "大江山" , Ratio=1 ,IsSelected=true },
                        new Item { Name = "地獄" , Ratio=1 ,IsSelected=true },
                    };
                    FourthSet.RandomList.Initialization(items);
                    FourthSet.LoopTimes.Value = 40;
                    FourthSet.LoopTotalTime.Value = 10000;
                    FourthSet.RanCommandButtonText.Value = "討伐先";
                }
                if (ranType == "乱メモ1") {
                    ObservableCollection<Item> items;
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "文学" , Ratio=1 },
                        new Item { Name = "理学" , Ratio=1 },
                        new Item { Name = "芸術" , Ratio=1 },
                        new Item { Name = "運動" , Ratio=1 },
                        new Item { Name = "部活" , Ratio=1 },
                        new Item { Name = "遊び" , Ratio=1 },
                        new Item { Name = "容姿" , Ratio=1 },
                        new Item { Name = "休憩" , Ratio=2 },
                    };
                    FirstSet.RandomList.Initialization(items);
                    FirstSet.LoopTimes.Value = 10;
                    FirstSet.LoopTotalTime.Value = 1000;
                    FirstSet.RanCommandButtonText.Value = "平日";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "文学" , Ratio=1 },
                        new Item { Name = "理学" , Ratio=1 },
                        new Item { Name = "芸術" , Ratio=1 },
                        new Item { Name = "運動" , Ratio=1 },
                        new Item { Name = "部活" , Ratio=1 },
                        new Item { Name = "遊び" , Ratio=1 },
                        new Item { Name = "容姿" , Ratio=1 },
                        new Item { Name = "休憩" , Ratio=1 },
                        new Item { Name = "電話" , Ratio=2 },
                    };
                    SecondSet.RandomList.Initialization(items);
                    SecondSet.LoopTimes.Value = 10;
                    SecondSet.LoopTotalTime.Value = 1000;
                    SecondSet.RanCommandButtonText.Value = "休日";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "了承" , Ratio=1 },
                        new Item { Name = "却下" , Ratio=1 },
                    };
                    ThirdSet.RandomList.Initialization(items);
                    ThirdSet.LoopTimes.Value = 10;
                    ThirdSet.LoopTotalTime.Value = 1000;
                    ThirdSet.RanCommandButtonText.Value = "下校判定";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "藤崎詩織" , Ratio=1 },
                        new Item { Name = "如月未緒" , Ratio=1 },
                        new Item { Name = "紐緒結奈" , Ratio=1 },
                        new Item { Name = "片桐彩子" , Ratio=1 },
                        new Item { Name = "虹野沙希" , Ratio=1 },
                        new Item { Name = "古式ゆかり" , Ratio=1 },
                        new Item { Name = "清川望" , Ratio=1 },
                        new Item { Name = "鏡魅羅" , Ratio=1 },
                        new Item { Name = "朝日奈夕子" , Ratio=1 },
                        new Item { Name = "美樹原愛" , Ratio=1 },
                        new Item { Name = "早乙女優美" , Ratio=1 },
                        new Item { Name = "館林見晴" , Ratio=1 },
                        new Item { Name = "伊集院レイ" , Ratio=1 ,IsSelected=true},
                    };
                    FourthSet.RandomList.Initialization(items);
                    FourthSet.LoopTimes.Value = 40;
                    FourthSet.LoopTotalTime.Value = 10000;
                    FourthSet.RanCommandButtonText.Value = "攻略対象";
                }
                if (ranType == "乱メモ2") {
                    ObservableCollection<Item> items;
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "文学" , Ratio=1 },
                        new Item { Name = "理学" , Ratio=1 },
                        new Item { Name = "芸術" , Ratio=1 },
                        new Item { Name = "運動" , Ratio=1 },
                        new Item { Name = "部活" , Ratio=1 },
                        new Item { Name = "遊び" , Ratio=1 },
                        new Item { Name = "容姿" , Ratio=1 },
                        new Item { Name = "休憩" , Ratio=2 },
                    };
                    FirstSet.RandomList.Initialization(items);
                    FirstSet.LoopTimes.Value = 10;
                    FirstSet.LoopTotalTime.Value = 1000;
                    FirstSet.RanCommandButtonText.Value = "平日";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "文学" , Ratio=1 },
                        new Item { Name = "理学" , Ratio=1 },
                        new Item { Name = "芸術" , Ratio=1 },
                        new Item { Name = "運動" , Ratio=1 },
                        new Item { Name = "部活" , Ratio=1 },
                        new Item { Name = "遊び" , Ratio=1 },
                        new Item { Name = "容姿" , Ratio=1 },
                        new Item { Name = "休憩" , Ratio=1 },
                        new Item { Name = "電話" , Ratio=2 },
                    };
                    SecondSet.RandomList.Initialization(items);
                    SecondSet.LoopTimes.Value = 10;
                    SecondSet.LoopTotalTime.Value = 1000;
                    SecondSet.RanCommandButtonText.Value = "休日";
                    items = new ObservableCollection<Item>();
                    ThirdSet.RandomList.Initialization(items);
                    ThirdSet.LoopTimes.Value = 10;
                    ThirdSet.LoopTotalTime.Value = 1000;
                    ThirdSet.RanCommandButtonText.Value = "-";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "陽ノ下光" , Ratio=1 },
                        new Item { Name = "水無月琴子" , Ratio=1 },
                        new Item { Name = "寿美幸" , Ratio=1 },
                        new Item { Name = "一文字茜" , Ratio=1 },
                        new Item { Name = "白雪美帆" , Ratio=1 },
                        new Item { Name = "赤井ほむら" , Ratio=1 },
                        new Item { Name = "八重花桜梨" , Ratio=1 },
                        new Item { Name = "佐倉楓子" , Ratio=1 },
                        new Item { Name = "伊集院メイ" , Ratio=1 },
                        new Item { Name = "麻生華澄" , Ratio=1 },
                        new Item { Name = "白雪真帆" , Ratio=1 },
                        new Item { Name = "九段下舞佳" , Ratio=1 },
                        new Item { Name = "野咲すみれ" , Ratio=1 ,IsSelected=true},
                    };
                    FourthSet.RandomList.Initialization(items);
                    FourthSet.LoopTimes.Value = 40;
                    FourthSet.LoopTotalTime.Value = 10000;
                    FourthSet.RanCommandButtonText.Value = "攻略対象";
                }
                if (ranType == "サイコロ") {
                    ObservableCollection<Item> items;
                    items = new ObservableCollection<Item>();
                    foreach (var i in Enumerable.Range(1, 6)) {
                        items.Add(new Item { Name = i.ToString(), Ratio = 1 });
                    }
                    FirstSet.RandomList.Initialization(items);
                    FirstSet.LoopTimes.Value = 10;
                    FirstSet.LoopTotalTime.Value = 1000;
                    FirstSet.RanCommandButtonText.Value = "6マス";
                    items = new ObservableCollection<Item>();
                    foreach (var i in Enumerable.Range(1, 8)) {
                        items.Add(new Item { Name = i.ToString(), Ratio = 1 });
                    }
                    SecondSet.RandomList.Initialization(items);
                    SecondSet.LoopTimes.Value = 10;
                    SecondSet.LoopTotalTime.Value = 1000;
                    SecondSet.RanCommandButtonText.Value = "8マス";
                    items = new ObservableCollection<Item>();
                    foreach (var i in Enumerable.Range(1, 12)) {
                        items.Add(new Item { Name = i.ToString(), Ratio = 1 });
                    }
                    ThirdSet.RandomList.Initialization(items);
                    ThirdSet.LoopTimes.Value = 10;
                    ThirdSet.LoopTotalTime.Value = 1000;
                    ThirdSet.RanCommandButtonText.Value = "12マス";
                    items = new ObservableCollection<Item>();
                    foreach (var i in Enumerable.Range(1, 20)) {
                        items.Add(new Item { Name = i.ToString(), Ratio = 1 });
                    }
                    FourthSet.RandomList.Initialization(items);
                    FourthSet.LoopTimes.Value = 10;
                    FourthSet.LoopTotalTime.Value = 1000;
                    FourthSet.RanCommandButtonText.Value = "20マス";
                }
                if (ranType == "乱サガ３") {
                    ObservableCollection<Item> items;
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "ユリアン" , Ratio=1 },
                        new Item { Name = "エレン" , Ratio=1 },
                        new Item { Name = "サラ" , Ratio=1 },
                        new Item { Name = "トーマス" , Ratio=1 },
                        new Item { Name = "ハリード" , Ratio=1 },
                        new Item { Name = "ミカエル" , Ratio=1 },
                        new Item { Name = "モニカ" , Ratio=1 },
                        new Item { Name = "カタリナ" , Ratio=1 },
                        new Item { Name = "レオニード" , Ratio=1 },
                        new Item { Name = "少年" , Ratio=1 },
                        new Item { Name = "ティベリウス" , Ratio=1 },
                        new Item { Name = "ウォード" , Ratio=1 },
                        new Item { Name = "ポール" , Ratio=1 },
                        new Item { Name = "ロビン(細)" , Ratio=1 },
                        new Item { Name = "ロビン(太)" , Ratio=1 },
                        new Item { Name = "ミューズ" , Ratio=1 },
                        new Item { Name = "シャール" , Ratio=1 },
                        new Item { Name = "詩人" , Ratio=1 },
                        new Item { Name = "タチアナ" , Ratio=1 },
                        new Item { Name = "ヤンファン" , Ratio=1 },
                        new Item { Name = "ウンディーネ" , Ratio=1 },
                        new Item { Name = "ツィーリン" , Ratio=1 },
                        new Item { Name = "ハーマン" , Ratio=1 },
                        new Item { Name = "フルブライト" , Ratio=1 },
                        new Item { Name = "バイメイニャン" , Ratio=1 },
                        new Item { Name = "ノーラ" , Ratio=1 },
                        new Item { Name = "ブラック" , Ratio=1 },
                        new Item { Name = "ようせい" , Ratio=1 },
                        new Item { Name = "ボストン" , Ratio=1 },
                        new Item { Name = "ぞう" , Ratio=1 },
                        new Item { Name = "ゆきだるま" , Ratio=1 },
                    };
                    FirstSet.RandomList.Initialization(items);
                    FirstSet.LoopTimes.Value = 20;
                    FirstSet.LoopTotalTime.Value = 5000;
                    FirstSet.RanCommandButtonText.Value = "キャラ";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "剣" , Ratio=1 },
                        new Item { Name = "大剣" , Ratio=1 },
                        new Item { Name = "斧" , Ratio=1 },
                        new Item { Name = "棍棒" , Ratio=1 },
                        new Item { Name = "小剣" , Ratio=1 },
                        new Item { Name = "槍" , Ratio=1 },
                        new Item { Name = "弓" , Ratio=1 },
                        new Item { Name = "素手" , Ratio=1 },
                        new Item { Name = "術" , Ratio=1 },
                    };
                    SecondSet.RandomList.Initialization(items);
                    SecondSet.LoopTimes.Value = 20;
                    SecondSet.LoopTotalTime.Value = 5000;
                    SecondSet.RanCommandButtonText.Value = "武器";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "歳星(狩人)" , Ratio=1 },
                        new Item { Name = "螢惑(学者)" , Ratio=1 },
                        new Item { Name = "鎮星(王者)" , Ratio=1 },
                        new Item { Name = "太白(武人)" , Ratio=1 },
                        new Item { Name = "辰星(商人)" , Ratio=1 },
                    };
                    ThirdSet.RandomList.Initialization(items);
                    ThirdSet.LoopTimes.Value = 20;
                    ThirdSet.LoopTotalTime.Value = 5000;
                    ThirdSet.RanCommandButtonText.Value = "宿星";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "フリーファイト" , Ratio=1 },
                        new Item { Name = "スペキュレイション" , Ratio=1 },
                        new Item { Name = "ワールウィンド" , Ratio=1 },
                        new Item { Name = "鳳天舞の陣" , Ratio=1 },
                        new Item { Name = "玄武陣" , Ratio=1 },
                        new Item { Name = "パワーレイズ" , Ratio=1 },
                        new Item { Name = "龍陣" , Ratio=1 },
                        new Item { Name = "トライアンカー" , Ratio=1  },
                        new Item { Name = "虎穴陣" , Ratio=1 },
                        new Item { Name = "ハンターシフト" , Ratio=1 },
                        new Item { Name = "デザートランス" , Ratio=1 },
                    };
                    FourthSet.RandomList.Initialization(items);
                    FourthSet.LoopTimes.Value = 40;
                    FourthSet.LoopTotalTime.Value = 10000;
                    FourthSet.RanCommandButtonText.Value = "陣形";
                }
            }
        }
        /// <summary>
        /// 遷移してきて最初に通るとこ
        /// </summary>
        /// <param name="parameters"></param>
        public override void OnNavigatedTo(INavigationParameters parameters) {
            id = (string)parameters["Id"];
            var set = new[] { new { rSet = FirstSet, buttonText="", dbPath = "Generic" + id + "First" },
                              new { rSet = SecondSet, buttonText="", dbPath = "Generic" + id + "Second" },
                              new { rSet = ThirdSet, buttonText="", dbPath = "Generic" + id + "Third" },
                              new { rSet = FourthSet, buttonText="", dbPath = "Generic" + id + "Fourth" },
                            };
            foreach (var s in set) {
                //DB情報取得
                s.rSet.RandomList.SetStartInfo(s.dbPath, s.buttonText);
                s.rSet.RandomList.DbDataRead();
            }
            //ページ設定
            PageInfo.Number = int.Parse(id);
        }
    }
}
