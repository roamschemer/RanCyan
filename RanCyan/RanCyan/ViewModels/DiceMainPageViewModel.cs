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
            var diceItems = new List<Item>(Enumerable.Range(1, 100).Select(x => new Item { Name = x.ToString(), Ratio = 1, IsSelected = true }));
            for(var i = 0; i < 6; i++) { diceItems[i].IsSelected = false; }
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
