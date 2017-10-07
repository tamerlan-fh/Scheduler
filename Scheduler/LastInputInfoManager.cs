using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    class LastInputInfoManager
    {
        public static LastInputInfoManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LastInputInfoManager();
                return instance;
            }
        }
        private static LastInputInfoManager instance;



        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// Функция вычисления бездействия пользователя
        /// </summary>
        /// <returns>количество секунд</returns>
        public TimeSpan GetTimeAfterLastInput()
        {
            int t = 0;
            LASTINPUTINFO l = new LASTINPUTINFO();
            l.cbSize = (UInt32)Marshal.SizeOf(l);
            l.dwTime = 0;
            int e = Environment.TickCount;

            if (GetLastInputInfo(ref l))
            {
                int inp = (Int32)l.dwTime;
                t = e - inp;
            }
            var seconds = (t > 0) ? (t / 1000) : 0;
            return TimeSpan.FromSeconds(seconds);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }
    }
}
