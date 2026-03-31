# DNCefView.Avalonia

`DNCefView.Avalonia` provides an Avalonia `CefView` control powered by CEF.

Use it to embed Chromium-based web content in Avalonia desktop applications.

## Installation

Add the NuGet package:

```bash
dotnet add package DNCefView.Avalonia
```

## Initialize CEF

Create the global `CefContext` once during application startup:

```csharp
using DNCefView;

var config = new CefConfig();
config.SetMultiThreadedMessageLoop(true);

var context = new CefContext(config);
```

## XAML Usage

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:cef="clr-namespace:DNCefView.Avalonia;assembly=DNCefView.Avalonia">
    <cef:CefView Source="https://www.example.com" />
</Window>
```

## Code Usage

`CefSetting` is supplied through the constructor:

```csharp
var setting = new CefSetting();
setting.SetWindowlessFrameRate(60);

var view = new DNCefView.Avalonia.CefView(setting, "https://www.example.com");
```

The native browser is created when the control is attached to the Avalonia visual tree, so constructor settings are preserved even if the view is shown later.

## Notes

- Create `CefContext` before creating any `CefView`
- `CefSetting` is constructor-only
- `Source` can be changed after the control is created

## Source

Repository: <https://github.com/CefView/DNCefView>
