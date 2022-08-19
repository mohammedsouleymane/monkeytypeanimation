using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MonkeyAnimation;

namespace MonkeyAnimation
{
    class KeyLogger
    {
        public static PictureBox _pictureBox = null;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;

        private static IntPtr _hookID = IntPtr.Zero;
        private static int index = 0;
        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        public static void Start()
        {
            _hookID = SetHook(_proc);
            Application.Run(new Form1());
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            
            if (nCode >= 0 && wParam == (IntPtr) WM_KEYDOWN)
            {
                index = index > 1 ? 0 : ++index;
              
                var properties = typeof(Properties.Resources).GetProperties(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                var images = properties.Where(i => i.Name.EndsWith("1") || i.Name.EndsWith("2") || i.Name.EndsWith("4")).ToArray();
                _pictureBox.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(images[index].Name)!;
                //_pictureBox.Enabled = true;
                //int vkCode = Marshal.ReadInt32(lParam);
                //var keyName = Enum.GetName(typeof(Keys), vkCode);
                //string currentLog = File.ReadAllText(notepadPath);
                //using (StreamWriter sw = new StreamWriter(notepadPath))
                //{
                //    if (keyName.Length == 1)
                //        sw.Write(currentLog + keyName);
                //    else
                //        sw.Write(currentLog);
                //}
            }
            else
            {                
                var properties = typeof(Properties.Resources).GetProperties(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                var images = properties.Where(i => i.Name.EndsWith("3")).ToArray();
                _pictureBox.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(images[0].Name)! ;
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        
    }
}
