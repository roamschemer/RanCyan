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
	public class DiceMainPageViewModel : BindableBase
	{
        public ReactiveProperty<String> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");

        public ReadOnlyReactiveCollection<Item> DiceItems { get; }
        public ReactiveCommand<Item> DiceItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand DiceCommand { get; }
        public ReactiveProperty<String> DiceLabel { get; set; } = new ReactiveProperty<string>();

        public DiceMainPageViewModel()
        {
            var diceItems = new List<Item>()
            {
                new Item { Name = "1" , Ratio=1 },
                new Item { Name = "2" , Ratio=1 },
                new Item { Name = "3" , Ratio=1 },
                new Item { Name = "4" , Ratio=1 },
                new Item { Name = "5" , Ratio=1 },
                new Item { Name = "6" , Ratio=1 },
                new Item { Name = "7" , Ratio=1 ,IsSelected=true},
                new Item { Name = "8" , Ratio=1 ,IsSelected=true},
                new Item { Name = "9" , Ratio=1 ,IsSelected=true},
                new Item { Name = "10" , Ratio=1 ,IsSelected=true},
                new Item { Name = "11" , Ratio=1 ,IsSelected=true},
                new Item { Name = "12" , Ratio=1 ,IsSelected=true},
                new Item { Name = "13" , Ratio=1 ,IsSelected=true},
                new Item { Name = "14" , Ratio=1 ,IsSelected=true},
                new Item { Name = "15" , Ratio=1 ,IsSelected=true},
                new Item { Name = "16" , Ratio=1 ,IsSelected=true},
                new Item { Name = "17" , Ratio=1 ,IsSelected=true},
                new Item { Name = "18" , Ratio=1 ,IsSelected=true},
                new Item { Name = "19" , Ratio=1 ,IsSelected=true},
                new Item { Name = "20" , Ratio=1 ,IsSelected=true},
                new Item { Name = "21" , Ratio=1 ,IsSelected=true},
                new Item { Name = "22" , Ratio=1 ,IsSelected=true},
                new Item { Name = "23" , Ratio=1 ,IsSelected=true},
                new Item { Name = "24" , Ratio=1 ,IsSelected=true},
                new Item { Name = "25" , Ratio=1 ,IsSelected=true},
                new Item { Name = "26" , Ratio=1 ,IsSelected=true},
                new Item { Name = "27" , Ratio=1 ,IsSelected=true},
                new Item { Name = "28" , Ratio=1 ,IsSelected=true},
                new Item { Name = "29" , Ratio=1 ,IsSelected=true},
                new Item { Name = "30" , Ratio=1 ,IsSelected=true},
            };
            RandomList diceRandomList = new RandomList(diceItems);

            //ViewModel←Model
            DiceItems = diceRandomList.Items.ToReadOnlyReactiveCollection();
            DiceCommand = diceRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand();
            DiceLabel = diceRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty();

            //Button
            DiceItemTapped.Where(_ => !diceRandomList.InRundom).Subscribe(x => x.IsSelected = !x.IsSelected);
            DiceCommand.Subscribe(_ => diceRandomList.RandomAction());
        }
    }
}
