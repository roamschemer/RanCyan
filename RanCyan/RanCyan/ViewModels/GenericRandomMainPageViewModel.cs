﻿using Prism.Commands;
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
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RanCyan.ViewModels
{
    public class GenericRandomMainPageViewModel : ViewModelBase
    {
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public ReactiveProperty<string> RanCyanKowashiyaImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniKowashiyaRanCyan.png");
        public ReactiveProperty<string> RanCyanMikoImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");
        public ReactiveProperty<string> RanCyanMainImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MainRanCyan.png");

        public RandomSet FirstSet { get; } = new RandomSet();
        public RandomSet SecondSet { get; } = new RandomSet();
        public RandomSet ThirdSet { get; } = new RandomSet();
        public RandomSet FourthSet { get; } = new RandomSet();

        public ReactiveProperty<string> GenericLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Title { get; set; } = new ReactiveProperty<string>();

        private string id;

        public ReactiveCommand<string> InitializationCommand { get; set; } = new ReactiveCommand<string>();

        public GenericRandomMainPageViewModel(INavigationService navigationService) : base(navigationService)
        {

            FirstSet.RandomList = new RandomList(new ObservableCollection<Item>());
            SecondSet.RandomList = new RandomList(new ObservableCollection<Item>());
            ThirdSet.RandomList = new RandomList(new ObservableCollection<Item>());
            FourthSet.RandomList = new RandomList(new ObservableCollection<Item>());

            var ranCyanImageItems = new List<string>
            {
                "RanCyan.Images.MainRanCyan.png",
                "RanCyan.Images.MiniKowashiyaRanCyan.png",
                "RanCyan.Images.MiniMikoRanCyan.png",
            };
            var timer = new ReactiveTimer(TimeSpan.FromSeconds(1)); // 秒スパンのタイマー
            timer.Subscribe(x => {
                int seed = Environment.TickCount;
                Random rnd = new System.Random(seed);
                RanCyanMainImage.Value = ranCyanImageItems[rnd.Next(ranCyanImageItems.Count)];
            });
            timer.Start();

            var set = new[] { FirstSet, SecondSet, ThirdSet, FourthSet };
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
                    SecondSet.LoopTimes.Value = 20;
                    SecondSet.LoopTotalTime.Value = 5000;
                    SecondSet.RanCommandButtonText.Value = "職業";
                    items = new ObservableCollection<Item>()
                    {
                        new Item { Name = "火の神様" , Ratio=1 },
                        new Item { Name = "水の神様" , Ratio=1 },
                        new Item { Name = "風の神様" , Ratio=1 },
                        new Item { Name = "土の神様" , Ratio=1 },
                    };
                    ThirdSet.RandomList.Initialization(items);
                    ThirdSet.LoopTimes.Value = 20;
                    ThirdSet.LoopTotalTime.Value = 5000;
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
                    FourthSet.LoopTimes.Value = 20;
                    FourthSet.LoopTotalTime.Value = 5000;
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
