namespace MailAnimations.Features
{
    using MailAnimations.Base;
    using ReactiveUI;
    using System;
    using System.Diagnostics;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    public class ProfileViewModel : BaseViewModel
    {
        private string name;
        private string surname;
        private string mail;
        private string phone;
        private string address;
        private string recoveryMail;
        private bool notifications;
        private int storedMB;
        private bool signature;
        private bool markNotRead;
        private bool notifyWhenRead;
        private bool cloudStorage;

        public ProfileViewModel() : base()
        {
            name = "Jorge";
            surname = "Diego Crespo";
            mail = "jorge@mail.com";
            phone = "012 345 678";
            address = "Mi dirección";
            recoveryMail = "recovery@mail.com";
            notifications = true;
            storedMB = 200;
            signature = false;
            markNotRead = true;
            notifyWhenRead = false;
            cloudStorage = true;

            CreateCommands();
        }

        public string Name
        {
            get => name;
            private set => this.RaiseAndSetIfChanged(ref name, value);
        }

        public string Surname
        {
            get => surname;
            private set => this.RaiseAndSetIfChanged(ref surname, value);
        }

        public string Mail
        {
            get => mail;
            private set => this.RaiseAndSetIfChanged(ref mail, value);
        }

        public string Phone
        {
            get => phone;
            private set => this.RaiseAndSetIfChanged(ref phone, value);
        }

        public string Address
        {
            get => address;
            private set => this.RaiseAndSetIfChanged(ref address, value);
        }

        public string RecoveryMail
        {
            get => recoveryMail;
            private set => this.RaiseAndSetIfChanged(ref recoveryMail, value);
        }

        public bool Notifications
        {
            get => notifications;
            private set => this.RaiseAndSetIfChanged(ref notifications, value);
        }

        public int StoredMB
        {
            get => storedMB;
            private set => this.RaiseAndSetIfChanged(ref storedMB, value);
        }

        public bool Signature
        {
            get => signature;
            private set => this.RaiseAndSetIfChanged(ref signature, value);
        }

        public bool MarkNotRead
        {
            get => markNotRead;
            private set => this.RaiseAndSetIfChanged(ref markNotRead, value);
        }

        public bool NotifyWhenRead
        {
            get => notifyWhenRead;
            private set => this.RaiseAndSetIfChanged(ref notifyWhenRead, value);
        }

        public bool CloudStorage
        {
            get => cloudStorage;
            private set => this.RaiseAndSetIfChanged(ref cloudStorage, value);
        }

        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; private set; }

        public override async Task AppearingAsync()
        {
            await base.AppearingAsync();

            Disposables.Add(NavigateBackCommand.ThrownExceptions.Subscribe(ex => Debug.WriteLine(ex?.Message)));
        }
        private void CreateCommands()
        {
            NavigateBackCommand = ReactiveCommand.CreateFromTask(navigationService.NavigateBack);
        }
    }
}
