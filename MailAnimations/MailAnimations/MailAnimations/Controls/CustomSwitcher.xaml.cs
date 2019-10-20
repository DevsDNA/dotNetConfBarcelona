namespace MailAnimations.Controls
{
    using System;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public partial class CustomSwitcher
    {
        public static readonly BindableProperty LeftTextProperty = BindableProperty.Create(nameof(LeftText), typeof(string), typeof(CustomSwitcher));
        public static readonly BindableProperty RightTextProperty = BindableProperty.Create(nameof(RightText), typeof(string), typeof(CustomSwitcher));
        public static readonly BindableProperty IsLeftSelectedProperty = BindableProperty.Create(nameof(IsLeftSelected), typeof(bool), typeof(CustomSwitcher), true, propertyChanged: IsLeftSelectedChanged);

        public event EventHandler<bool> SelectionChanged;

        public CustomSwitcher()
        {
            InitializeComponent();
            BindingContext = this;
            CreateCommands();
        }

        public string LeftText
        {
            get { return (string)GetValue(LeftTextProperty); }
            set { SetValue(LeftTextProperty, value); }
        }

        public string RightText
        {
            get { return (string)GetValue(RightTextProperty); }
            set { SetValue(RightTextProperty, value); }
        }

        public bool IsLeftSelected
        {
            get { return (bool)GetValue(IsLeftSelectedProperty); }
            set { SetValue(IsLeftSelectedProperty, value); }
        }

        private static void IsLeftSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != newValue)
                ((CustomSwitcher)bindable).Select();
        }

        private async void Select()
        {
            if (IsLeftSelected)
            {
                //DEMO
                LblRightSelectedText.Opacity = 0;
                LblRightUnselectedText.Opacity = 1;
                GridSelection.TranslationX = 0;
                LblLeftSelectedText.Opacity = 1;
                LblLeftUnselectedText.Opacity = 0;
                //await MoveToLeft();
            }
            else
            {
                //DEMO
                LblLeftSelectedText.Opacity = 0;
                LblLeftUnselectedText.Opacity = 1;
                GridSelection.TranslationX = GridContent.Width / 2;
                LblRightSelectedText.Opacity = 1;
                LblRightUnselectedText.Opacity = 0;
                //await MoveToRight();
            }

            SelectionChanged?.Invoke(this, IsLeftSelected);
        }

        private async Task MoveToLeft()
        {
            await Task.WhenAll(CollapseSelector(),
                               LblRightSelectedText.FadeTo(0, 200),
                               LblRightUnselectedText.FadeTo(1, 200),
                               Task.Delay(200));

            await Task.WhenAll(GridSelection.TranslateTo(0, 0, 200),
                               Task.Delay(200));

            await Task.WhenAll(ExpandSelector(),
                               LblLeftSelectedText.FadeTo(1, 200),
                               LblLeftUnselectedText.FadeTo(0, 200),
                               Task.Delay(200));
        }

        private async Task MoveToRight()
        {
            await Task.WhenAll(CollapseSelector(),
                               LblLeftSelectedText.FadeTo(0, 200),
                               LblLeftUnselectedText.FadeTo(1, 200),
                               Task.Delay(200));

            await Task.WhenAll(GridSelection.TranslateTo(GridContent.Width / 2, 0, 200),
                               Task.Delay(200));

            await Task.WhenAll(ExpandSelector(),
                               LblRightSelectedText.FadeTo(1, 200),
                               LblRightUnselectedText.FadeTo(0, 200),
                               Task.Delay(200));
        }

        private Task CollapseSelector()
        {
            Animation animation = new Animation(x =>
            {
                GridSelection.ColumnDefinitions[1].Width = new GridLength(x, GridUnitType.Absolute);
            }, 0, GridSelection.Width - 36);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            animation.Commit(this, "CollapseSelector", length: 200, finished: (x, y) =>
            {
                GridSelection.ColumnDefinitions[1].Width = new GridLength(GridSelection.Width - 36, GridUnitType.Absolute);
                tcs.SetResult(true);
            });

            return tcs.Task;
        }

        private Task ExpandSelector()
        {
            Animation animation = new Animation(x =>
            {
                GridSelection.ColumnDefinitions[1].Width = new GridLength(x, GridUnitType.Absolute);
            }, GridSelection.Width - 36, 0);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            animation.Commit(this, "ExpandSelector", length: 200, finished: (x, y) =>
            {
                GridSelection.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Absolute);
                tcs.SetResult(true);
            });

            return tcs.Task;
        }

        private void CreateCommands()
        {
            TapLeft.Command = new Command(() =>
            {
                IsLeftSelected = true;
                Select();
            });
            TapRight.Command = new Command(() =>
            {
                IsLeftSelected = false;
                Select();
            });
        }
    }
}