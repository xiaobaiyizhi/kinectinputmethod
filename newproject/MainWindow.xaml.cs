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
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit.Interaction;
using Microsoft.Kinect.Toolkit;
using System.Windows.Media.Animation;
using System.Windows.Ink;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Models;
using System.Threading;

namespace newproject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class KinectInputPanel : Window
    {
        
        public int isstoryboard = 0;
		Storyboard myStoryboard;
		Models.Kinect kinect;

        public static KinectInputPanel kinectInputpanel = null;
        public static KinectRegion MianwdKinectRegion=null;

		private int currentLanguage;//当前识别语言

        public KinectInputPanel(Models.Kinect _kinect)
        {
            InitializeComponent();
            //窗口最大化 不遮挡任务栏
            //FullScreenManager.RepairWpfWindowFullScreenBehavior(this);
            this.ShowInTaskbar = false;
			this.Topmost = true;
            Loaded += OnLoaded;
			kinect = _kinect;
            kinectInputpanel = this;
			//隐藏鼠标
			Mouse.OverrideCursor = Cursors.None;
			currentLanguage=Models.Lang.ChsLanguageId;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
 
            MianwdKinectRegion = this.kinectRegion;
        
			this.Width = System.Windows.Forms.SystemInformation.VirtualScreen.Width;
			this.Height = System.Windows.Forms.SystemInformation.VirtualScreen.Height;
			kinect.mainIntPtr = BLL.FocusSetting.GetIntPtr("KinectInputPanel");
			((MiniWindow)this.Owner).KinectRenew(kinect);
			BLL.FocusSetting.SetNoFocus("KinectInputPanel");
            kinectinkcanvas.Clip = path2.Data;
            kinectinkcanvas.DefaultDrawingAttributes.Width = 7;
            kinectinkcanvas.DefaultDrawingAttributes.Height = 7;
            if (kinectinkcanvas.IsGestureRecognizerAvailable)//待封装
            {
                kinectinkcanvas.EditingMode = InkCanvasEditingMode.InkAndGesture;
                kinectinkcanvas.Gesture += new InkCanvasGestureEventHandler(inkCanvas1_Gesture);
                kinectinkcanvas.SetEnabledGestures(new ApplicationGesture[] 
                    {ApplicationGesture.Down, 
                     ApplicationGesture.ArrowDown,
                     ApplicationGesture.Right,
                    ApplicationGesture.Left});
            }
            //动画
            myStoryboard = (Storyboard)this.Resources["Storyboard1"];
            myStoryboard.Begin(this);
            isstoryboard = 1;
        }

        private void inkCanvas1_Gesture(object sender, InkCanvasGestureEventArgs e)
        {
			string gesture=BLL.GestureRecognize.InkGestureResult(e.GetGestureRecognitionResults());
           // MessageBox.Show(gesture);
            if (gesture=="space")
            {
               // if (System.Windows.Forms.SendKeys!=null) 存在bug
                   System.Windows.Forms.SendKeys.SendWait(" ");
               
 
                   

                kinectinkcanvas.Strokes.Clear();
            }
            else if (gesture == "circle")
            {
                kinectinkcanvas.Strokes.Clear();
            }
            else if (gesture == "backspace")
            {
                try
                {
                    System.Windows.Forms.SendKeys.SendWait("{backspace}");
                }
                catch (System.Exception ex)
                {
                	
                }
               
                kinectinkcanvas.Strokes.Clear();
            }

        }


      

        private void kinectTileButton_Click(object sender, RoutedEventArgs e)
        {
            //sensorChooser.KinectChanged -= SensorChooserOnKinectChanged;
            if (isstoryboard == 1)
            {
                myStoryboard = (Storyboard)this.Resources["Storyboar2"];
                myStoryboard.Begin(this);
                isstoryboard = 0;

            }
            else
            {
                myStoryboard = (Storyboard)this.Resources["Storyboard1"];
                myStoryboard.Begin(this);
                isstoryboard = 1;
            }
        }

        private void KinectTileButton_Click_1(object sender, RoutedEventArgs e)
        {
			currentLanguage = (currentLanguage == Lang.ChsLanguageId) ?  Lang.EnLanguageId:Lang.ChsLanguageId;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (kinect.kinectSensor != null)
                kinect.kinectSensor.Stop();
        }
        public struct ob
        {
               public StrokeCollection strokecollection;
               public int currentlang;
        }

        public delegate IEnumerable<string> MyDelegate(StrokeCollection stroke, int currentlang);
		private void KinectTileButton_Click_2(object sender, RoutedEventArgs e)
		{
			//sensorChooser.KinectChanged -= SensorChooserOnKinectChanged;
			if (kinectinkcanvas.Strokes.Count > 0)
			{
                scrollContent.Children.Clear();
              // Thread reg = new Thread(new ThreadStart(Threadfun));

                MyDelegate dele = new MyDelegate(regcallback);
                Console.WriteLine(this.kinectinkcanvas.Strokes.ToString());
                Console.WriteLine(currentLanguage.ToString());
                IAsyncResult ref1=dele.BeginInvoke(this.kinectinkcanvas.Strokes, currentLanguage,null,null);
               // reg.Start();
 

                while (!ref1.IsCompleted)
                {
                    Thread.Sleep(2000);
                }
                IEnumerable<string> theAlternateCollection = dele.EndInvoke(ref1);
                foreach (string alternateString in theAlternateCollection)
                {
                    int stringlenth = alternateString.Length;
                    int backwidth;
                    if (stringlenth > 4)
                        backwidth = (stringlenth + 1) * 25;
                    else
                        backwidth = (4 + 1) * 25;

                    var button = new KinectTileButton
                    {
                        Content = alternateString,
                        Height = 120,
                        BorderThickness = new Thickness(2),
                        Width = backwidth,

                    };
                    button.BorderThickness = new Thickness(2);
                    button.BorderBrush = Brushes.Black;
                    button.Background = Brushes.White;
                    button.Click += new RoutedEventHandler(button_Click);
                    scrollContent.Children.Add(button);
                }

                kinectinkcanvas.Strokes.Clear();
	

				kinectinkcanvas.Clip = path1.Data;
			}
		}


        IEnumerable<string> regcallback(StrokeCollection stroke, int currentlang)
        {

            Console.WriteLine(stroke.ToString());
            Console.WriteLine(currentlang.ToString());
            IEnumerable<string> theAlternateCollection = BLL.GestureRecognize.CharactersResult(stroke, currentlang);

            return theAlternateCollection;
           
        }
		void button_Click(object sender, RoutedEventArgs e)
		{
			BLL.SendText.SendToForeWin(((KinectTileButton)sender).Content.ToString());
		}


        private void KinectTileButton_Click_3(object sender, RoutedEventArgs e)
        {
			BLL.FocusSetting.SetVisible(kinect.mainIntPtr, false);
			//this.Visibility = Visibility.Hidden;
			BLL.FocusSetting.SetVisible(kinect.miniIntPtr, true);
           // MiniWindow.miniwindow.kinectRegion1.KinectSensor = this.kinectRegion.KinectSensor;
            MiniWindow.miniwindow.kinectRegion1.KinectSensor = this.kinectRegion.KinectSensor;
            this.kinectRegion.KinectSensor = null;



           
    


        }



    }
}

