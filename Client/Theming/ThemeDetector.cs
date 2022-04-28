using Microsoft.JSInterop;

namespace BlazorApp1.Client.Theming;

public class ThemeDetector : IDisposable
{
    private readonly IJSRuntime _jsRuntime;

    public ThemeDetector(IJSRuntime jSRuntime)
    {
        _jsRuntime = jSRuntime;

        Internal.ThemeChanged += Internal_ThemeChanged;
    }

    private void Internal_ThemeChanged(object? sender, ThemeChangedEventArgs e)
    {
        ThemeChanged?.Invoke(this, e);
    }

    public event EventHandler<ThemeChangedEventArgs> ThemeChanged = null!;

    public void Dispose()
    {
        Internal.ThemeChanged -= Internal_ThemeChanged;
    }

    public async Task<Theme> GetCurrentThemeAsync() => await _jsRuntime.InvokeAsync<bool>("isDarkMode") ? Theme.Dark : Theme.Light;
    
    public static class Internal
    {
        [JSInvokable("OnDarkModeChanged")]
        public static void OnDarkModeChanged(bool isDarkMode)
        {
            ThemeChanged?.Invoke(null, new ThemeChangedEventArgs(isDarkMode ? Theme.Dark : Theme.Light));
        }

        public static event EventHandler<ThemeChangedEventArgs> ThemeChanged = null!;
    }
}
