namespace BlazorApp1.Client.Theming;

public class SystemColorSchemeChangedEventArgs : EventArgs
{
    public SystemColorSchemeChangedEventArgs(ColorScheme colorScheme)
    {
        ColorScheme = colorScheme;
    }

    public ColorScheme ColorScheme { get; }
}