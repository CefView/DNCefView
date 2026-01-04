using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Labs.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DNCefView.Avalonia.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnCefQueryRequest(int browserId, string frameId, CefQuery query)
        {
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var dialog = new ContentDialog
                {
                    Title = "OnCefQueryRequest",
                    Content = query.Request,
                    PrimaryButtonText = "OK"
                };

                await dialog.ShowAsync();

                query.SetResponseResult(true, query.Request.ToUpper(), 0);
                LocalCefview.ResponseQCefQuery(query);
            });
        }

        private void OnInvokeMethod(int browserId, string frameId, string method, List<dynamic> arguments)
        {
            var content = "";
            content += $"Method Name: {method}\n";
            content += $"Arguments: \n";
            content += $"{JsonSerializer.Serialize(arguments, new JsonSerializerOptions { WriteIndented = true })}";

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var dialog = new ContentDialog
                {
                    Title = "OnInvokeMethod",
                    Content = content,
                    PrimaryButtonText = "OK"
                };

                await dialog.ShowAsync();
            });
        }

        private void BtnChangeBGColor_Click(object sender, RoutedEventArgs e)
        {
            // create a random color
            Random r = new Random();
            var color = Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 233));

            List<object> arguments = new List<object>();
            arguments.Add(string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B));
            arguments.Add(123.4f);
            arguments.Add(0);

            //broadcast the event to all frames in all browsers created by this QCefView widget
            LocalCefview.TriggerEvent("colorChange", arguments, CefBrowser.AllFrameID);
        }
    }
}