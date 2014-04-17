using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BLL;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;

namespace newproject
{
    public class Buttonevent//活动屏幕中取均值的点
    {
        private Timer t;
        private static int i = 0;
        private  static ScreenPoint.POINT temp_p;
        private  static ScreenPoint.POINT p;
        public static int Isclosed=0;
        public int Timerstate = 0;
        private DispatcherTimer dispatchertimer = new System.Windows.Threading.DispatcherTimer();
        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="duetime"></param>
        public  void Time( )
        {

            dispatchertimer.Tick += new EventHandler(dispatchertimer_timer);
            dispatchertimer.Interval = new TimeSpan(0, 0,1);

        }

        public void ButteneventDispose()
        {
            dispatchertimer.Stop();
        }

        public void ButteneventStop()
        {
            dispatchertimer.Stop();
        }
        public void ButteneventStart()
        {
            dispatchertimer.Start();
        }
        private void dispatchertimer_timer(object sender, EventArgs e)
        {
            p = BLL.ScreenPoint.Getcursorpos();
            if (p.X < (temp_p.X + 20) && p.X > (temp_p.X - 20) && p.Y < (temp_p.Y + 20) && p.Y > (temp_p.Y - 20))
            {
                i++;
            }
            else
            {
                i = 0;
            }
            if (i >= 3)
            {
                i = 0;
            }
            if (i == 2)
            {
                //this.ButteneventStop();
                i = 0;
              
                try
                {
                   /* ButtonWindow.px = p.X - 50;
                    ButtonWindow.py = p.Y - 50;
                    ButtonWindow bt = new ButtonWindow();

                    bt.Left = 0;
                    bt.Top = 0;
                    bt.Width = SystemParameters.WorkArea.Width;
                    bt.Height = SystemParameters.WorkArea.Height;
                    bt.btkinectregin.Width = SystemParameters.WorkArea.Width;
                    bt.btkinectregin.Height = SystemParameters.WorkArea.Height;
                    bt.btkinectregin.KinectSensor = MiniWindow.miniwindow.kinectRegion1.KinectSensor;
                    */
                    //if(bt.btkinectregin.KinectSensor!=null)
                   // MessageBox.Show("weqwe");
                    Microsoft.Kinect.KinectSensor temp = MiniWindow.miniwindow.kinectRegion1.KinectSensor; 
                    MiniWindow.miniwindow.kinectRegion1.KinectSensor=null;
                    
                  /* bt.Dispatcher.Invoke(new Action(delegate
                    {

                      //  bt.canvas.SetValue(Canvas.LeftProperty, p.Y - 150);
                    }));
                    bt.Dispatcher.Invoke(new Action(delegate
                    {
                       // bt.canvas.SetValue(Canvas.TopProperty, p.X - 150);
                    }));
                    bt.Dispatcher.Invoke(new Action(delegate
                    {
                       bt.Visibility = Visibility.Visible;
                    }));
                    
                   bt.circlebt.ClickMode
                */
                    IntPtr intptr = BLL.FocusSetting.GetIntPtr("MiniWindow");
                    BLL.FocusSetting fs = new BLL.FocusSetting();
                    fs.CanPenetrate(intptr);
                    //IntPtr buttomintptr = FindWindowEx((IntPtr)intptr, (IntPtr)0, "0", "button1");
                    BLL.FocusSetting.SetWindowLong(intptr, (-20), 0x8000000);
                    //iscanpenetrate = 1;
                    BLL.MouseEvent.mouse_event(BLL.MouseEvent.MouseEventFlag.LeftDown, p.X, p.Y, 0, UIntPtr.Zero);
                    BLL.MouseEvent.mouse_event(BLL.MouseEvent.MouseEventFlag.LeftUp, p.X, p.Y, 0, UIntPtr.Zero);
                    MiniWindow.miniwindow.kinectRegion1.KinectSensor = temp;
                }
                catch (System.Exception ex)
                {
                   // MessageBox.Show(ex.ToString());
                }
            }
            temp_p = p;
        }
        /// <summary>
        /// 计时器的回调函数
        /// </summary>
        /// <param name="state"></param>
    
    }
}
