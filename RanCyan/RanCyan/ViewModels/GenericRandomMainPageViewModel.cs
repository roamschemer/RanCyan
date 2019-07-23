using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace RanCyan.ViewModels
{
    public class GenericRandomMainPageViewModel : ViewModelBase
    {
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public ReactiveProperty<string> RanCyanKowashiyaImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniKowashiyaRanCyan.png");
        public ReactiveProperty<string> RanCyanMikoImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");
        public ReactiveProperty<string> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.TalkRanCyan.png");
        public ReactiveProperty<string> RanCyanMainImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.RanCyanTaiki2.gif");

        public RandomSet FirstSet { get; } = new RandomSet();
        public RandomSet SecondSet { get; } = new RandomSet();
        public RandomSet ThirdSet { get; } = new RandomSet();
        public RandomSet FourthSet { get;  } = new RandomSet();

        public ReactiveProperty<string> GenericLabel { get; set; } = new ReactiveProperty<string>();

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
            FirstSet.RandomList = new RandomList("GenericFirst", firstItems);

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
            SecondSet.RandomList = new RandomList("GenericSecond", secondItems);

            var thirdItems = new List<Item>()
            {
                new Item { Name = "火の神様" , Ratio=1 },
                new Item { Name = "水の神様" , Ratio=1 },
                new Item { Name = "風の神様" , Ratio=1 },
                new Item { Name = "土の神様" , Ratio=1 },
                new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            };
            ThirdSet.RandomList = new RandomList("GenericThird", thirdItems);

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
            FourthSet.RandomList = new RandomList("Genericfourth", fourthItems);

            var set = new[] { FirstSet, SecondSet, ThirdSet, FourthSet };
            foreach(var s in set)
            {
                s.RandomList.DbDataRead();
                //ViewModel←Model
                s.Items = s.RandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
                s.RanCommand = s.RandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
                s.Label = s.RandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
                s.Color = s.RandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
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
            }
        }
        public override void Destroy()
        {
            this.Disposable.Dispose();
        }
    }
}
