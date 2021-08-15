using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Management;
using System.Security.Principal;
using System.Text;

namespace System_Information
{
    class ObjectSearcher
    {
        // System info Tab
        public string ShortDeviceName { get; private set; }
        public string DeviceComment { get; private set; }
        public string OSversion { get; private set; }
        public string OSplatform { get; private set; }
        public string OSlanguage { get; private set; }
        public string OSlanguageUI { get; private set; }
        public string SecureBoot { get; private set; }
        public string Bitlocker { get; private set; }
        public string InstallationDate { get; private set; }
        public string Uptime { get; private set; }

        // HW info Tab
        public string DeviceVendor { get; private set; }
        public string DeviceModel { get; private set; }
        public string SerialNumber { get; private set; }
        public string UUID { get; private set; }
        public string DiskSpace{ get; private set; }
        public string OperatingMemory { get; private set; }

        // Network info Tab
        public string NetworkInfo { get; private set; }

        // User info Tab
        public string FullName { get; private set; }
        public string LogonName { get; private set; }
        public string CorpId { get; private set; }
        public string LogonServer { get; private set; }
        public string HomeDir { get; private set; }
        public string Profile { get; private set; }
        public string LocalRights { get; private set; }

        public ObjectSearcher()
        {
            this.GetSystemInfoTab();
            this.GetHWinfoTab();
            this.GetNetworkInfoTab();
            this.GetUserInfoTab();
        }

        private void GetSystemInfoTab()
        {
            ShortDeviceName = Environment.MachineName;
            DeviceComment = Environment.UserName;

            ManagementObjectSearcher osObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (var obj in osObject.Get())
            {
                try
                {
                    OSversion = obj["Caption"].ToString() + " / " + obj["BuildNumber"].ToString();
                    if (obj["OSLanguage"].ToString() == "1033")
                    {
                        OSlanguage = "English";
                    }
                    else if (obj["OSLanguage"].ToString() == "1029")
                    {
                        OSlanguage = "Czech";
                    }
                    else
                        OSlanguage = "Unknown";

                    CultureInfo ci = CultureInfo.InstalledUICulture;
                    OSlanguageUI = ci.Name;

                    // installation date
                    DateTime installDate = ManagementDateTimeConverter.ToDateTime(obj["InstallDate"].ToString());
                    InstallationDate = installDate.ToString();
                    DateTime upTimeDate = ManagementDateTimeConverter.ToDateTime(obj["LastBootUpTime"].ToString());
                    Uptime = upTimeDate.ToString();

                    // RAM
                    var totalMemory = Math.Round(double.Parse(obj["TotalVisibleMemorySize"].ToString()) / 1024 / 1024, 2);
                    var freeMemory = Math.Round(double.Parse(obj["FreePhysicalMemory"].ToString()) / 1024 / 1024, 2);
                    OperatingMemory = string.Format("{0} Gb Total ({1} Gb Free)", totalMemory, freeMemory);
                }
                catch (Exception e)
                {

                    LoggerTool.Logger(e.ToString());
                }
            }

            // processor platform x84 or x64
            ManagementObjectSearcher processorObject = new ManagementObjectSearcher("select * from Win32_processor");
            foreach (var obj in processorObject.Get())
            {
                try
                {
                    OSplatform = obj["Architecture"].ToString() == "9" ? "x64" : "x86";
                }
                catch (Exception e )
                {
                    LoggerTool.Logger(e.ToString());
                }
            }

            // bitlocker
            //    ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\CIMV2\\Security\\MicrosoftVolumeEncryption");
            //    ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_EncryptableVolume");
            //    ManagementObjectSearcher bitlockerObject = new ManagementObjectSearcher(scope, query);

            //    foreach (var obj in bitlockerObject.Get())
            //    {
            //        try
            //        {
            //            Bitlocker = obj["ProtectionStatus"].ToString() == "1" ? "Active" : "Deactivated";
            //        }
            //        catch (Exception)
            //        {
            //            // to be logged
            //        }

            //    }
            // secure boot
            try
            {
                int rc = 0;
                string key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State";
                string subkey = @"UEFISecureBootEnabled";
                object value = Registry.GetValue(key, subkey, rc);
                SecureBoot = value.ToString() == "1" ? "Active" : "Deactivated";
            }
            catch (Exception e)
            {
                LoggerTool.Logger(e.ToString());
            }

        }

        private void GetHWinfoTab()
        {
            // device Vendor&Model
            ManagementObjectSearcher systemObject = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            foreach (var obj in systemObject.Get())
            {
                try
                {
                    DeviceVendor = obj["Manufacturer"].ToString();
                    DeviceModel = obj["Model"].ToString();
                }
                catch (Exception e)
                {
                    LoggerTool.Logger(e.ToString());
                }
            }

            // Serial number
            ManagementObjectSearcher BIOSObject = new ManagementObjectSearcher("select * from Win32_BIOS");
            foreach (var obj in BIOSObject.Get())
            {
                try
                {
                    SerialNumber = obj["SerialNumber"].ToString();
                }
                catch (Exception e)
                {
                    LoggerTool.Logger(e.ToString());
                }

            }

            // UUID
            ManagementObjectSearcher UUIDObject = new ManagementObjectSearcher("select * from Win32_ComputerSystemProduct");
            foreach (var obj in UUIDObject.Get())
            {
                try
                {
                    UUID = obj["UUID"].ToString();
                }
                catch (Exception e)
                {
                    LoggerTool.Logger(e.ToString());
                }

            }

            // diskspace
            try
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady == true)
                        DiskSpace += d.Name + " " + string.Format("[{0}] Total {1} Gb", d.VolumeLabel, (d.TotalSize / 1024 / 1024 / 1024)) + " " + string.Format("({0} Gb Free )\n", (d.TotalFreeSpace / 1024 / 1024 / 1024).ToString());
                }
            }
            catch (Exception e)
            {
                LoggerTool.Logger(e.ToString());
            }
            
        }

        private void GetNetworkInfoTab()
        {
            // network info
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = "ipconfig.exe";
                p.StartInfo.Arguments = "/all";
                p.Start();
                p.StartInfo.Arguments = "exit";
                NetworkInfo = p.StandardOutput.ReadToEnd();
            }
            catch (Exception e)
            {
                LoggerTool.Logger(e.ToString());
            }
            
        }

        private void GetUserInfoTab()
        {
            try
            {
                FullName = WindowsIdentity.GetCurrent().Name;
                Profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                HomeDir = Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH");
                CorpId = Environment.UserName;
                LogonName = Environment.UserDomainName + "/" + CorpId;
                LogonServer = Environment.GetEnvironmentVariable("LOGONSERVER");

                // getting local user rights
                //var userRights = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                string sPath = "WinNT://" + Environment.MachineName + ",computer";
                using (var computerEntry = new DirectoryEntry(sPath))
                {
                    foreach (DirectoryEntry childEntry in computerEntry.Children)
                    {
                        if (childEntry.SchemaClassName == "User")
                        {
                            object obGroups = childEntry.Invoke("Groups");
                            foreach (object ob in (System.Collections.IEnumerable)obGroups)
                            {
                                DirectoryEntry obGpEntry = new DirectoryEntry(ob);
                                LocalRights += obGpEntry.Name + ", ";
                                obGpEntry.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LoggerTool.Logger(e.ToString());
            }
        }
    }
}
