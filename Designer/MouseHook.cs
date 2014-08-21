using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Designer
{
    class MouseHook
    {
        private List<MouseEventHandler> _procs;
        private IntPtr _hookID = IntPtr.Zero;
        // to prevent the delegate from being GC'ed
        private Win32API.LowLevelMouseProc _hookproc;

        public delegate void MouseEventHandler(int nCode, IntPtr wParam, IntPtr lParam);

        public MouseHook(MouseEventHandler proc)
        {
            _procs = new List<MouseEventHandler>();
            _procs.Add(proc);
            _hookID = SetHook();
        }
        ~MouseHook()
        {
            Win32API.UnhookWindowsHookEx(_hookID);
        }
        private IntPtr SetHook()
        {
            _hookproc = HookCallback;
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return Win32API.SetWindowsHookEx(Win32API.WH_MOUSE_LL, _hookproc,
                    Win32API.GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            foreach (var proc in _procs)
            {
                proc(nCode, wParam, lParam);
            }
            return Win32API.CallNextHookEx(_hookID, nCode, wParam, lParam); 
        }
    }
}
