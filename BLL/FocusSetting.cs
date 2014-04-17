using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BLL
{
	public class FocusSetting
	{
		private const int SW_HIDE = 0; 
        private const int SW_SHOWNORMAL = 1; 
        private const int SW_SHOWMINIMIZED = 2; 
        private const int SW_SHOWMAXIMIZED = 3;
		private const int SW_RESTORE = 9;          //用原来的大小和位置显示一个窗口，同时令其进入活动状态         
		private const int SW_SHOWDEFAULT = 10;     //根据默认 创建窗口时的样式 来显示  
		private const int GWL_EXSTYLE = (-20);
		public const int WS_DISABLED = 0x8000000;
		public const int WS_EX_LAYERED = 0x80000;
		private const int WS_EX_TRANSPARENT = 0x20;
		private const int LWA_ALPHA = 0x2;


        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll", EntryPoint = "FindWindow")]
		private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		[DllImport("user32.dll")]
		private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
		[DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
		private static extern int SetLayeredWindowAttributes(
		IntPtr hwnd,
		int crKey,
		int bAlpha,
		int dwFlags
		);
		
		/// <summary>
		/// 设置无焦点窗体
		/// </summary>
		/// <param name="windowName">窗体名</param>
		/// <returns>窗体句柄</returns>
		public static IntPtr SetNoFocus(string windowName)
		{
			IntPtr hwnd = FindWindow(null, windowName);
			//GetWindowLong(hwnd, GWL_EXSTYLE);
			SetWindowLong(hwnd, GWL_EXSTYLE,WS_DISABLED);
			return hwnd;
		}

		/// <summary>
		/// 获取窗口句柄
		/// </summary>
		/// <param name="windowName">窗口名字</param>
		/// <returns></returns>
		public static IntPtr GetIntPtr(string windowName)
		{
			return FindWindow(null, windowName);
		}

		/// <summary>
		/// 设置窗口是否可见
		/// </summary>
		/// <param name="windowIntPtr">窗口句柄</param>
		/// <param name="isVisible">是否可见</param>
		/// <returns></returns>
		public static bool SetVisible(IntPtr windowIntPtr, bool isVisible)
		{
			if (isVisible)
			{
				return ShowWindowAsync(windowIntPtr, SW_RESTORE);
			}
			else
			{
				return ShowWindowAsync(windowIntPtr,SW_HIDE);
			}
		}

		/// <summary>
		/// 使窗体具有鼠标穿透功能
		/// </summary>
		/// <param name="windowIntPtr">穿透窗体句柄</param>
		public void CanPenetrate(IntPtr windowIntPtr)
		{
			GetWindowLong(windowIntPtr, GWL_EXSTYLE);
			SetWindowLong(windowIntPtr, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_LAYERED);
			//SetLayeredWindowAttributes(windowIntPtr, 0, 100, LWA_ALPHA);
		}

        public static int GetWindowLongs(IntPtr windowIntPtr)
        {
            int windowlongs=GetWindowLong(windowIntPtr, GWL_EXSTYLE);
            return windowlongs;
        }

        public static void SetWindowLongs(IntPtr windowIntPtr,int style)
        {
            SetWindowLong(windowIntPtr, GWL_EXSTYLE, style);
        }
	}
}
