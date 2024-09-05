using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockSafe.Models
{
    static class PasswordGenerator
    {
        private static readonly Random Random = new Random();
        private static readonly string Numbers = "0123456789";
        private static readonly string LowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string UpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static readonly string SpecialCharacters = "!@#$%^&*()-_=+[]{}|;:,.<>?/";

        public static string GeneratePassword(int length, bool includeNumbers, bool includeLetters, bool includeUpperCase, bool includeSpecialCharacters)
        {
            if (length < 1) throw new ArgumentException("Length must be greater than 0");

            StringBuilder password = new StringBuilder(length);
            StringBuilder allCharacters = new StringBuilder();

            // Fügt die benötigten Zeichen ein
            if (includeNumbers)
            {
                char number = Numbers[Random.Next(Numbers.Length)];
                password.Append(number);
                allCharacters.Append(Numbers);
            }
            if (includeLetters)
            {
                char letter = LowerCaseLetters[Random.Next(LowerCaseLetters.Length)];
                password.Append(letter);
                allCharacters.Append(LowerCaseLetters);
            }
            if (includeUpperCase)
            {
                char upperCaseLetter = UpperCaseLetters[Random.Next(UpperCaseLetters.Length)];
                password.Append(upperCaseLetter);
                allCharacters.Append(UpperCaseLetters);
            }
            if (includeSpecialCharacters)
            {
                char specialCharacter = SpecialCharacters[Random.Next(SpecialCharacters.Length)];
                password.Append(specialCharacter);
                allCharacters.Append(SpecialCharacters);
            }

            // Füllt den Rest des Passworts auf
            while (password.Length < length)
            {
                char randomCharacter = allCharacters[Random.Next(allCharacters.Length)];
                password.Append(randomCharacter);
            }

            // Mische die Zeichen
            return new string(password.ToString().OrderBy(c => Random.Next()).ToArray());
        }
    }
}
