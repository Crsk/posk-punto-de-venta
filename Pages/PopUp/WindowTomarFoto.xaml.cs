using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using System.Windows.Interop;
using WPFMediaKit.DirectShow.Controls;
using System.Windows.Media;
using System.Linq;
using posk.Controls;

namespace posk.Pages.PopUp
{
    public partial class WindowTomarFoto : Window
    {
        private string rutaImagen;
        private ItemFoto itemFoto;
        private string nombreArchivo;

        public WindowTomarFoto(string rutaImagen, ItemFoto itemFoto, string nombreArchivo)
        {
            InitializeComponent();
            Eventos();
            this.rutaImagen = rutaImagen;
            this.itemFoto = itemFoto;
            this.nombreArchivo = nombreArchivo;
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
            public string FullName
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


        private void Eventos()
        {
            cbCameras.SelectionChanged += (se, ev) =>
            {
                captureElement.VideoCaptureSource = (string)cbCameras.SelectedItem;
            };
            Loaded += (se, ev) =>
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

            btnCerrar.Click += (se, a) => Cerrar();

            btnGuardar.Click += (se, a) =>
            {
                ImageInfo imageInfo = new ImageInfo();
                {
                    imageInfo.Path = rutaImagen;
                    imageInfo.Name = nombreArchivo;
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
                            itemFoto.imagen.Source = imageBox2.Source;
                            itemFoto.NombreFoto = imageInfo.FullName;
                            Cerrar();
                        }
                    }
                }
            };
            btnBuscarImagen.Click += (se, ev) =>
            {
                string nombreImagen;
                System.Windows.Forms.OpenFileDialog file = new System.Windows.Forms.OpenFileDialog();
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    nombreImagen = file.SafeFileName;
                    //this.tbImageFromXaml.Text = path;
                    BitmapImage imageFromDialog = new BitmapImage();
                    imageFromDialog.BeginInit();
                    imageFromDialog.UriSource = new Uri(@rutaImagen + nombreImagen);
                    imageFromDialog.EndInit();
                    itemFoto.imagen.Source = imageFromDialog;
                    itemFoto.NombreFoto = nombreImagen;
                    Cerrar();
                }
            };


            btnCapturar.Click += (se, a) => 
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
            };
            btnCerrar.Click += (se, a) => Cerrar();
        }



        public static System.Drawing.Image ResizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
        {
            return (System.Drawing.Image)(new Bitmap(imgToResize, size));
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

        private void Cerrar()
        {
            captureElement.Stop();
            Close();
        }
    }
}
