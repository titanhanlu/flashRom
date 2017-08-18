using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace flashRom
{
    class ConfigNetwork
    {
        public static void SetIP(int num)
        {
            List<string> ips = genIPs(num);
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            int index = 0;
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;
                if (!mo["Caption"].ToString().Contains("USB"))
                    continue;

                //设置ip地址和子网掩码 
                inPar = mo.GetMethodParameters("EnableStatic");
                string ip = ips[index++];
                inPar["IPAddress"] = new string[] { ip };// 1.备用 2.IP
                inPar["SubnetMask"] = new string[] { "255.255.255.0"};
                outPar = mo.InvokeMethod("EnableStatic", inPar, null);

                //设置网关地址 
                inPar = mo.GetMethodParameters("SetGateways");
                inPar["DefaultIPGateway"] = new string[] { "192.168.31.1" }; // 1.网关;2.备用网关
                outPar = mo.InvokeMethod("SetGateways", inPar, null);

                //设置DNS 
                inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                inPar["DNSServerSearchOrder"] = new string[] { "192.168.31.1"}; // 1.DNS 2.备用DNS
                outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
            }
        }

        private static List<string> genIPs(int num)
        {
            string baseStr = "192.168.31.";
            List<string> ips = new List<string>();
            for (int i = 5;i < 5 + num; i++)
            {
                ips.Add(baseStr + i.ToString());
            }
            return ips;
        }
    }
}
