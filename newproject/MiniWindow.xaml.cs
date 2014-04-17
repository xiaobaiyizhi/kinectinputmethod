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
using System.Runtime.InteropServices;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using Models;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Windows.Threading;

namespace newproject
{
    /// <summary>
    /// MiniWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MiniWindow : Window
    {

        public MiniWindow()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Topmost = true;
			Loaded += new RoutedEventHandler(Window_Loaded);
        }
        public static MiniWindow miniwindow = null;
        public static KinectRegion MiniwdKinectRegion = null;
		Magnifier.MagnifierForm magnifier;
		private KinectSensorChooser sensorChooser;
		Models.Kinect kinect;
        public Buttonevent btevent = new Buttonevent();
        int timerstate;
		int mainOperatorSkeletonId;//定位主要操作人

		InteractionClient ic;
		InteractionStream its;
        //public static MiniWindow miniwindow =null;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			//放大镜
			//magnifier = new Magnifier.MagnifierForm();
			//magnifier.Show();

			this.sensorChooser = new KinectSensorChooser();
            miniwindow = this;

            this.WheelupTime();
            this.WheeldownTime();

            this.btevent.Time();
            btevent.ButteneventStop();
            //this.Width = SystemParameters.WorkArea.Width;
            //this.Height = SystemParameters.WorkArea.Height;
            this.kinectRegion1.Height = SystemParameters.WorkArea.Height;
            this.kinectRegion1.Width = SystemParameters.WorkArea.Width;
            this.Left = 0;
            this.Top = 0;
			this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
			this.sensorChooser.Start();
			UserViewer.PrimaryUserColor = Colors.Black;//操作中的人的颜色

			IntPtr hwnd = BLL.FocusSetting.SetNoFocus("MiniWindow");
			kinect = new Models.Kinect(this.sensorChooser.Kinect, IntPtr.Zero, hwnd);

            //ButtonWindow.bt.WindowStartupLocation = WindowStartupLocation.Manual;
            //ButtonWindow.bt.Visibility = Visibility.Hidden;

			
        }

		void its_InteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
		{
			//throw new NotImplementedException();
			using (InteractionFrame IFrame = e.OpenInteractionFrame())
			{
				if (IFrame == null)
					return;
				UserInfo[] userInfos = new UserInfo[InteractionFrame.UserInfoArrayLength];
				IFrame.CopyInteractionDataTo(userInfos);
				if (userInfos == null)
					return;
				//是否改变侦测人，避免干扰
				bool changeOperator = true;
				int tempSkeletonId = 0;
				//扫描所有人
				for (int currentUserIndex = 0; currentUserIndex < userInfos.Length; currentUserIndex++)
				{
					int playIndex = currentUserIndex;
					UserInfo userInfo = userInfos[currentUserIndex];
					int skeletonTrackingId = userInfo.SkeletonTrackingId;

					foreach (InteractionHandPointer interactionHandPointer in userInfo.HandPointers)
					{
						if (interactionHandPointer.HandType == InteractionHandType.None && interactionHandPointer.IsPrimaryForUser == false && interactionHandPointer.IsActive == false && interactionHandPointer.IsInteractive == false && interactionHandPointer.IsTracked == false)
						{
							continue;
						}
						if (interactionHandPointer.IsTracked == true)
						{
							if (userInfo.SkeletonTrackingId == mainOperatorSkeletonId)
							{
								changeOperator = false;
								if (interactionHandPointer.HandType == InteractionHandType.Right && interactionHandPointer.IsActive == true)
									textBox2.Text = "RightAc:" + interactionHandPointer.X.ToString() + "," + interactionHandPointer.Y.ToString();
							}
							else
								tempSkeletonId = userInfo.SkeletonTrackingId;
						}
					}
				}
				if (changeOperator)
					mainOperatorSkeletonId = tempSkeletonId;
			}
		}


		private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
		{
			 //MessageBox.Show(args.NewSensor == null ? "No Kinect" : args.NewSensor.Status.ToString());
			 bool error = false;
			 //this.buttom.Content = sensorChooser.Kinect.Status.ToString();
			 //MessageBox.Show(args.NewSensor == null ? "No Kinect" : args.NewSensor.Status.ToString());
			 if (args.OldSensor != null)
			 {
				 try
				 {
					 args.OldSensor.DepthStream.Range = DepthRange.Default;
					 args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
					 args.OldSensor.DepthStream.Disable();
					 args.OldSensor.SkeletonStream.Disable();
				 }
				 catch (InvalidOperationException)
				 {
					 // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
					 // E.g.: sensor might be abruptly unplugged.
					 error = true;
				 }
			 }
			 if (args.NewSensor != null)
			 {
				 try
				 {
					 var parameters = new TransformSmoothParameters
					 {
						 Smoothing = 0.7f,// 设置处理骨骼数据帧时的平滑量，接受一个0-1的浮点值，值越大，平滑的越多。0表示不进行平滑。
						 Correction = 0.3f,//接受一个从0-1的浮点型，值越小，修正越多
						 Prediction = 0.4f,// 返回用来进行平滑需要的骨骼帧的数目
						 JitterRadius = 0.01f,// 抖动半径，单位为m，如果关节点“抖动”超过了设置的这个半径，将会被纠正到这个半径之内
						 MaxDeviationRadius = 0.5f// 用来和抖动半径一起来设置抖动半径的最大边界，任何超过这一半径的点都不会认为是抖动产生的，而被认定为是一个新的点。该属性为浮点型，单位为米

					 };
					 args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
					 args.NewSensor.SkeletonStream.Enable(parameters);
					 args.NewSensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(NewSensor_AllFramesReady);
					 ic = new InteractionClient();
					 its = new InteractionStream(args.NewSensor, ic);
					 its.InteractionFrameReady+=new EventHandler<InteractionFrameReadyEventArgs>(its_InteractionFrameReady);
					 //InteractionFrame IF = its.OpenNextFrame(0);
					 //if (IF == null)
					 //    Console.Write("no if");
					 //else
					 //    Console.Write("if");
					 try
					 {
						 args.NewSensor.DepthStream.Range = DepthRange.Near;
						 args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
						 args.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
					 }
					 catch (InvalidOperationException)
					 {
						 // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
						 args.NewSensor.DepthStream.Range = DepthRange.Default;
						 args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
						 error = true;
					 }
				 }
				 catch (InvalidOperationException)
				 {
					 error = true;
					 // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
					 // E.g.: sensor might be abruptly unplugged.
				 }
			 }
			 if (!error)
				 kinectRegion1.KinectSensor = args.NewSensor;

		}

		void NewSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
		{
			//throw new NotImplementedException();
			using (DepthImageFrame DFrame = e.OpenDepthImageFrame())
			{
				if (DFrame != null)
				{
					short[] pixelData = new short[kinect.kinectSensor.DepthStream.FramePixelDataLength];
					DFrame.CopyPixelDataTo(pixelData);
					its.ProcessDepth(DFrame.GetRawPixelData(), DFrame.Timestamp);
				}
				else
					return;

			}
			using (SkeletonFrame SFrame = e.OpenSkeletonFrame())
			{
				if (SFrame != null)
				{
					Skeleton[] skeletions = new Skeleton[SFrame.SkeletonArrayLength];
					Skeleton closetSkeleton = GetClosetSkeleton(SFrame);
					SFrame.CopySkeletonDataTo(skeletions);
					its.ProcessSkeleton(skeletions, kinect.kinectSensor.AccelerometerGetCurrentReading(), SFrame.Timestamp);
				}
				else
					return;
			}
		}

		Skeleton GetClosetSkeleton(SkeletonFrame frame)
		{
			Skeleton[] allSkeletons = new Skeleton[frame.SkeletonArrayLength];
			frame.CopySkeletonDataTo(allSkeletons);
			Skeleton closetSkeleton = (from s in allSkeletons
									   where s.TrackingState == SkeletonTrackingState.Tracked
									   && s.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked
									   select s).OrderBy(s => s.Joints[JointType.Head].Position.Z).FirstOrDefault();
			return closetSkeleton;
		}  

        private void buttom_Click(object sender, RoutedEventArgs e)
        {

			BLL.FocusSetting.SetVisible(kinect.miniIntPtr, false);
			if (kinect.mainIntPtr == IntPtr.Zero)
			{
				KinectInputPanel mainWindow = new KinectInputPanel(kinect);
				mainWindow.kinectRegion.KinectSensor = this.kinectRegion1.KinectSensor;

                this.kinectRegion1.KinectSensor = null;
				mainWindow.Owner = this;
				mainWindow.Show();
                if (timerstate==1)
                {
                    this.timerstate = 0;
                    this.btevent.ButteneventStop();
                }


			}
			else
			{
                KinectInputPanel.kinectInputpanel.kinectRegion.KinectSensor = this.kinectRegion1.KinectSensor;
                this.kinectRegion1.KinectSensor = null;
               // KinectInputPanel.kinectInputpanel.Visibility=Visibility.Hidden;
				BLL.FocusSetting.SetVisible(kinect.mainIntPtr, true);
                if (timerstate==1)
                {
                    this.timerstate = 0;
                    this.btevent.ButteneventStop();
                }

			}
        }

		public void KinectRenew(Models.Kinect _kinect)
		{
			this.kinect = _kinect;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (kinect.kinectSensor != null)
				if (kinect.kinectSensor.Status == KinectStatus.Connected)
					kinect.kinectSensor.Stop();
		}


#region  翻页


        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private DispatcherTimer wheeluptime = new DispatcherTimer();
        private DispatcherTimer wheeldowntime = new DispatcherTimer();
        /// <summary>
        /// 滚轮上滑计时器
        /// </summary>
        public void WheelupTime()
        {
            wheeluptime.Tick += new EventHandler(wheelupdowntime_timer);
            wheeluptime.Interval = new TimeSpan(0, 0, 2);
        }
        /// <summary>
        /// 滚轮下滑计时器
        /// </summary>
        public void WheeldownTime()
        {
            wheeldowntime.Tick += new EventHandler(wheelupdowntime_timer);
            wheeldowntime.Interval = new TimeSpan(0, 0, 2);
        }

        /// <summary>
        /// 上滑暂停
        /// </summary>
        public void WheeluptimeStop()
        {
            wheeluptime.Stop();
        }
        /// <summary>
        /// 下滑暂停
        /// </summary>
        public void WheeldowntimeStop()
        {
            wheeldowntime.Stop();
        }
        /// <summary>
        /// 上滑开始
        /// </summary>
        public void WheeluptimeStarrt()
        {
            wheeluptime.Start();
        }

         /// <summary>
         /// 下滑开始
         /// </summary>
        public void WheeldowntimeStart()
        {
            wheeldowntime.Start();
        }
       
        /// <summary>
        /// 上下滑计时器回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wheelupdowntime_timer(object sender, EventArgs e)
        {
            //MessageBox.Show("1234");
            BLL.ScreenPoint.POINT p = BLL.ScreenPoint.Getcursorpos();
            BLL.MouseEvent.mouse_event(BLL.MouseEvent.MouseEventFlag.LeftDown, p.X, p.Y, 0, UIntPtr.Zero);
            BLL.MouseEvent.mouse_event(BLL.MouseEvent.MouseEventFlag.LeftUp, p.X, p.Y, 0, UIntPtr.Zero);
        }
        
        /// <summary>
        /// 计时器的开始和暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upbuttom_MouseEnter(object sender, MouseEventArgs e)
        {
 
            WheeluptimeStarrt();
        }

        private void upbuttom_MouseLeave(object sender, MouseEventArgs e)
        {

            WheeluptimeStop();
        }
        private void downbuttom_MouseEnter(object sender, MouseEventArgs e) 
        {
 
            WheeldowntimeStart();
        }

        private void downbuttom_MouseLeave(object sender, MouseEventArgs e)
        {

            WheeldowntimeStop();
        }

        private void upbuttom_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SendKeys.SendWait("{PGUP}");
        }

        private void downbuttom_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.SendKeys.SendWait("{PGDN}");
        }
#endregion

        private void KinectTileButton_Click(object sender, RoutedEventArgs e)
        {
            timerstate = (timerstate == 0) ? 1 : 0;
            if (timerstate == 1)
            {
                btevent.ButteneventStart();
            }
            if (timerstate==0)
            {
                btevent.ButteneventStop();
            }
        }

        private void kinectRegion1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (timerstate==1)
            this.btevent.ButteneventStop();
        }

        private void kinectRegion1_MouseLeave(object sender, MouseEventArgs e)
        {
            if (timerstate == 1)
                this.btevent.ButteneventStart();
        }

    }
}


