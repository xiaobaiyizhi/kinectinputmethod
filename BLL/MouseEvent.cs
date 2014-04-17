using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BLL
{
    public class MouseEvent
    {
        [DllImport("user32.dll")]
          public static extern void mouse_event(MouseEventFlag flags, int dx, int dy, int data, UIntPtr extraInfo);
        [Flags]
        public enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,//滚轮移动 data为移动距离 正为向上移动 负为向下移动
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        } 

    }
}
