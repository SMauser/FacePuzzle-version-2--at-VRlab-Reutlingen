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
using System.Threading;
using System.Drawing.Imaging;
using System.Windows.Threading;
using System.ComponentModel;
using Nui;
using Coding4Fun.Kinect;
using Coding4Fun.Kinect.Wpf;
using Microsoft.Expression;
using Microsoft.Expression.Media;
using Microsoft.Expression.Controls;
using Microsoft.Expression.BlendSDK;
using Microsoft.Expression.Shapes;
using Microsoft.Expression.Drawing;
using Microsoft.Kinect;
using System.Runtime.InteropServices;


namespace KinectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {
        public byte[] pixelData;
        NuiSensor _sensor;
        KinectSensor kinectsensor;
        BackgroundWorker _worker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

          
            kinectsensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            kinectsensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(nui_VideoFrameReady);
          

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
       
            Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

            _sensor = new NuiSensor("SamplesConfig.xml");

            _worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
        }
        

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                imgRaw.Source = _sensor.RawImageSource;
                imgDepth.Source = _sensor.DepthImageSource;
            });
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (!_worker.IsBusy)
            {
                _worker.RunWorkerAsync();
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _sensor.Dispose();
        }

        private void BtnToggleVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (imgDepth.IsVisible)
            {
                imgDepth.Visibility = System.Windows.Visibility.Hidden;
                btnToggleVisibility.Content = "Show depth image";
            }
            else
            {
                imgDepth.Visibility = System.Windows.Visibility.Visible;
                btnToggleVisibility.Content = "Show raw image";
            }
        }


public void nui_VideoFrameReady(object sender, Microsoft.Kinect.ColorImageFrameReadyEventArgs e)
{
       
        bool receivedData= false;

        using (ColorImageFrame colorImageFrame = e.OpenColorImageFrame()) 
        {

          if (colorImageFrame != null)
          {
            if( pixelData == null)
            //allocate the first time
            {
             pixelData = new byte[colorImageFrame.PixelDataLength];
            }

            colorImageFrame.CopyPixelDataTo(pixelData);
            receivedData = true;
         }
           else
          {
            // apps processing of image data is taking too long, it got more than 2 frames behind.
            // the data is no longer avabilable.
          }
       }

    if (receivedData)
    {
        // DISPLAY OR PROCESS IMAGE DATA IN pixelData HERE
    }



 
          /*  System.Drawing.Bitmap bitmap;
            ColorImageFrame image = e.OpenColorImageFrame();
            bitmap = ImageToBitmap(image);
            
            
            bitmap.Save(DateTime.Now.ToString("ddMMyyyy HHmmss") + ".bmp");*/
}

private void Button_Click_1(object sender, RoutedEventArgs e)
{

}

        /*  System.Drawing.Bitmap ImageToBitmap(ColorImageFrame Image)
        {
            byte[] pixeldata = new byte[Image.PixelDataLength];
            Image.CopyPixelDataTo(pixeldata);
            System.Drawing.Bitmap bmap = new System.Drawing.Bitmap(Image.Width, Image.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            BitmapData bmapdata = bmap.LockBits(new System.Drawing.Rectangle(0, 0, Image.Width, Image.Height),
            ImageLockMode.WriteOnly,
            bmap.PixelFormat);
            IntPtr ptr = bmapdata.Scan0;
            Marshal.Copy(pixeldata, 0, ptr, Image.PixelDataLength);
            bmap.UnlockBits(bmapdata);
            return bmap;
        }*/
        
    
    }
}
