using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Runtime.InteropServices;
namespace BLL
{
    public class ScreenPoint
    {
        [DllImport("user32.dll")]
        private  static extern bool GetCursorPos(out POINT pt);
        private static POINT p;


        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        /// <summary>
        /// 返回鼠标当前的位置
        /// </summary>
        /// <returns>POINT类型</returns>
        public static POINT Getcursorpos()
        {
            GetCursorPos(out p);
            return p;
        }
        

    }
}
