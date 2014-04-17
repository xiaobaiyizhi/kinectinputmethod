using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Models
{
	public class Kinect
	{
		private static KinectSensor kinect;
		private static IntPtr mainWindow;
		private static IntPtr miniWindow;
		/// <summary>
		/// 赋值kinect
		/// </summary>
		/// <param name="_kinect">currentKinect</param>
		/// <param name="_mainWindow">MainWindow句柄</param>
		/// <param name="_miniWindow">MiniWindow句柄</param>
		public Kinect(KinectSensor _kinect, IntPtr _mainWindow, IntPtr _miniWindow)
		{
			kinect = _kinect;
			mainWindow = _mainWindow;
			miniWindow = _miniWindow;
		}
		public KinectSensor kinectSensor
		{
			get
			{
				return kinect;
			}
			set
			{
				kinect = value;
			}
		}
		public IntPtr mainIntPtr
		{
			set
			{
				mainWindow = value;
			}
			get
			{
				return mainWindow;
			}
		}
		public IntPtr miniIntPtr
		{
			set
			{
				miniWindow = value;
			}
			get
			{
				return miniWindow;
			}
		}
	}
}
