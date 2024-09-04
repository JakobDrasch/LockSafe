using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private int _passwordLength;
        public int PasswordLength
        {
            get { return _passwordLength; }
            set 
            {
  
                if (value < 4 || value > 64)
                    throw new ArgumentOutOfRangeException("Value must be between 4 and 64");
                _passwordLength = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GeneratedPassword));
            }
        }

        private bool _includeNumbers;

        public bool IncludeNumbers
        {
            get { return _includeNumbers; }
            set 
            {
                if (!IncludeNumbers || (IncludeLetters || IncludeSpecialCharacters))
                    _includeNumbers = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GeneratedPassword));
            }
        }

        private bool _includeLetters;

        public bool IncludeLetters
        {
            get { return _includeLetters; }
            set 
            {
                if (IncludeLetters && IncludeUpperCase && !value && (IncludeNumbers || IncludeSpecialCharacters))
                    IncludeUpperCase = false;
                if (!IncludeLetters || (IncludeNumbers || IncludeSpecialCharacters))
                    _includeLetters = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GeneratedPassword));
            }
        }

        private bool _includeUpperCase;

        public bool IncludeUpperCase
        {
            get { return _includeUpperCase; }
            set 
            {
                if (IncludeUpperCase && IncludeLetters && (IncludeNumbers || IncludeSpecialCharacters) && !value)
                    _includeUpperCase = value;

                if (IncludeLetters)
                    _includeUpperCase = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GeneratedPassword));
            }
        }

        private bool _includeSpecialCharacters;

        public bool IncludeSpecialCharacters
        {
            get { return _includeSpecialCharacters; }
            set 
            {
                if (!IncludeSpecialCharacters || (IncludeLetters || IncludeNumbers))
                    _includeSpecialCharacters = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GeneratedPassword));
            }
        }

        public string GeneratedPassword
        {
            get
            {
                return PasswordGenerator.GeneratePassword(PasswordLength, IncludeNumbers, IncludeLetters, IncludeUpperCase, IncludeSpecialCharacters);
            }
        }

        public MainViewModel()
        {
            PasswordLength = 4;
            IncludeNumbers = false;
            IncludeLetters = true;
            IncludeUpperCase = false;
            IncludeSpecialCharacters = false;
        }
    }
}
