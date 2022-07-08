using DNCef;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

namespace DNCef.WPF.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnCefQueryRequest(int browserId, long frameId, CefQuery query)
        {
            string msg = $"OnCefQueryRequest:{query.Request}";
            MessageBox.Show(msg);
            query.SetResponseResult(true, query.Request.ToUpper(), 0);
            LocalCefview.ResponseQCefQuery(query);
        }

        private void OnInvokeMethod(int browserId, long frameId, string method, List<dynamic> arguments)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string msg = $"OnInvokeMethod:{method}\narguments:\n{JsonSerializer.Serialize(arguments, options)}";
            MessageBox.Show(msg);
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
            LocalCefview.TriggerEventOnMainFrame("colorChange", arguments);
        }
    }
}
