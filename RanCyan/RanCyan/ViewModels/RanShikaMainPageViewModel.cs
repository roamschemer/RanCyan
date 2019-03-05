using Prism.Commands;
using Prism.Mvvm;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace RanCyan.ViewModels
{
    public class RanShikaMainPageViewModel : BindableBase
    {
        public ReadOnlyReactiveCollection<Item> ShingenItems { get; }
        public ReactiveCommand<Item> ShingenItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand ShingenRanCommand { get; }
        public RandomList ShingenRandomList { get; }

        public ReadOnlyReactiveCollection<Item> KoushinItems { get; }
        public ReactiveCommand<Item> KoushinItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand KoushinRanCommand { get; }
        public RandomList KoushinRandomList { get; }

        public ReadOnlyReactiveCollection<Item> SyokugyouItems { get; }
        public ReactiveCommand<Item> SyokugyouItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand SyokugyouRanCommand { get; }
        public RandomList SyokugyouRandomList { get; }

        public ReadOnlyReactiveCollection<Item> ToubatsuItems { get; }
        public ReactiveCommand<Item> ToubatsuItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand ToubatsuRanCommand { get; }
        public RandomList ToubatsuRandomList { get; }

        public ReactiveProperty<String> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniKowashiyaRanCyan.png");

        public RanShikaMainPageViewModel()
        {
            var shingenItems = new List<Item>()
            {
                new Item { Name = "進言 1" , Ratio=1 },
                new Item { Name = "進言 2" , Ratio=1 },
                new Item { Name = "進言 3" , Ratio=1 },
            };
            RandomList shingenRandomList = new RandomList(shingenItems);
            var koushinItems = new List<Item>()
            {
                new Item { Name = "火" , Ratio=1 },
                new Item { Name = "水" , Ratio=1 },
                new Item { Name = "風" , Ratio=1 },
                new Item { Name = "土" , Ratio=1 },
            };
            RandomList koushinRandomList = new RandomList(koushinItems, 20, 5000);
            var syokugyouItems = new List<Item>()
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
            RandomList syokugyouRandomList = new RandomList(syokugyouItems, 20, 5000);
            var toubatsuItems = new List<Item>()
            {
                new Item { Name = "鳥居" , Ratio=1 },
                new Item { Name = "相翼院" , Ratio=1 },
                new Item { Name = "九重楼" , Ratio=1 },
                new Item { Name = "白骨城" , Ratio=1 },
                new Item { Name = "流水道" , Ratio=1 },
                new Item { Name = "鎮魂墓" , Ratio=1 },
                new Item { Name = "紅蓮" , Ratio=1 },
                new Item { Name = "大江山" , Ratio=1 },
                new Item { Name = "地獄" , Ratio=1 },
            };
            RandomList toubatsuRandomList = new RandomList(toubatsuItems, 20, 5000);


            //ViewModel←Model
            ShingenItems = shingenRandomList.Items.ToReadOnlyReactiveCollection();
            ShingenRanCommand = shingenRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand();
            KoushinItems = koushinRandomList.Items.ToReadOnlyReactiveCollection();
            KoushinRanCommand = koushinRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand();
            SyokugyouItems = syokugyouRandomList.Items.ToReadOnlyReactiveCollection();
            SyokugyouRanCommand = syokugyouRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand();
            ToubatsuItems = toubatsuRandomList.Items.ToReadOnlyReactiveCollection();
            ToubatsuRanCommand = toubatsuRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand();

            //Button
            ShingenItemTapped.Where(_ => !shingenRandomList.InRundom).Subscribe(x => x.IsSelected = !x.IsSelected);
            ShingenRanCommand.Subscribe(_ => shingenRandomList.RandomAction());
            KoushinItemTapped.Where(_ => !koushinRandomList.InRundom).Subscribe(x => x.IsSelected = !x.IsSelected);
            KoushinRanCommand.Subscribe(_ => koushinRandomList.RandomAction());
            SyokugyouItemTapped.Where(_ => !syokugyouRandomList.InRundom).Subscribe(x => x.IsSelected = !x.IsSelected);
            SyokugyouRanCommand.Subscribe(_ => syokugyouRandomList.RandomAction());
            ToubatsuItemTapped.Where(_ => !toubatsuRandomList.InRundom).Subscribe(x => x.IsSelected = !x.IsSelected);
            ToubatsuRanCommand.Subscribe(_ => toubatsuRandomList.RandomAction());

        }
    }
}
