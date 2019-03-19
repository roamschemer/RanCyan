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
            };
            RandomList diceRandomList = new RandomList(diceItems);

            //ViewModel←Model
            DiceItems = diceRandomList.Items.ToReadOnlyReactiveCollection();
            DiceCommand = diceRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand();

            //Button
            DiceItemTapped.Where(_ => !diceRandomList.InRundom).Subscribe(x => x.IsSelected = !x.IsSelected);
            DiceCommand.Subscribe(_ => diceRandomList.RandomAction());
        }
    }
}
