using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using System;

namespace DNCefView.Avalonia
{
    public partial class CefView
    {
        static void ClassInitializeJSDialogs()
        {
        }

        CefViewJSDialog? _jsAlertDialog;
        CefViewJSDialog? _jsConfirmDialog;
        CefViewJSDialog? _jsPromptDialog;

        void InitializeJSDialogs()
        {

        }

        bool UI_ShowJSDialogAlert(string originUrl, string messageText)
        {
            if (_jsAlertDialog == null)
            {
                _jsAlertDialog = CefViewJSDialog.CreateJSDialog(this, CefViewJSDialog.DialogType.Alert);
            }
            return _jsAlertDialog.Show(this, messageText, $"JavaScript Alert - {originUrl}", "", out _);
        }

        bool UI_ShowJSDialogConfirm(string originUrl, string messageText)
        {
            if (_jsConfirmDialog == null)
            {
                _jsConfirmDialog = CefViewJSDialog.CreateJSDialog(this, CefViewJSDialog.DialogType.Confirm);
            }
            return _jsConfirmDialog.Show(this, messageText, $"JavaScript Confirm - {originUrl}", "", out _);
        }

        bool UI_ShowJSDialogPrompt(string originUrl, string messageText, string defaultPromptText, out string? promptResult)
        {
            if (_jsPromptDialog == null)
            {
                _jsPromptDialog = CefViewJSDialog.CreateJSDialog(this, CefViewJSDialog.DialogType.Prompt);
            }
            return _jsPromptDialog.Show(this, messageText, $"JavaScript Prompt - {originUrl}", defaultPromptText, out promptResult);
        }

        sealed class CefViewJSDialog
        {
            public enum DialogType
            {
                Alert = 0,
                Confirm = 1,
                Prompt = 2,
            }

            sealed class DialogResult
            {
                public bool IsAccepted { get; set; }

                public string? PromptResult { get; set; }
            }

            readonly Window _dialog;

            readonly DialogType _type;

            readonly TextBlock _messageTextBlock;

            readonly TextBox? _promptInput;

            readonly DialogResult _result = new();

            CefViewJSDialog(CefView owner, DialogType type)
            {
                _type = type;

                _messageTextBlock = new TextBlock
                {
                    Margin = new Thickness(0, 0, 0, 8),
                    TextWrapping = TextWrapping.Wrap,
                };

                if (_type == DialogType.Prompt)
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
                    Height = _type == DialogType.Prompt ? 200 : 160,
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

            public static CefViewJSDialog CreateJSDialog(CefView owner, DialogType type)
            {
                return new CefViewJSDialog(owner, type);
            }

            public bool Show(CefView owner, string message, string title, string defaultPromptText, out string? promptResult)
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

                var showTask = Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    if (TopLevel.GetTopLevel(owner) is Window ownerWindow)
                    {
                        await _dialog.ShowDialog(ownerWindow);
                    }
                    else
                    {
                        _result.IsAccepted = false;
                        _dialog.Close();
                    }
                }, DispatcherPriority.Normal);

                showTask.GetAwaiter().GetResult();
                promptResult = _result.PromptResult;
                return _result.IsAccepted;
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
                    case DialogType.Alert:
                        panel.Children.Add(CreateButton("OK", OnOk));
                        break;
                    case DialogType.Confirm:
                    case DialogType.Prompt:
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
}
