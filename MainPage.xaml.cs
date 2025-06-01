using CommunityToolkit.Maui.Media;
using NovaVoice.Services;
using System.Globalization;

namespace NovaVoice
{
    public partial class MainPage : ContentPage
    {
        private readonly GeminiService _geminiService ;
        private readonly TTSService _ttsService;
        private readonly ISpeechToText _speechToText;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public MainPage(ISpeechToText speechToText, GeminiService geminiService, TTSService tTSService )
        {
            InitializeComponent();
            _speechToText = speechToText;
            _geminiService = geminiService;
            _ttsService = tTSService;
        }

        private async void OnStartListeningClicked(object sender, EventArgs e)
        {
            await Stop(); 

            if (_speechToText.CurrentState == SpeechToTextState.Stopped)
            {
                var isGranted = await _speechToText.RequestPermissions();
                if (!isGranted)
                    return;

                tokenSource = new CancellationTokenSource();

                _speechToText.RecognitionResultUpdated += RecognitionResultUpdated;
                _speechToText.RecognitionResultCompleted += RecognitionResultCompleted;

                try
                {
                    tokenSource.CancelAfter(TimeSpan.FromMinutes(10));
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        try
                        {
                            StartButton.IsEnabled = false;
                            StopButton.IsEnabled = true;

                            //await _speechToText.StartListenAsync(CultureInfo.GetCultureInfo("en-US"), tokenSource.Token);
                            var recognitionResult = await SpeechToText.ListenAsync(
                                                                CultureInfo.GetCultureInfo("en-US"),
                                                                new Progress<string>(partialText =>
                                                                {

                                                                }), tokenSource.Token);
                            await Stop();
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", ex.Message, "OK");
                        }
                    });
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }

        private async void OnStopListeningClicked(object sender, EventArgs e)
        {
            await Stop();
        }

        private void RecognitionResultUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TranscriptionEditor.Text = e.RecognitionResult;
            });
        }

        private async void RecognitionResultCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs e)
        {
            await Stop();

            var input = e.RecognitionResult?.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                await DisplayAlert("Warning", "No input detected.", "OK");
                return;
            }

            TranscriptionEditor.Text += input;

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("stop", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                await _ttsService.SpeakAsync("Goodbye, see you soon!");
                return;
            }

            string response = await _geminiService.GetGeminiReplyAsync(input);
            TranscriptionEditor.Text += "\n\nBot: " + response;
            await _ttsService.SpeakAsync(response);
        }

        private async Task Stop()
        {
            if (_speechToText.CurrentState == SpeechToTextState.Listening)
            {
                _speechToText.RecognitionResultCompleted -= RecognitionResultCompleted;
                _speechToText.RecognitionResultUpdated -= RecognitionResultUpdated;
                await _speechToText.StopListenAsync(tokenSource.Token);
            }

            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }
    }
}
