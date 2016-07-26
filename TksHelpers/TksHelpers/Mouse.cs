using System.Drawing;
using System.Runtime.InteropServices;

namespace TksHelpers
{
    public class Mouse
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// Get the position of the mouse
        /// </summary>
        /// <returns>A point at mouse coordonates</returns>
        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        private const int MouseeventfLeftdown = 0x02;
        private const int MouseeventfLeftup = 0x04;

        /// <summary>
        /// Make a Click at the position of the cursor
        /// </summary>
        public static void Click()
        {
            var pt = GetMousePosition();
            Click((uint)pt.X, (uint)pt.Y);
        }

        /// <summary>
        /// Make a Click at the choosen coordonates
        /// </summary>
        /// <param name="x">Easting</param>
        /// <param name="y">Northing</param>
        public static void Click(uint x, uint y)
        {
            mouse_event(MouseeventfLeftdown | MouseeventfLeftup, x,y, 0, 0);
        }
    }
}
