using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FShot
{
    class Screen
    {

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]

        private static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]

        private static extern IntPtr GetShellWindow();
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowRect(IntPtr hwnd, out RECT rc);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        public static bool isActiveWindowFullscreen() {
            RECT appBounds;
            Rectangle screenBounds;
            IntPtr hWnd;

            //get the dimensions of the active window
            hWnd = GetForegroundWindow();
            if (hWnd != null && !hWnd.Equals(IntPtr.Zero))
            {
                //Check we haven't picked up the desktop or the shell
                if (!(hWnd.Equals(GetDesktopWindow()) || hWnd.Equals(GetShellWindow())))
                {
                    GetWindowRect(hWnd, out appBounds);
                    //determine if window is fullscreen
                    screenBounds = System.Windows.Forms.Screen.FromHandle(hWnd).Bounds;
                    if ((appBounds.Bottom - appBounds.Top) == screenBounds.Height && (appBounds.Right - appBounds.Left) == screenBounds.Width)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string getActiveWindowTitle() {
            int textLength = 256;
            StringBuilder outText = new StringBuilder(textLength + 1);
            int a = GetWindowText(GetForegroundWindow(), outText, outText.Capacity);
            return outText.ToString();
        }





        public static Bitmap caputreScreen()
        {
            Bitmap screenShot = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);

            using (Graphics graphics = Graphics.FromImage(screenShot))
            {
                graphics.CopyFromScreen(new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Left, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Top), Point.Empty, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size);
            }

            Bitmap result = new Bitmap(1920, 1080);
            using (Graphics graphics = Graphics.FromImage(result)) {
                graphics.DrawImage(screenShot, 0, 0, 1920, 1080);
            }
            return result;
        }


    }
}
