using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LockSafe.ViewModels;
using LockSafe.Models;
using System.Windows.Media.Animation;

namespace LockSafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModelContext;
        private NotificationManager _popup;

        public MainWindow()
        {
            InitializeComponent();
            _mainViewModelContext = new MainViewModel();
            _popup = new NotificationManager(ref this.NotificationPopup, ref this.NotificationText, ref this.MainGrid);

            this.DataContext = _mainViewModelContext;

            // Füge einen EventHandler hinzu, der auf Änderungen in FormattedPassword reagiert
            _mainViewModelContext.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "FormattedPassword")
                {
                    UpdateRichTextBoxInlines(_mainViewModelContext.FormattedPassword);
                }
            };


        }

        private void UpdateRichTextBoxInlines(ObservableCollection<Run> formattedPassword)
        {
            RTbPassword.Document.Blocks.Clear();

            var paragraph = new Paragraph();
            foreach (var run in formattedPassword)
            {
                paragraph.Inlines.Add(run);  // Add each Run to the paragraph
            }

            RTbPassword.Document.Blocks.Add(paragraph);
        }


        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            // this.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                BtnMaximize.Content = "🗖";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                BtnMaximize.Content = "🗗";
            }
        }

        private void WinMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModelContext.GenerateNewPassword();
        }
        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModelContext.CopyCurrentPassword();            
            _popup.ShowNotification("Password copied to clipboard");

        }
    }
}