namespace MailAnimations.Features
{
    using DynamicData;
    using MailAnimations.Base;
    using MailAnimations.Models;
    using MailAnimations.Services;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class MailListViewModel : BaseViewModel
    {
        private readonly IApiService apiService;

        private bool isLoadingMailList;
        private bool isLoadingSummary;

        private int notReadMails;
        private int checkedMails;
        private int removedMails;
        
        private ReadOnlyObservableCollection<Mail> allMailList;
        private ReadOnlyObservableCollection<Mail> notReadMailList;

        public MailListViewModel() : base()
        {
            apiService = DependencyService.Get<IApiService>();

            isLoadingMailList = true;
            isLoadingSummary = true;

            CreateCommands();
        }

        public int NotReadMails
        {
            get => notReadMails;
            private set => this.RaiseAndSetIfChanged(ref notReadMails, value);
        }

        public int CheckedMails
        {
            get => checkedMails;
            private set => this.RaiseAndSetIfChanged(ref checkedMails, value);
        }

        public int RemovedMails
        {
            get => removedMails;
            private set => this.RaiseAndSetIfChanged(ref removedMails, value);
        }

        public bool IsLoadingMailList
        {
            get => isLoadingMailList;
            private set => this.RaiseAndSetIfChanged(ref isLoadingMailList, value);
        }

        public bool IsLoadingSummary
        {
            get => isLoadingSummary;
            private set => this.RaiseAndSetIfChanged(ref isLoadingSummary, value);
        }

        public SourceList<Mail> AllMailListSource { get; private set; }
        public ReadOnlyObservableCollection<Mail> AllMailList => allMailList;
        public SourceList<Mail> NotReadMailListSource { get; private set; }
        public ReadOnlyObservableCollection<Mail> NotReadMailList => notReadMailList;


        public ReactiveCommand<Unit, Unit> SearchMailsCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> NavigateToNewMailCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> NavigateToRemovedMailCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> NavigateToSentMailCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> NavigateToPendingMailCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> NavigateToProfileCommand { get; private set; }

        public override async Task AppearingAsync()
        {
            await base.AppearingAsync();

            Disposables.Add(SearchMailsCommand.ThrownExceptions.Subscribe(ex => Debug.WriteLine(ex?.Message)));
            Disposables.Add(NavigateToNewMailCommand.ThrownExceptions.Subscribe(ex => Debug.WriteLine(ex?.Message)));
            Disposables.Add(NavigateToRemovedMailCommand.ThrownExceptions.Subscribe(ex => Debug.WriteLine(ex?.Message)));
            Disposables.Add(NavigateToSentMailCommand.ThrownExceptions.Subscribe(ex => Debug.WriteLine(ex?.Message)));
            Disposables.Add(NavigateToPendingMailCommand.ThrownExceptions.Subscribe(ex => Debug.WriteLine(ex?.Message)));
            Disposables.Add(NavigateToProfileCommand.ThrownExceptions.Subscribe(ex => Debug.WriteLine(ex?.Message)));

            ConnectLists();
            LoadMailList();
            LoadSummary();
        }
        private void CreateCommands()
        {
            SearchMailsCommand = ReactiveCommand.CreateFromTask(SearchMails);
            NavigateToNewMailCommand = ReactiveCommand.CreateFromTask(navigationService.NavigateToNewMailAsync);
            NavigateToRemovedMailCommand = ReactiveCommand.CreateFromTask(() => Task.CompletedTask);
            NavigateToSentMailCommand = ReactiveCommand.CreateFromTask(() => Task.CompletedTask);
            NavigateToPendingMailCommand = ReactiveCommand.CreateFromTask(() => Task.CompletedTask);
            NavigateToProfileCommand = ReactiveCommand.CreateFromTask(navigationService.NavigateToProfileAsync);
        }

        private void ConnectLists()
        {
            AllMailListSource = new SourceList<Mail>();
            Disposables.Add(AllMailListSource.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out allMailList)
                .Subscribe((mf) => this.RaisePropertyChanged(nameof(AllMailList))));
            Disposables.Add(AllMailListSource);

            NotReadMailListSource = new SourceList<Mail>();
            Disposables.Add(NotReadMailListSource.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out notReadMailList)
                .Subscribe((mf) => this.RaisePropertyChanged(nameof(NotReadMailList))));
            Disposables.Add(NotReadMailListSource);
        }

        private async void LoadMailList()
        {
            try
            {
                IsLoadingMailList = true;

                await Task.Delay(1500);
                List<Mail> mails = await apiService.GetMails();
                List<Mail> notRead = mails.Where(m => !m.Read).ToList();

                IEnumerable<Mail> elementsToRemove = mails.Where(m => AllMailList.Contains(m));
                mails.Remove(elementsToRemove);
                AllMailListSource.AddRange(mails);

                elementsToRemove = notRead.Where(m => NotReadMailList.Contains(m));
                notRead.Remove(elementsToRemove);
                NotReadMailListSource.AddRange(notRead);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsLoadingMailList = false;
            }
        }

        private async void LoadSummary()
        {
            try
            {
                IsLoadingSummary = true;

                await Task.Delay(1000);
                NotReadMails = 26;
                CheckedMails = 15;
                RemovedMails = 5;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsLoadingSummary = false;
            }
        }

        private async Task SearchMails()
        {
            await Task.Delay(1000);
        }
    }
}
