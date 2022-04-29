﻿using Blazored.LocalStorage;

using Microsoft.JSInterop;

namespace BlazorApp1.Client.Theming;

public class ThemeManager : IDisposable
{
    private const string PreferredColorSchemeKey = "preferredColorScheme";
    private readonly SystemColorSchemeDetector _systemColorSchemeDetector;
    private readonly ISyncLocalStorageService _localStorage;

    public ThemeManager(SystemColorSchemeDetector systemColorSchemeDetector, ISyncLocalStorageService localStorage)
    {
        _systemColorSchemeDetector = systemColorSchemeDetector;
        _systemColorSchemeDetector.ColorSchemeChanged += _systemColorSchemeDetector_ColorSchemeChanged;
        
        _localStorage = localStorage;
    }

    public void Initialize()
    {
        CurrentColorScheme = PreferredColorScheme ?? _systemColorSchemeDetector.CurrentColorScheme;
    }

    private void _systemColorSchemeDetector_ColorSchemeChanged(object? sender, SystemColorSchemeChangedEventArgs e)
    {
        if (PreferredColorScheme == null)
        {
            CurrentColorScheme = e.ColorScheme;
            ColorSchemeChanged?.Invoke(this, new ColorSchemeChangedEventArgs(e.ColorScheme));
        }
    }
    
    public ColorScheme CurrentColorScheme { get; private set; }

    public ColorScheme? PreferredColorScheme
    {
        get
        {
            return _localStorage.GetItem<ColorScheme?>(PreferredColorSchemeKey);
        }

        private set
        {
            _localStorage.SetItem(PreferredColorSchemeKey, value);
        }
    }

    public void UseSystemScheme()
    {
        PreferredColorScheme = null;
        CurrentColorScheme = _systemColorSchemeDetector.CurrentColorScheme;
        _localStorage.SetItem<ColorScheme?>(PreferredColorSchemeKey, null);
        ColorSchemeChanged?.Invoke(this, new ColorSchemeChangedEventArgs(CurrentColorScheme));
    }

    public void SetPreferredColorScheme(ColorScheme colorScheme)
    {
        PreferredColorScheme = colorScheme;

        if (CurrentColorScheme != colorScheme)
        {
            CurrentColorScheme = colorScheme;

            ColorSchemeChanged?.Invoke(this, new ColorSchemeChangedEventArgs(CurrentColorScheme));

        }
       
        _localStorage.SetItem<ColorScheme?>(PreferredColorSchemeKey, colorScheme);
    }
    
    public event EventHandler<ColorSchemeChangedEventArgs> ColorSchemeChanged = null!;

    public void Dispose()
    {
        _systemColorSchemeDetector.ColorSchemeChanged -= _systemColorSchemeDetector_ColorSchemeChanged;
    }
}