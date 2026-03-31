# DNCefView

`DNCefView` is the core .NET wrapper around CefView and Chromium Embedded Framework (CEF).

It provides the low-level browser, context, settings, and runtime loading pieces used by the UI packages.

## Package Contents

- `CefContext` for global CEF initialization and lifetime management
- `CefConfig` for process-wide configuration
- `CefSetting` for per-browser settings
- `CefBrowser` for browser operations and callbacks
- runtime assets for supported desktop platforms

## Supported Platforms

- Windows x64
- macOS arm64 & x64
- Linux (TODO)

## Basic Usage

Create and keep a single `CefContext` alive for your application:

```csharp
using DNCefView;

var config = new CefConfig();
config.SetMultiThreadedMessageLoop(true);

var context = new CefContext(config);
```

UI applications typically consume this package through a higher-level control package such as `DNCefView.Avalonia`.

## Related Packages

- `DNCefView.Avalonia` for Avalonia UI
- `DNCefView.WPF` for WPF

## Source

Repository: <https://github.com/CefView/DNCefView>
