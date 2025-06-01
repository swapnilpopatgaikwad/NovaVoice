namespace NovaVoice.Services;

public class TTSService
{
    public async Task SpeakAsync(string text)
    {
        await TextToSpeech.SpeakAsync(text, new SpeechOptions
        {
            Pitch = 1.0f,
            Volume = 1.0f,
            Locale = await GetEnglishLocale()
        });
    }

    private async Task<Locale> GetEnglishLocale()
    {
        var locales = await TextToSpeech.GetLocalesAsync();
        return locales.FirstOrDefault(l => l.Language.StartsWith("en")) ?? locales.First();
    }
}
