using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RanCyan.TriggerActions {
    public class MoveTriggerAction : TriggerAction<VisualElement> {
        public bool IsActive { get; set; }

        protected override void Invoke(VisualElement sender) {
            if (IsActive) {
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
