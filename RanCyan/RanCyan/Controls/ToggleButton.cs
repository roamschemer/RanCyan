using Xamarin.Forms;

namespace RanCyan.Controls
{
    public class ToggleButton : Button
    {
        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(
                "IsSelected",
                typeof(bool),
                typeof(ToggleButton),
                false);

        public object IsSelected
        {
            get => GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly BindableProperty IsHitedProperty =
            BindableProperty.Create(
            "IsHited",
            typeof(bool),
            typeof(ToggleButton),
            false);

        public object IsHited
        {
            get => GetValue(IsHitedProperty);
            set => SetValue(IsHitedProperty, value);
        }

    }
}
