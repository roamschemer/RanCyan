using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RanCyan.TriggerActions {
    public class MoveTriggerAction : TriggerAction<VisualElement> {
        public bool IsActive { get; set; }

        protected override void Invoke(VisualElement sender) {
            var rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var xRate = (rnd.Next(0, 20) - 10) * 0.1;
            if (IsActive) {
                sender.TranslationX = sender.Width * xRate;
                sender.TranslationY = sender.Height;
                sender.Opacity = 0;
                sender.TranslateTo(0, 0);
                sender.FadeTo(1);
            } else {
                sender.TranslateTo(0, sender.Height);
                sender.FadeTo(0);
            }
        }
    }
}
