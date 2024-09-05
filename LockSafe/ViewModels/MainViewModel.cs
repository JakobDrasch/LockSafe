using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using LockSafe.Models;

namespace LockSafe.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        // INotifyPropertyChanged-Event
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isInitializing = false;

        private int _passwordLength;
        public int PasswordLength
        {
            get { return _passwordLength; }
            set 
            {
                if (value == _passwordLength)
                    return;

                if (value < 4 || value > 64)
                    throw new ArgumentOutOfRangeException("Value must be between 4 and 42");
                _passwordLength = value;
                GenerateNewPassword();
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormattedPassword));
            }
        }

        private bool _includeNumbers;

        public bool IncludeNumbers
        {
            get { return _includeNumbers; }
            set 
            {
                if (!IncludeNumbers || (IncludeLetters || IncludeSpecialCharacters))
                {
                    _includeNumbers = value;
                    GenerateNewPassword();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FormattedPassword));
                }
            }
        }

        private bool _includeLetters;

        public bool IncludeLetters
        {
            get { return _includeLetters; }
            set
            {
                if (IncludeLetters && IncludeUpperCase && !value && (IncludeNumbers || IncludeSpecialCharacters))
                {
                    IncludeUpperCase = false;
                    OnPropertyChanged(nameof(FormattedPassword));
                }
                   
                if (!IncludeLetters || (IncludeNumbers || IncludeSpecialCharacters))
                {
                    _includeLetters = value;
                    GenerateNewPassword();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FormattedPassword));
                }
            }
        }

        private bool _includeUpperCase;

        public bool IncludeUpperCase
        {
            get { return _includeUpperCase; }
            set
            {
                if (IncludeLetters)
                {
                    _includeUpperCase = value;
                    GenerateNewPassword();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FormattedPassword));
                }
            }
        }

        private bool _includeSpecialCharacters;

        public bool IncludeSpecialCharacters
        {
            get { return _includeSpecialCharacters; }
            set
            {
                if (!IncludeSpecialCharacters || IncludeLetters || IncludeNumbers)
                {
                    _includeSpecialCharacters = value;
                    GenerateNewPassword();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FormattedPassword));
                }
            }
        }

        private string _generatedPassword = "";
        public string GeneratedPassword
        {
            get
            {
                return _generatedPassword;
            }
            private set
            {
                if (_generatedPassword != value)
                {
                    _generatedPassword = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FormattedPassword));
                }
            }
        }

        public ObservableCollection<Run> FormattedPassword
        {
            get
            {
                return FormatPassword(GeneratedPassword);
            }
        }

        public MainViewModel()
        {
            _isInitializing = true;

            PasswordLength = 4;
            IncludeNumbers = false;
            IncludeLetters = true;
            IncludeUpperCase = false;
            IncludeSpecialCharacters = false;

            _isInitializing = false;
            GenerateNewPassword();
        }


        public void GenerateNewPassword()
        {
            if (_isInitializing)
                return;

            string password = PasswordGenerator.GeneratePassword(PasswordLength, IncludeNumbers, IncludeLetters, IncludeUpperCase, IncludeSpecialCharacters);
            GeneratedPassword = password;
        }

        public void CopyCurrentPassword()
        {
            System.Windows.Clipboard.SetText(GeneratedPassword);
        }

        /// <summary>
        /// A method that returns the formated password (special characters colored)
        /// </summary>
        /// <param name="password">The generated password as string</param>
        /// <returns>A ObservableCollection of a formated string (Run) which notifys the UI</returns>
        public ObservableCollection<Run> FormatPassword(string password)
        {
            // Run = formated string/char
            var formattedPassword = new ObservableCollection<Run>();

            foreach (var character in password)
            {
                var run = new Run(character.ToString());

                // if the current character is a special character of the PasswordGenerator class
                if (PasswordGenerator.SpecialCharacters.Contains(character))
                {
                    run.Foreground = (SolidColorBrush)Application.Current.Resources["SecondaryColor"];
                }
                else
                {
                    run.Foreground = (SolidColorBrush)Application.Current.Resources["FontColor"];
                }

                formattedPassword.Add(run);
            }

            return formattedPassword;
        }

    }
}
