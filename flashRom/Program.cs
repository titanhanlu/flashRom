using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace flashRom
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new mainForm());
            RouterApi router = new RouterApi("admin", "12345678", "192.168.31.1", "192.168.31.10");
            //router.UploadRom(@"c:/miwifi.bin");
            //router.getToken();
            router.getInitInfo();
            //ConfigNetwork.SetIP(1);

        }
    }
}
