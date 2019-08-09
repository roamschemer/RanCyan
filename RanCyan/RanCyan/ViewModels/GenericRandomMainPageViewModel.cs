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
using Xamarin.Forms;

namespace RanCyan.ViewModels
{
    public class GenericRandomMainPageViewModel : ViewModelBase
    {
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public ReactiveProperty<string> RanCyanKowashiyaImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniKowashiyaRanCyan.png");
        public ReactiveProperty<string> RanCyanMikoImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");
        public ReactiveProperty<string> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.TalkRanCyan.png");
        public ReactiveProperty<string> RanCyanMainImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MainRanCyan.png");

        public RandomSet FirstSet { get; } = new RandomSet();
        public RandomSet SecondSet { get; } = new RandomSet();
        public RandomSet ThirdSet { get; } = new RandomSet();
        public RandomSet FourthSet { get; } = new RandomSet();

        public ReactiveProperty<string> GenericLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Title { get; set; } = new ReactiveProperty<string>();
        public string id;


        public GenericRandomMainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            var firstItems = new List<Item>()
            {
                new Item { Name = "進言 1" , Ratio=1 },
                new Item { Name = "進言 2" , Ratio=1 },
                new Item { Name = "進言 3" , Ratio=1 },
                new Item { Name = "拒否" , Ratio=1 ,IsSelected=true},
                new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            };
            FirstSet.RandomList = new RandomList(firstItems);

            var secondItems = new List<Item>()
            {
                new Item { Name = "剣士" , Ratio=1 },
                new Item { Name = "薙刀士" , Ratio=1 },
                new Item { Name = "弓使い" , Ratio=1 },
                new Item { Name = "槍使い" , Ratio=1 },
                new Item { Name = "拳法家" , Ratio=1 },
                new Item { Name = "壊し屋" , Ratio=1 },
                new Item { Name = "大筒士" , Ratio=1 },
                new Item { Name = "踊り屋" , Ratio=1 },
                new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            };
            SecondSet.RandomList = new RandomList(secondItems);

            var thirdItems = new List<Item>()
            {
                new Item { Name = "火の神様" , Ratio=1 },
                new Item { Name = "水の神様" , Ratio=1 },
                new Item { Name = "風の神様" , Ratio=1 },
                new Item { Name = "土の神様" , Ratio=1 },
                new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            };
            ThirdSet.RandomList = new RandomList(thirdItems);

            var fourthItems = new List<Item>()
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
                new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            };
            FourthSet.RandomList = new RandomList(fourthItems);

            var set = new[] { FirstSet, SecondSet, ThirdSet, FourthSet };
            foreach (var s in set)
            {
                //レートリスト
                s.RatioList = new ObservableCollection<int>(Enumerable.Range(1, 100).ToList());
                s.LoopTimesList = new ObservableCollection<int>(Enumerable.Range(1, 100).ToList());
                s.LoopTotalTimeList = new ObservableCollection<int>(Enumerable.Range(1, 40).Select(x => x * 500).ToList());
                //ViewModel←Model
                s.Items = s.RandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
                //s.RanCommand = s.RandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
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
                s.RanCommand.Subscribe(_ =>
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
        }
        public override void Destroy()
        {
            this.Disposable.Dispose();
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            id = (string)parameters["Id"];
            Title.Value = "乱ちゃんProject" + id;
            var set = new[] { new { rSet = FirstSet, dbPath = "Generic" + id + "First" },
                              new { rSet = SecondSet, dbPath = "Generic" + id + "Second" },
                              new { rSet = ThirdSet, dbPath = "Generic" + id + "Third" },
                              new { rSet = FourthSet, dbPath = "Generic" + id + "Fourth" },
                            };
            foreach (var s in set)
            {
                //DB情報取得
                s.rSet.RandomList.SetDbPath(s.dbPath);
                s.rSet.RandomList.DbDataRead();
            }
        }
    }
}
