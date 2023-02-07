using Avalonia.Controls;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += delegate { ((MainWindowViewModel)this.DataContext).Save(); };
        }
    }
        
}