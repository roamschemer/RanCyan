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

namespace RanCyan.ViewModels
{
    public class GenericRandomPageViewModel : ViewModelBase
    {
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public ReactiveProperty<string> RanCyanKowashiyaImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniKowashiyaRanCyan.png");
        public ReactiveProperty<string> RanCyanMikoImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");
        public ReactiveProperty<string> RanCyanMainImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MainRanCyan.png");
        public ReactiveProperty<string> TalkRanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.TalkRanCyan.png");

        public RandomSet FirstSet { get; } = new RandomSet();
        public RandomSet SecondSet { get; } = new RandomSet();
        public RandomSet ThirdSet { get; } = new RandomSet();
        public RandomSet FourthSet { get; } = new RandomSet();

        public ReactiveProperty<string> Title { get; set; } = new ReactiveProperty<string>();

        private string id;

        public ReactiveCommand<string> InitializationCommand { get; set; } = new ReactiveCommand<string>();

        public GenericRandomPageViewModel(INavigationService navigationService) : base(navigationService)
        {

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
                timer1.Subscribe(x =>
                {
                    int seed = Environment.TickCount;
                    Random rnd = new System.Random(seed);
                    RanCyanMainImage.Value = ranCyanImageItems[rnd.Next(ranCyanImageItems.Count)];
                });
                timer1.Start();
                timer2.Subscribe(x =>
                {
                    timer1.Start(TimeSpan.FromSeconds(0));
                    timer2.Stop();
                });
            }

            foreach (var s in set)
            {
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
                s.ItemTapped.Where(_ => !s.RandomList.InRundom).Subscribe(x =>
                {
                    x.IsSelected = !x.IsSelected;
                    s.RandomList.DBDataWrite();
                });
                s.RanCommand.Where(_ => !s.RandomList.InRundom).Subscribe(_ =>
                {
                    if (Device.RuntimePlatform == Device.UWP) //WPFはGifが動かない
                    {
                        RanCyanMainImage.Value = "RanCyan.Images.3D_Jamp1.gif";
                        timer2.Start(TimeSpan.FromSeconds(2.5));
                        timer1.Stop();
                    }
                    set.Select(x => x.IsVisible.Value = false).ToList();
                    s.IsVisible.Value = true;
                    s.RandomList.RandomAction();
                });
                s.SelectedItem.Where(x => x != null).Subscribe(x =>
                {
                    s.InputName.Value = x.Name;
                    s.InputRatio.Value = x.Ratio;
                });
                s.InsertTapped.Subscribe(_ => s.RandomList.Insert(s.InputName.Value, s.InputRatio.Value));
                s.UpDateTapped.Subscribe(_ => s.RandomList.UpDate(s.SelectedItem.Value, s.InputName.Value, s.InputRatio.Value));
                s.DeleteTapped.Subscribe(_ => s.RandomList.Delete(s.SelectedItem.Value));
                s.AllDeleteTapped.Subscribe(async _ =>
                {
                    var select = await Application.Current.MainPage.DisplayAlert("全消去", "リストを全て消去するけど構わない？", "いいよ", "待った");
                    if (select) s.RandomList.AllDelete();
                });

            }
            InitializationCommand.Subscribe(async x => await InitializationAsync(x));
        }
        public override void Destroy()
        {
            Thread.Sleep(100);
            this.Disposable.Dispose();
        }
        public async Task InitializationAsync(string ranType)
        {
            var select = await Application.Current.MainPage.DisplayAlert("初期化", ranType + "に初期化するけど構わない？", "いいよ", "待った");
            if (select)
            {
                if (ranType == "乱屍")
                {
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
                        new Item { Name = "白骨城" , Ratio=1 },
                        new Item { Name = "流水道" , Ratio=1 },
                        new Item { Name = "鎮魂墓" , Ratio=1 },
                        new Item { Name = "紅蓮" , Ratio=1 },
                        new Item { Name = "大江山" , Ratio=1 ,IsSelected=true },
                        new Item { Name = "地獄" , Ratio=1 ,IsSelected=true },
                    };
                    FourthSet.RandomList.Initialization(items);
                    FourthSet.LoopTimes.Value = 40;
                    FourthSet.LoopTotalTime.Value = 10000;
                    FourthSet.RanCommandButtonText.Value = "討伐先";
                }
                if (ranType == "乱メモ")
                {
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
                    items = new ObservableCollection<Item>();
                    FourthSet.RandomList.Initialization(items);
                    FourthSet.LoopTimes.Value = 10;
                    FourthSet.LoopTotalTime.Value = 1000;
                    FourthSet.RanCommandButtonText.Value = "-";
                }
                if (ranType == "サイコロ")
                {
                    ObservableCollection<Item> items;
                    items = new ObservableCollection<Item>();
                    foreach (var i in Enumerable.Range(1, 6))
                    {
                        items.Add(new Item { Name = i.ToString(), Ratio = 1 });
                    }
                    FirstSet.RandomList.Initialization(items);
                    FirstSet.LoopTimes.Value = 10;
                    FirstSet.LoopTotalTime.Value = 1000;
                    FirstSet.RanCommandButtonText.Value = "6マス";
                    items = new ObservableCollection<Item>();
                    foreach (var i in Enumerable.Range(1, 8))
                    {
                        items.Add(new Item { Name = i.ToString(), Ratio = 1 });
                    }
                    SecondSet.RandomList.Initialization(items);
                    SecondSet.LoopTimes.Value = 10;
                    SecondSet.LoopTotalTime.Value = 1000;
                    SecondSet.RanCommandButtonText.Value = "8マス";
                    items = new ObservableCollection<Item>();
                    foreach (var i in Enumerable.Range(1, 12))
                    {
                        items.Add(new Item { Name = i.ToString(), Ratio = 1 });
                    }
                    ThirdSet.RandomList.Initialization(items);
                    ThirdSet.LoopTimes.Value = 10;
                    ThirdSet.LoopTotalTime.Value = 1000;
                    ThirdSet.RanCommandButtonText.Value = "12マス";
                    items = new ObservableCollection<Item>();
                    foreach (var i in Enumerable.Range(1, 20))
                    {
                        items.Add(new Item { Name = i.ToString(), Ratio = 1 });
                    }
                    FourthSet.RandomList.Initialization(items);
                    FourthSet.LoopTimes.Value = 10;
                    FourthSet.LoopTotalTime.Value = 1000;
                    FourthSet.RanCommandButtonText.Value = "20マス";
                }
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            id = (string)parameters["Id"];
            Title.Value = "乱ちゃんProject" + id;
            var set = new[] { new { rSet = FirstSet, buttonText="", dbPath = "Generic" + id + "First" },
                              new { rSet = SecondSet, buttonText="", dbPath = "Generic" + id + "Second" },
                              new { rSet = ThirdSet, buttonText="", dbPath = "Generic" + id + "Third" },
                              new { rSet = FourthSet, buttonText="", dbPath = "Generic" + id + "Fourth" },
                            };
            foreach (var s in set)
            {
                //DB情報取得
                s.rSet.RandomList.SetStartInfo(s.dbPath, s.buttonText);
                s.rSet.RandomList.DbDataRead();
            }
        }
    }
}
