using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Designer
{
    class ScreenCapture
    {
        public ScreenCapture()
        {
        }
        public static Image captureDesktopRegion(Point topleft, Point bottomright)
        {
            int width = bottomright.X - topleft.X;
            int height = bottomright.Y - topleft.Y;
            IntPtr desktopWnd = Win32API.GetDesktopWindow();
            IntPtr deskDC = Win32API.GetDC(IntPtr.Zero);
            IntPtr regionDC = Win32API.CreateCompatibleDC(deskDC);
            IntPtr regionBitmap = Win32API.CreateCompatibleBitmap(deskDC, width, height);
            IntPtr bold = Win32API.SelectObject(regionDC, regionBitmap);
            Win32API.BitBlt(regionDC, 0, 0, width, height, deskDC, topleft.X, topleft.Y, Win32API.SRCCOPY);
            Bitmap img = Image.FromHbitmap(regionBitmap);
            Win32API.SelectObject(regionDC, bold);
            Win32API.DeleteObject(regionBitmap);
            Win32API.DeleteDC(regionDC);
            Win32API.ReleaseDC(desktopWnd, deskDC);
            return img;
        }
    }
}
