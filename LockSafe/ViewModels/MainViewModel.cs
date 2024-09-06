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

        private SolidColorBrush[] _passwordSecurityColors = { new SolidColorBrush(Colors.Red), 
                                                              new SolidColorBrush(Colors.DarkOrange), 
                                                              new SolidColorBrush(Colors.Green) };

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
                    OnPropertyChanged(nameof(CrackTimeText));
                }
            }
        }

        public ObservableCollection<Run> FormattedPassword
        {
            get
            {
                return FormatPassword();
            }
        }

        public string CrackTimeText 
        { 
            get
            {
                return GetCrackTimeText();
            }        
        }

        private SolidColorBrush _passwordStrengthColor;
        public SolidColorBrush PasswordStrengthColor 
        { 
            get
            {
                return _passwordStrengthColor;
            }
            private set
            {
                if (_passwordStrengthColor != value)
                {
                    _passwordStrengthColor = value;
                    OnPropertyChanged(nameof(PasswordStrengthText));
                    OnPropertyChanged(nameof(PasswordStrengthIcon));
                }
            }
        }

        public string PasswordStrengthText 
        { 
            get
            {
                if (PasswordStrengthColor == _passwordSecurityColors[0])
                    return "Weak";
                else if (PasswordStrengthColor == _passwordSecurityColors[1])
                    return "Medium";
                else if (PasswordStrengthColor == _passwordSecurityColors[2])
                    return "Strong";
                return "";
            }
        }

        public Geometry PasswordStrengthIcon
        {
            get
            {
                if (PasswordStrengthColor == _passwordSecurityColors[2])
                    return (Geometry) Application.Current.Resources["CheckIcon"];
                else if (PasswordStrengthColor == _passwordSecurityColors[1])
                    return (Geometry)Application.Current.Resources["WarningIcon"];
                else
                    return (Geometry)Application.Current.Resources["CloseIcon"];
                
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
            try
            {
                System.Windows.Clipboard.SetText(GeneratedPassword);
            }
            catch (Exception ex)
            {
                return;
            }            
        }

        /// <summary>
        /// A method that returns the formated password (special characters colored)
        /// </summary>
        /// <param name="password">The generated password as string</param>
        /// <returns>A ObservableCollection of a formated string (Run) which notifys the UI</returns>
        private ObservableCollection<Run> FormatPassword()
        {
            // Run = formated string/char
            var formattedPassword = new ObservableCollection<Run>();

            foreach (var character in GeneratedPassword)
            {
                var run = new Run(character.ToString().Normalize());

                // if the current character is a special character of the PasswordGenerator class
                if (PasswordGenerator.SpecialCharacters.Contains(character))
                {
                    run.Foreground = (SolidColorBrush)Application.Current.Resources["SecondaryColor"];
                }
                else
                {
                    run.Foreground = (SolidColorBrush)Application.Current.Resources["FontColor"];
                }

                run.Text = $"\u200B{character.ToString()}";

                formattedPassword.Add(run);
            }

            return formattedPassword;
        }

        private string GetCrackTimeText()
        {
            const int secondsInMinute = 60;
            const int secondsInHour = 60 * 60;
            const int secondsInDay = 60 * 60 * 24;
            //const int secondsInMonth = 60 * 60 * 24 * 30;
            const int secondsInYear = 60 * 60 * 24 * 365;

            var entropy = PasswordGenerator.CalculateEntropy(GeneratedPassword);
            var timeInSeconds = PasswordGenerator.EstimateCrackTime(entropy, 1_000_000);

            if (entropy < 40)
                PasswordStrengthColor = _passwordSecurityColors[0];
            else if (entropy < 60)
                PasswordStrengthColor = _passwordSecurityColors[1];
            else
                PasswordStrengthColor = _passwordSecurityColors[2];

            OnPropertyChanged(nameof(PasswordStrengthColor));

            if (timeInSeconds < 1D)
                timeInSeconds = 1D;

            if (timeInSeconds < secondsInMinute)
            {
                double roundedTime = Math.Round(timeInSeconds);
                return $"{roundedTime} {(roundedTime == 1D ? "second" : "seconds")}";
            }
            else if (timeInSeconds < secondsInHour)
            {
                double roundedTime = Math.Round(timeInSeconds / secondsInMinute);
                return $"{roundedTime} {(roundedTime == 1D ? "minute" : "minutes")}";
            }
            else if (timeInSeconds < secondsInDay)
            {
                double roundedTime = Math.Round(timeInSeconds / secondsInHour);
                return $"{roundedTime} {(roundedTime == 1D ? "hour" : "hours")}";
            }
            else if (timeInSeconds < secondsInYear)
            {
                double roundedTime = Math.Round(timeInSeconds / secondsInDay);
                return $"{roundedTime} {(roundedTime == 1D ? "day" : "days")}";
            }
            else
            {
                double roundedTime = Math.Round(timeInSeconds / secondsInYear);
                if (roundedTime <= 1000)
                    return $"{roundedTime} {(roundedTime == 1D ? "year" : "years")}";
                else
                    return "Infinte";
            }
        }

    }
}
