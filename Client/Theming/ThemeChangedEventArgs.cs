namespace BlazorApp1.Client.Theming;

public class ThemeChangedEventArgs : EventArgs
{
    public ThemeChangedEventArgs(Theme theme)
    {
        Theme = theme;
    }

    public Theme Theme { get; }
}
