# NovaVoice

NovaVoice is a cross-platform .NET MAUI application that provides voice-to-text transcription and AI-powered conversational responses using Gemini and text-to-speech (TTS) services.

## Features
- Start and stop voice recognition with a simple UI
- Transcribes spoken words to text in real time
- Integrates with GeminiService for AI-powered replies
- Uses TTSService to read responses aloud
- Built with .NET MAUI and CommunityToolkit.Maui

## Getting Started

### Prerequisites
- [.NET 8+ SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022+ with MAUI workload

### Setup
1. Clone the repository:
   ```sh
   git clone https://github.com/swapnilpopatgaikwad/NovaVoice.git
   ```
2. Open the solution in Visual Studio.
3. Restore NuGet packages.
4. Build and run the project on your preferred platform (Windows, Android, iOS, MacCatalyst).

## Usage
- Click **Start Listening** to begin voice recognition.
- Speak into your device's microphone.
- The transcribed text will appear in the editor.
- The app will send your input to Gemini and display the AI's response.
- The response will also be spoken aloud using TTS.
- Click **Stop Listening** to end voice recognition.

## Project Structure
- `MainPage.xaml` / `MainPage.xaml.cs`: Main UI and logic
- `Services/`: Contains GeminiService and TTSService
- `MauiProgram.cs`: App startup and dependency injection

## License
MIT License