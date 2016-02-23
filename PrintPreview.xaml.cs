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
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using System.Windows.Markup;
using System.Drawing.Imaging;

namespace tx_wpf_printpreview
{
    /// <summary>
    /// Interaction logic for PrintPreview.xaml
    /// </summary>
    public partial class PrintPreview : Window
    {
        public TXTextControl.WPF.TextControl TextControl;

        public PrintPreview()
        {
            InitializeComponent();
        }

        #region "Bitmap Helper Functions"

        private System.Windows.Media.Imaging.BitmapSource GetBitmapSource(System.Drawing.Bitmap _image)
        {
            System.Drawing.Bitmap bitmap = _image;

            System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            
            return bitmapSource;
        }

        public static BitmapImage BitmapSourceToBitmapImage(BitmapSource bitmapSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
            bImg.EndInit();

            memoryStream.Close();

            return bImg;
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.PrintQueue =  System.Printing.LocalPrintServer.GetDefaultPrintQueue();
            pd.PrintTicket = pd.PrintQueue.DefaultPrintTicket;

            // set default paper size or remove this line to use the default size
            pd.PrintTicket.PageMediaSize = new System.Printing.PageMediaSize(System.Printing.PageMediaSizeName.NorthAmericaLetter);

            // create a new FixedDocument that is used as a container
            FixedDocument document = new FixedDocument();

            document.DocumentPaginator.PageSize = new System.Windows.Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);

            foreach (TXTextControl.Page page in TextControl.GetPages())
            {
                FixedPage page1 = new FixedPage();

                // landscape?
                if (TextControl.Sections[page.Section].Format.Landscape == true)
                {
                    page1.Width = document.DocumentPaginator.PageSize.Height;
                    page1.Height = document.DocumentPaginator.PageSize.Width;
                }
                else
                {
                    page1.Width = document.DocumentPaginator.PageSize.Width;
                    page1.Height = document.DocumentPaginator.PageSize.Height;
                }

                try
                {
                    // we create a bitmap representation of the page due to TX Text Control's
                    // special WYSWIYG rendering. 
                    Bitmap pageContent = page.GetImage(100, TXTextControl.Page.PageContent.All);
                    BitmapSource bmp = GetBitmapSource(pageContent);
                    BitmapImage bmpImg = BitmapSourceToBitmapImage(bmp);

                    // create a new image control for each page
                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source = bmpImg;

                    page1.Children.Add(img);
                     
                    PageContent page1Content = new PageContent();

                    // add the content to the current page
                    ((IAddChild)page1Content).AddChild(page1);
                    document.Pages.Add(page1Content);
                }
                catch(Exception exc)
                {
                    throw exc;
                }
            }

            // display the document at 50% zoom
            documentViewer1.Document = document;
            documentViewer1.Zoom = 50;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextControl.Print("Doc", true);
        }
    }
}

