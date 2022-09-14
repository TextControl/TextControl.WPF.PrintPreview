using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;

namespace tx_wpf_printpreview
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

        

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PrintPreview preview = new PrintPreview();
            preview.TextControl = textControl1;

            preview.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textControl1.Focus();
        }

        private void textControl1_Loaded(object sender, RoutedEventArgs e)
        {
            textControl1.Load("Demo_wpf.rtf", TXTextControl.StreamType.RichTextFormat);
        }
    }
}
