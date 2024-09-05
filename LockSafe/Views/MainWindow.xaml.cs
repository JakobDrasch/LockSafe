﻿using System.Collections.ObjectModel;
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
using LockSafe.ViewModels;

namespace LockSafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModelContext { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _mainViewModelContext = new MainViewModel();
            this.DataContext = _mainViewModelContext;

            // Füge einen EventHandler hinzu, der auf Änderungen in FormattedPassword reagiert
            _mainViewModelContext.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "FormattedPassword")
                {
                    UpdateTextBlockInlines(_mainViewModelContext.FormattedPassword);
                }
            };

        }

        private void UpdateTextBlockInlines(ObservableCollection<Run> formattedPassword)
        {
            TxtPassword.Inlines.Clear();
            foreach (var run in formattedPassword)
            {
                TxtPassword.Inlines.Add(run);
            }
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
        }
    }
}