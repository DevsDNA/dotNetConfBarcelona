namespace MailAnimations.Controls
{
    using System.Windows.Input;
    using Xamarin.Forms;

    public partial class CustomSearchEntry
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomSearchEntry));
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomSearchEntry), propertyChanged: CommandPropertyChanged);
        
        public CustomSearchEntry()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        private static void CommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomSearchEntry)bindable).TapSearch.Command = (ICommand)newValue;
        }
    }
}