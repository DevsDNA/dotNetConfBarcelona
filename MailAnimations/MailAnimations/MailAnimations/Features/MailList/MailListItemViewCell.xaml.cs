namespace MailAnimations.Features
{
    using MailAnimations.Common;
    using MailAnimations.Models;
    using ReactiveUI;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public partial class MailListItemViewCell
    {
        private double panelTranslation;
        private CompositeDisposable disposables;
        private bool isUpdatingSize;

        public MailListItemViewCell()
        {
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Do(PopulateInfo)
                .Subscribe();
        }

        protected override void OnAppearing()
        {
            Debug.WriteLine($"Appearing");
            if (isUpdatingSize)
                return;

            base.OnAppearing();
            DefineControlsPosition();

            disposables = new CompositeDisposable();
            var tapCircleTapped = Observable.FromEventPattern(h => TapCircle.Tapped += h, h => TapCircle.Tapped -= h);
            disposables.Add(tapCircleTapped.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ => await ManageActionsVisibility(), ex => Debug.WriteLine(ex.Message)));

            var btnCancelClicked = Observable.FromEventPattern(h => BtnCancel.Clicked += h, h => BtnCancel.Clicked -= h);
            disposables.Add(btnCancelClicked.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ => await ManageActionsVisibility(), ex => Debug.WriteLine(ex.Message)));

            var btnRemoveClicked = Observable.FromEventPattern(h => BtnRemove.Clicked += h, h => BtnRemove.Clicked -= h);
            disposables.Add(btnRemoveClicked.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ => await ShowRemovePanels(), ex => Debug.WriteLine(ex.Message)));

            var tapRemoveNoTapped = Observable.FromEventPattern(h => TapRemoveNo.Tapped += h, h => TapRemoveNo.Tapped -= h);
            disposables.Add(tapRemoveNoTapped.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ => await HideRemovePanels(), ex => Debug.WriteLine(ex.Message)));

            var tapRemoveYesTapped = Observable.FromEventPattern(h => TapRemoveYes.Tapped += h, h => TapRemoveYes.Tapped -= h);
            disposables.Add(tapRemoveYesTapped.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ => await RemoveViewCell(), ex => Debug.WriteLine(ex.Message)));
        }

        protected override void OnDisappearing()
        {
            Debug.WriteLine($"Disappearing");
            if (isUpdatingSize)
                return;

            disposables.Dispose();
            disposables = null;
            base.OnDisappearing();
        }

        private void DefineControlsPosition()
        {
            panelTranslation = DeviceDisplayInfo.ScreenWidth + 50;
            GridActions.TranslationX = -panelTranslation;
            GridContent.TranslationX = 0;

            Height = 90;
            GridMain.RowDefinitions[0].Height = new GridLength(89, GridUnitType.Absolute);
            GridMain.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Absolute);
            GridMain.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Absolute);
            GridQuestion.IsVisible = false;
        }

        private void PopulateInfo(Mail mail)
        {
            BvCircleSource.BackgroundColor = GetCircleColor();
            LblCircleSource.Text = GetCircleText(mail.SourceName);

            LblDate.Text = mail.Date.ToString("d");
            LblSourceName.Text = mail.SourceName;
            BvCircleNotRead.IsVisible = !mail.Read;
            LblSubject.Text = mail.Subject;
            LblContent.Text = mail.Content;
        }

        private Color GetCircleColor()
        {
            Random rnd = new Random();
            int colorIndex = rnd.Next(1, 5);

            switch (colorIndex)
            {
                case 1:
                    return Color.Red;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Blue;
                case 4:
                default:
                    return Color.Orange;
            }
        }

        private string GetCircleText(string mailSourceName)
        {
            if (string.IsNullOrWhiteSpace(mailSourceName))
                return string.Empty;

            string[] splitedMailSourceName = mailSourceName.Split(' ');
            string circleText = splitedMailSourceName[0].Substring(0, 1);

            if (splitedMailSourceName.Length >= 2)
                circleText += splitedMailSourceName[1].Substring(0, 1);

            return circleText;
        }

        private async Task ManageActionsVisibility()
        {
            if (GridActions.TranslationX == 0)
            {
                //DEMO 6
                GridContent.TranslationX = 0;
                GridActions.TranslationX = -panelTranslation;
                //await Task.WhenAll(GridContent.TranslateTo(0, 0),
                //                   GridActions.TranslateTo(-panelTranslation, 0));
            }
            else
            {
                //DEMO 6
                GridContent.TranslationX = panelTranslation;
                GridActions.TranslationX = 0;
                //await Task.WhenAll(GridContent.TranslateTo(panelTranslation, 0),
                //                   GridActions.TranslateTo(0, 0));
            }
        }

        private async Task ShowRemovePanels()
        {
            isUpdatingSize = true;

            //DEMO 6
            GridActions.IsVisible = false;
            GridQuestion.IsVisible = true;
            Height = 90 + 90;
            GridMain.RowDefinitions[1].Height = new GridLength(90, GridUnitType.Absolute);
            //await ShowRemoveQuestionPanel();
            //await ManageRemoveAnswerPanelVisibility(true);

            isUpdatingSize = false;
        }

        private async Task HideRemovePanels()
        {
            isUpdatingSize = true;

            //DEMO 6
            Height = 90;
            GridMain.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Absolute);

            //await ManageRemoveAnswerPanelVisibility(false);
            //await HideRemoveQuestionPanel();
            GridQuestion.IsVisible = false;
            GridActions.IsVisible = true;

            isUpdatingSize = false;
        }

        private async Task ShowRemoveQuestionPanel()
        {
            GridQuestion.RotationX = -270;
            await GridActions.RotateXTo(-90);
            GridActions.IsVisible = false;
            GridQuestion.IsVisible = true;
            await GridQuestion.RotateXTo(-360);
            GridQuestion.RotationX = 0;
        }

        private async Task HideRemoveQuestionPanel()
        {
            GridActions.RotationX = -270;
            await GridQuestion.RotateXTo(-90);
            GridQuestion.IsVisible = false;
            GridActions.IsVisible = true;
            await GridActions.RotateXTo(-360);
            GridActions.RotationX = 0;
        }

        private Task ManageRemoveAnswerPanelVisibility(bool show)
        {
            double sourceValue = show ? 0 : 90; 
            double targetValue = show ? 90 : 0; 
            
            Animation animation = new Animation(x =>
            {
                double value = x < 0 ? 0 : x;
                Height = 90 + value;
                GridMain.RowDefinitions[1].Height = new GridLength(value, GridUnitType.Absolute);
            }, sourceValue, targetValue);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            animation.Commit(GridMain, "RemoveAnswerPanelAnimation", easing: show ? Easing.BounceOut : null, finished: (x, y) =>
            {
                Height = 90 + targetValue;
                GridMain.RowDefinitions[1].Height = new GridLength(targetValue, GridUnitType.Absolute);
                tcs.SetResult(true);
            });

            return tcs.Task;
        }

        private async Task RemoveViewCell()
        {
            isUpdatingSize = true;

            //DEMO 6
            GridMain.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Absolute);
            GridMain.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Absolute);
            GridMain.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Absolute);

            Height = 0;
            //await HideViewCell();

            isUpdatingSize = false;
        }

        private Task HideViewCell()
        {
            double rowZeroProportion = 89 / Height;
            double rowOneProportion = 90 / Height;
            double rowTwoProportion = 1 / Height;

            Animation animation = new Animation(x =>
            {
                GridMain.RowDefinitions[0].Height = new GridLength(x * rowZeroProportion, GridUnitType.Absolute);
                GridMain.RowDefinitions[1].Height = new GridLength(x * rowOneProportion, GridUnitType.Absolute);
                GridMain.RowDefinitions[2].Height = new GridLength(x * rowTwoProportion, GridUnitType.Absolute);
                Height = x;
            }, Height, 0);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            animation.Commit(GridMain, "RemoveViewCellAnimation", finished: (x, y) =>
            {
                GridMain.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Absolute);
                GridMain.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Absolute);
                GridMain.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Absolute);

                Height = 0;
                tcs.SetResult(true);
            });

            return tcs.Task;
        }
    }
}