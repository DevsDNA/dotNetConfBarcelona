namespace MailAnimations.Features
{
    using MailAnimations.Common;
    using ReactiveUI;
    using System;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public partial class MailListView
    {
        public MailListView()
        {
            InitializeComponent();
            DefineControlsPosition();
        }

        protected override void CreateBindings(CompositeDisposable disposables)
        {
            base.CreateBindings(disposables);

            disposables.Add(this.OneWayBind(ViewModel, vm => vm.AllMailList, v => v.LvAllMails.ItemsSource));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.NotReadMailList, v => v.LvNotReadMails.ItemsSource));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.NavigateToProfileCommand, v => v.TapImgProfile.Command));

            disposables.Add(this.OneWayBind(ViewModel, vm => vm.NotReadMails, v => v.LblNotReadMailsValue.Text));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.CheckedMails, v => v.LblCheckedMailsValue.Text));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.RemovedMails, v => v.LblRemovedMailsValue.Text));
        }

        protected override void ManageEvents(CompositeDisposable disposables)
        {
            base.ManageEvents(disposables);

            disposables.Add(this.WhenAnyValue(x => x.ViewModel.IsLoadingMailList)
                       .Subscribe(e => Device.BeginInvokeOnMainThread(ManageMailListVisibility), ex => Debug.WriteLine(ex?.Message)));

            disposables.Add(this.WhenAnyValue(x => x.ViewModel.IsLoadingSummary)
                       .Subscribe(e => Device.BeginInvokeOnMainThread(ManageSummaryVisibility), ex => Debug.WriteLine(ex?.Message)));

            var csMailListSelectorSelectionChanged = Observable.FromEventPattern<bool>(h => CsMailListSelector.SelectionChanged += h, h => CsMailListSelector.SelectionChanged -= h);
            disposables.Add(csMailListSelectorSelectionChanged.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async e => await MailListSelectorSelection(e.EventArgs), ex => Debug.WriteLine(ex.Message)));

            var btnMenuClicked = Observable.FromEventPattern(h => BtnMenu.Clicked += h, h => BtnMenu.Clicked -= h);
            disposables.Add(btnMenuClicked.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ => await ManageMenuVisibility(), ex => Debug.WriteLine(ex.Message)));
        }

        private void DefineControlsPosition()
        {
            LvNotReadMails.TranslationX = DeviceDisplayInfo.ScreenWidth + 50;
        }

        private async void ManageMailListVisibility()
        {
            AiAllMails.IsVisible = ViewModel.IsLoadingMailList;
            AiAllMails.IsRunning = ViewModel.IsLoadingMailList;

            //DEMO
            LvAllMails.Opacity = ViewModel.IsLoadingMailList ? 0 : 1;
            LvAllMails.Scale = ViewModel.IsLoadingMailList ? 0 : 1;            
            //await Task.WhenAll(LvAllMails.FadeTo(ViewModel.IsLoadingMailList ? 0 : 1),
            //                   LvAllMails.ScaleTo(ViewModel.IsLoadingMailList ? 0 : 1));            
        }

        private async void ManageSummaryVisibility()
        {
            AiSummary.IsVisible = ViewModel.IsLoadingSummary;
            AiSummary.IsRunning = ViewModel.IsLoadingSummary;

            //DEMO
            GridSummary.Opacity = ViewModel.IsLoadingSummary ? 0 : 1;
            GridSummary.Scale = ViewModel.IsLoadingSummary ? 0 : 1;
            //await Task.WhenAll(GridSummary.FadeTo(ViewModel.IsLoadingSummary ? 0 : 1),
            //                   GridSummary.ScaleTo(ViewModel.IsLoadingSummary ? 0 : 1));
        }

        private async Task MailListSelectorSelection(bool showAllMails)
        {
            if (showAllMails)
            {
                //DEMO
                LvAllMails.TranslationX = 0;
                LvNotReadMails.TranslationX = DeviceDisplayInfo.ScreenHeight + 50;
                //await Task.WhenAll(LvAllMails.TranslateTo(0, 0),
                //                   LvNotReadMails.TranslateTo(DeviceDisplayInfo.ScreenHeight + 50, 0));
            }
            else
            {
                //DEMO
                LvAllMails.TranslationX = -(DeviceDisplayInfo.ScreenHeight + 50);
                LvNotReadMails.TranslationX = 0;
                //await Task.WhenAll(LvAllMails.TranslateTo(-(DeviceDisplayInfo.ScreenHeight + 50), 0),
                //                   LvNotReadMails.TranslateTo(0, 0));
            }
        }

        private async Task ManageMenuVisibility()
        {
            if (GridMenu.TranslationY == 0)
            {
                //DEMO
                GridMenu.TranslationY = 215;
                BvMenuVisibility.Opacity = 0;
                //await Task.WhenAll(GridMenu.TranslateTo(0, 215),
                //                   BvMenuVisibility.FadeTo(0));
                BvMenuVisibility.IsVisible = false;
            }
            else
            {
                BvMenuVisibility.IsVisible = true;
                //DEMO
                GridMenu.TranslationY = 0;
                BvMenuVisibility.Opacity = 0.5;
                //await Task.WhenAll(GridMenu.TranslateTo(0, 0),
                //                   BvMenuVisibility.FadeTo(0.5));
            }
        }
    }
}