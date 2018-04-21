using System;
using System.Windows;
using controls = System.Windows.Controls;
using System.Windows.Forms;
using WPFCSharpWebCam;
using System.Windows.Media.Imaging;
using System.IO;
using System.Configuration;
using posk.Globals;
using System.Drawing;
using System.Windows.Interop;


using Microsoft.Win32;

using WebEye.Controls.Wpf;


using WPFMediaKit.DirectShow.Controls;
using System.Windows.Media;
using System.Linq;

namespace posk.Pages.PopUp
{
    public partial class WindowChooseOrTakeImage : Window
    {

        //private controls.TextBox tbImageFromXaml;

        // lo necesito para que guarde el nombre de la imagen dentro del control Image, con tal de actualizar la imagen en la base de datos
        private ItemName_Image imageFromXaml;
        private string fileNameFromXaml;
        private string imgPath;
        //private WebCam webcam;

        //public event EventHandler OnFinish;

        public static System.Drawing.Image ResizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
        {
            return (System.Drawing.Image)(new Bitmap(imgToResize, size));
        }

        public WindowChooseOrTakeImage(string imgPath, ItemName_Image image, string nombre)
        {
            InitializeComponent();
            //bCalledFromProductsPage = true;
            //this.tbImageFromXaml = _txtImagen;
            this.imageFromXaml = image;
            this.fileNameFromXaml = nombre;
            //webcam = new WebCam();
            //webcam.InitializeWebCam(ref imageFromVideo);
            //webcam.Start();

            this.imgPath = imgPath;

            //this.Deactivated += (se, ev) => { if (!bCerrado && !bPreventClosingPopUp) { webcam.Stop(); Close(); } };
            this.Deactivated += (se, ev) => { this.Topmost = true; };

            btnCerrar.Click += (se, ev) => { /*bCerrado = true;*/ /*webcam.Stop();*/ captureElement.Stop(); Close(); };

            btnBuscarImagen.Click += (se, ev) =>
            {
                string path;
                System.Windows.Forms.OpenFileDialog file = new System.Windows.Forms.OpenFileDialog();
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = file.SafeFileName;
                    //this.tbImageFromXaml.Text = path;
                    imageFromXaml._name = path;
                    imageFromXaml.ToolTip = imageFromXaml._name;
                    BitmapImage imageFromDialog = new BitmapImage();
                    imageFromDialog.BeginInit();
                    imageFromDialog.UriSource = new Uri(@imgPath + path);
                    imageFromDialog.EndInit();
                    imageFromXaml.Source = imageFromDialog;
                    Cerrar();
                }
            };

            cbCameras.SelectionChanged += (se, ev) =>
            {
                captureElement.VideoCaptureSource = (string)cbCameras.SelectedItem;
            };
            this.Loaded += (se, ev) =>
            {
                cbCameras.ItemsSource = MultimediaUtil.VideoInputNames;
                if (MultimediaUtil.VideoInputNames.Count() > 0)
                {
                    cbCameras.SelectedIndex = 0;
                }
                else
                {
                    System.Windows.MessageBox.Show("No Camera!");
                }
            };
        }

        private void btnCerrar_Click_1(object sender, RoutedEventArgs e)
        {
            Cerrar();
        }

        private void Cerrar()
        {
            //bCerrado = true;
            //webcam.Stop();
            captureElement.Stop();
            Close();
            //OnFinish(this, null);
        }

        private void btnBuscarImagen_Click(object sender, RoutedEventArgs e)
        {

        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        private BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapImage retval;

            try
            {
                retval = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }

        public byte[] CaptureData { get; set; }
        private void btnCapturar_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)captureElement.ActualWidth, (int)captureElement.ActualHeight, 96, 96, PixelFormats.Default);
            bmp.Render(captureElement);
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                CaptureData = ms.ToArray();

            }
            imageBox2.Source = bmp;
        }

        public struct ImageInfo
        {
            public string Path;
            public string Name;
            public string Extension;
            public int Version;
            public string FullPath
            {
                get
                {
                    if (Version == 1)
                        return Path + Name + Extension;
                    else
                        return Path + Name + "(" + Version + ")" + Extension;
                }
            }
            public string NameToShow
            {
                get
                {
                    if (Version == 1)
                        return Name + Extension;
                    else
                        return Name + "(" + Version + ")" + Extension;
                }
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            ImageInfo imageInfo = new ImageInfo();
            {
                imageInfo.Path = imgPath;
                imageInfo.Name = fileNameFromXaml;
                imageInfo.Version = 1;
                imageInfo.Extension = ".jpg";
            }

            if (File.Exists(imageInfo.FullPath))
            {
                for (int i = 2; i < 100; i++)
                {
                    if (File.Exists(imageInfo.FullPath))
                        imageInfo.Version++;
                    else break;
                }
            }

            if (!File.Exists(imageInfo.FullPath))
            {
                using (var fileStream = new FileStream(imageInfo.FullPath, FileMode.Create))
                {
                    if (imageBox2.Source != null)
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageBox2.Source));
                        encoder.Save(fileStream);
                        //webcam.Stop();
                        //webCameraControl1.StopCapture();
                        captureElement.Stop();

                        imageFromXaml._name = imageInfo.NameToShow;
                        imageFromXaml.ToolTip = imageFromXaml._name;
                        imageFromXaml.Source = imageBox2.Source;
                        Cerrar();
                    }
                }
            }
        }

        private void Tb_TextChanged(object sender, controls.TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnEscogerCam_Click(object sender, RoutedEventArgs e)
        {
            //webcam.AdvanceSetting();
        }
    }
}
