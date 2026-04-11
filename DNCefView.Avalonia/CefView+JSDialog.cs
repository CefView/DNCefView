using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace DNCefView.Avalonia;

public partial class CefView
{
    static void ClassInitializeJSDialogs()
    {
    }

    void InitializeJSDialogs()
    {
    }

    bool UI_ShowCefJSDialog(int browserId, IntPtr dialogHandle, string originUrl, int dialogType, string messageText, string defaultPromptText, bool suppressMessage)
    {
        if (suppressMessage)
        {
            return false;
        }

        switch ((CefViewJSDialog.CefDialogType)dialogType)
        {
            case CefViewJSDialog.CefDialogType.ALERT:
                UI_ShowJSDialogAlert(dialogHandle, originUrl, messageText);
                break;
            case CefViewJSDialog.CefDialogType.CONFIRM:
                UI_ShowJSDialogConfirm(dialogHandle, originUrl, messageText);
                break;
            case CefViewJSDialog.CefDialogType.PROMPT:
                UI_ShowJSDialogPrompt(dialogHandle, originUrl, messageText, defaultPromptText);
                break;
            default:
                return false;
        }

        return true;
    }

    void UI_ShowJSDialogAlert(IntPtr dialogHandle, string originUrl, string messageText)
    {
        RunInUIThread(() =>
        {
            var dialog = CefViewJSDialog.CreateJSDialog(this, CefViewJSDialog.CefDialogType.ALERT);
            dialog.ShowAsync(this, dialogHandle, messageText, $"JavaScript Alert - {originUrl}", "");
        },
        block: false);
    }

    void UI_ShowJSDialogConfirm(IntPtr dialogHandle, string originUrl, string messageText)
    {
        RunInUIThread(() =>
        {
            var dialog = CefViewJSDialog.CreateJSDialog(this, CefViewJSDialog.CefDialogType.CONFIRM);
            dialog.ShowAsync(this, dialogHandle, messageText, $"JavaScript Confirm - {originUrl}", "");
        },
        block: false);
    }

    void UI_ShowJSDialogPrompt(IntPtr dialogHandle, string originUrl, string messageText, string defaultPromptText)
    {
        RunInUIThread(() =>
        {
            var dialog = CefViewJSDialog.CreateJSDialog(this, CefViewJSDialog.CefDialogType.PROMPT);
            dialog.ShowAsync(this, dialogHandle, messageText, $"JavaScript Prompt - {originUrl}", defaultPromptText);
        },
        block: false);
    }

    sealed class CefViewJSDialog
    {
        public enum CefDialogType
        {
            ALERT = 0,
            CONFIRM = 1,
            PROMPT = 2,
        }

        private sealed class CefDialogResult
        {
            public bool IsAccepted { get; set; }

            public string? PromptResult { get; set; }
        }

        readonly Window _dialog;

        readonly CefDialogType _type;

        readonly TextBlock _messageTextBlock;

        readonly TextBox? _promptInput;

        readonly CefDialogResult _result = new();

        CefViewJSDialog(CefView owner, CefDialogType type)
        {
            _type = type;

            _messageTextBlock = new TextBlock
            {
                Margin = new Thickness(0, 0, 0, 8),
                TextWrapping = TextWrapping.Wrap,
            };

            if (_type == CefDialogType.PROMPT)
            {
                _promptInput = new TextBox
                {
                    MinWidth = 320
                };
            }

            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(16),
                Spacing = 8,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            panel.Children.Add(_messageTextBlock);
            if (_promptInput != null)
            {
                panel.Children.Add(_promptInput);
            }

            panel.Children.Add(CreateButtonPanel());

            _dialog = new Window
            {
                Width = 420,
                Height = _type == CefDialogType.PROMPT ? 200 : 160,
                MinWidth = 360,
                MinHeight = 140,
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = panel
            };

            if (TopLevel.GetTopLevel(owner) is Window ownerWindow)
            {
                _dialog.Icon = ownerWindow.Icon;
            }
        }

        public static CefViewJSDialog CreateJSDialog(CefView owner, CefDialogType type)
        {
            return new CefViewJSDialog(owner, type);
        }

        public async void ShowAsync(CefView owner, IntPtr dialogHandle, string message, string title, string defaultPromptText)
        {
            _result.IsAccepted = false;
            _result.PromptResult = null;
            _messageTextBlock.Text = message;
            _dialog.Title = title;

            if (_promptInput != null)
            {
                _promptInput.Text = defaultPromptText;
                _promptInput.SelectAll();
            }

            if (TopLevel.GetTopLevel(owner) is Window ownerWindow)
            {
                await _dialog.ShowDialog(ownerWindow);
            }
            else
            {
                _result.IsAccepted = false;
                _dialog.Hide();
            }

            owner._cefBrowser?.ContinueJSDialog(dialogHandle, _result.IsAccepted, _result.PromptResult ?? string.Empty);
        }

        Panel CreateButtonPanel()
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Spacing = 8
            };

            switch (_type)
            {
                case CefDialogType.ALERT:
                    panel.Children.Add(CreateButton("OK", OnOk));
                    break;
                case CefDialogType.CONFIRM:
                case CefDialogType.PROMPT:
                    panel.Children.Add(CreateButton("OK", OnOk));
                    panel.Children.Add(CreateButton("Cancel", OnCancel));
                    break;
            }

            return panel;
        }

        static Button CreateButton(string text, EventHandler<RoutedEventArgs> onClick)
        {
            var button = new Button
            {
                Content = text,
                MinWidth = 80
            };
            button.Click += onClick;
            return button;
        }

        void OnOk(object? sender, RoutedEventArgs e)
        {
            _result.IsAccepted = true;
            _result.PromptResult = _promptInput?.Text;
            _dialog.Close();
        }

        void OnCancel(object? sender, RoutedEventArgs e)
        {
            _result.IsAccepted = false;
            _result.PromptResult = null;
            _dialog.Close();
        }
    }
}