# Lock Safe

## Overview

**Lock Safe** is a simple and secure password generator, developed with a user-friendly interface in C# WPF. It helps you create strong, secure passwords for various use cases.

## Features

- Generate random, secure passwords
- Adjustable password length
- Option to include special characters, numbers, and uppercase letters
- Copy passwords to the clipboard with one click
- Regenerate passwords with a button click
- Password strength evaluation
- User-friendly WPF interface

## Installation

### Option 1: Pre-built version (recommended for users)

1. **Requirements:**
   - **Windows OS** (as this is a WPF application)

2. **Steps:**
   1. Download the latest version of **Lock Safe**:  
      [Download the latest release](https://github.com/JakobDrasch/LockSafe/releases/latest)
   2. Unzip the downloaded file into any folder.
   3. Double-click the `LockSafe.exe` file to start the application.

---

### Option 2: Build from source (for developers)

1. **Requirements:**
   - **.NET 8 SDK** (Download here: [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
   - **Visual Studio 2022** or newer (with .NET 8 Workload)
   - **Windows OS** (for WPF applications)
   - **Git** (to clone the repository)

2. **Steps to install:**
   1. Clone this repository:  
      ```bash
      git clone https://github.com/JakobDrasch/LockSafe.git
      ```
   2. Open the project in Visual Studio 2022 or run it from the command line:
      ```bash
      dotnet run
      ```
   3. Ensure all dependencies are installed.
   4. Build and run the project.

## Usage

1. Start the application.
2. Choose the desired password length.
3. Optionally, select if you want to include special characters, numbers, or uppercase letters.
4. Click "Generate Password."
5. Copy the generated password using the built-in button and use it as needed.

## Screenshots

![Screenshot of the application](link-to-your-screenshot)

## Tech Stack

- **C#**: Programming language
- **WPF (Windows Presentation Foundation)**: For the user interface
- **.NET Core**: Runtime environment

## Contributing

Contributions are welcome! If you have an idea for a new feature or improvement:
1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -m 'Add new feature'`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Open a Pull Request.

## License

This project is licensed under the MIT License – see [LICENSE](LICENSE.txt) for more details.
