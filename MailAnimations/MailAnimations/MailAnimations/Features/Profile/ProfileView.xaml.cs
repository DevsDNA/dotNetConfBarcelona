namespace MailAnimations.Features
{
    using MailAnimations.Common;
    using ReactiveUI;
    using System;
    using System.Diagnostics;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public partial class ProfileView
    {
        private double headerMaxScroll = 0;
        private double labelTitleMaxTranslation = 0;
        private double imgProfileMaxTranslationX = 0;
        private double imgProfileMaxTranslationY = 0;
        private double imgProfileMinScaleDiff = 0;

        public ProfileView()
        {
            InitializeComponent();
        }

        protected override void CreateBindings(CompositeDisposable disposables)
        {
            base.CreateBindings(disposables);

            disposables.Add(this.OneWayBind(ViewModel, vm => vm.Name, v => v.LblNameValue.Text));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.Surname, v => v.LblSurnameValue.Text));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.Mail, v => v.LblMailValue.Text));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.Phone, v => v.LblPhoneValue.Text));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.Address, v => v.LblAddressValue.Text));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.RecoveryMail, v => v.LblRecoveryMailValue.Text));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.Notifications, v => v.LblNotificationsValue.Text, n => n ? "Activado" : "Desactivado"));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.StoredMB, v => v.LblStoredMBValue.Text, smb => $"{smb.ToString("N0")}MB"));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.Signature, v => v.LblSignatureValue.Text, s => s ? "Activado" : "Desactivado"));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.MarkNotRead, v => v.LblMarkNotReadValue.Text, mnr => mnr ? "Activado" : "Desactivado"));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.NotifyWhenRead, v => v.LblNotifyWhenReadValue.Text, mnr => mnr ? "Activado" : "Desactivado"));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.CloudStorage, v => v.LblCloudStorageValue.Text, mnr => mnr ? "Activado" : "Desactivado"));

            disposables.Add(this.BindCommand(ViewModel, vm => vm.NavigateBackCommand, v => v.BtnBack));
        }

        protected override void ManageEvents(CompositeDisposable disposables)
        {
            base.ManageEvents(disposables);

            IObservable<EventPattern<ScrolledEventArgs>> svContentScrolled = Observable.FromEventPattern<ScrolledEventArgs>(h => SvContent.Scrolled += h, h => SvContent.Scrolled -= h);
            disposables.Add(svContentScrolled.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async e => await ContentScrolled(e.EventArgs.ScrollY), ex => Debug.WriteLine(ex.Message)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CalculateCollapseHeaderItemsSize();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            CalculateCollapseHeaderItemsSize();
        }

        private void CalculateCollapseHeaderItemsSize()
        {
            if (headerMaxScroll == 0 &&
                BtnBack.Width > 0 &&
                LblTitle.Width > 0 &&
                ImgProfilePicture.Width > 0)
            {
                headerMaxScroll = 120;
                labelTitleMaxTranslation = ((DeviceDisplayInfo.ScreenWidth - LblTitle.Width) / 2) - BtnBack.Width - BtnBack.Margin.HorizontalThickness;

                double minProfileImageWith = Math.Min(DeviceDisplayInfo.ScreenWidth - BtnBack.Width - BtnBack.Margin.HorizontalThickness - LblTitle.Width - ImgProfilePicture.Margin.HorizontalThickness, 40);
                imgProfileMaxTranslationX = ((DeviceDisplayInfo.ScreenWidth - minProfileImageWith - ImgProfilePicture.Margin.HorizontalThickness) / 2);
                imgProfileMinScaleDiff = 1 - (minProfileImageWith / ImgProfilePicture.Width);
                imgProfileMaxTranslationY = ImgProfilePicture.Margin.Top - 10 + (ImgProfilePicture.Width - minProfileImageWith) / 2;
            }
        }

        private async Task ContentScrolled(double yOffset)
        {
            //DEMO
            //if (yOffset > headerMaxScroll && 
            //    -LblTitle.TranslationX > labelTitleMaxTranslation)
            //{
            //    return;
            //}

            //double scrollY = yOffset < 0 ? 0 : yOffset;
            //double labelTitleTranslation = labelTitleMaxTranslation * scrollY / headerMaxScroll;
            //double imageTranslationX = imgProfileMaxTranslationX * scrollY / headerMaxScroll;
            //double imageTranslationY = imgProfileMaxTranslationY * scrollY / headerMaxScroll;
            //double imageScale = 1 - imgProfileMinScaleDiff * scrollY / headerMaxScroll;

            //Debug.WriteLine($"scrollY: {scrollY}");
            //Debug.WriteLine($"labelTitleTranslation: {labelTitleTranslation}");
            //Debug.WriteLine($"imageTranslationX: {imageTranslationX}");
            //Debug.WriteLine($"imageTranslationY: {imageTranslationY}");
            //Debug.WriteLine($"imageScale: {imageScale}");

            //if (scrollY >= headerMaxScroll)
            //{
            //    await Task.WhenAll(GridImageBackgroud.TranslateTo(0, -headerMaxScroll),
            //                       LblTitle.TranslateTo(-labelTitleMaxTranslation, 0),
            //                       ImgProfilePicture.TranslateTo(imgProfileMaxTranslationX, -imgProfileMaxTranslationY),
            //                       ImgProfilePicture.ScaleTo(1 - imgProfileMinScaleDiff));
            //}
            //else
            //{
            //    await Task.WhenAll(GridImageBackgroud.TranslateTo(0, -scrollY),
            //                       LblTitle.TranslateTo(-labelTitleTranslation, 0),
            //                       ImgProfilePicture.TranslateTo(imageTranslationX, -imageTranslationY),
            //                       ImgProfilePicture.ScaleTo(imageScale));
            //}
        }
    }
}