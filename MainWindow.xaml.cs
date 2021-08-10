using System;
using System.Management;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;

namespace System_Information
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetSystemInfo();
        }

        private void GetSystemInfo()
        {
            ManagementObjectSearcher processorObject = new ManagementObjectSearcher("select * from Win32_processor");
            foreach (var obj in processorObject.Get())
            {
                try
                {
                    txtOSplatform.Text = obj["Architecture"].ToString() == "9" ? "x64" : "x86";
                }
                catch (Exception)
                {
                    // to be logged
                }

            }

            ManagementObjectSearcher BIOSObject = new ManagementObjectSearcher("select * from Win32_BIOS");
            foreach (var obj in BIOSObject.Get())
            {
                try
                {
                    txtSerialNumber.Text = obj["SerialNumber"].ToString();
                }
                catch (Exception)
                {
                    // to be logged
                }

            }

            ManagementObjectSearcher systemObject = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            foreach (var obj in systemObject.Get())
            {
                try
                {
                    txtDeviceVendor.Text = obj["Manufacturer"].ToString();
                    txtDeviceModel.Text = obj["Model"].ToString();
                }
                catch (Exception)
                {
                    // to be logged
                }

            }

            ManagementObjectSearcher osObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (var obj in osObject.Get())
            {
                try
                {
                    txtOSversion.Text = obj["Caption"].ToString() + " / " + obj["BuildNumber"].ToString();
                    if (obj["OSLanguage"].ToString() == "1033")
                    {
                        txtOSlanguage.Text = "English";
                    }
                    else if (obj["OSLanguage"].ToString() == "1029")
                    {
                        txtOSlanguage.Text = "Czech";
                    }
                    else
                        txtOSlanguage.Text = "Unknown";

                    DateTime installDate = ManagementDateTimeConverter.ToDateTime(obj["InstallDate"].ToString());
                    txtInstallationDate.Text = installDate.ToString();
                    DateTime upTimeDate = ManagementDateTimeConverter.ToDateTime(obj["LastBootUpTime"].ToString());
                    txtUptime.Text = upTimeDate.ToString();

                    var totalMemory = Math.Round(double.Parse(obj["TotalVisibleMemorySize"].ToString()) / 1024 / 1024, 2);
                    var freeMemory = Math.Round(double.Parse(obj["FreePhysicalMemory"].ToString()) / 1024 / 1024, 2);
                    txtOperatingMemory.Text = string.Format("{0} Gb Total ({1} Gb Free)", totalMemory, freeMemory);

                }
                catch (Exception)
                {

                    // to be logged
                }
            }
            // bitlocker
            ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\CIMV2\\Security\\MicrosoftVolumeEncryption");
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_EncryptableVolume");
            ManagementObjectSearcher bitlockerObject = new ManagementObjectSearcher(scope, query);

            foreach (var obj in bitlockerObject.Get())
            {
                try
                {
                    txtBitlocker.Text = obj["ProtectionStatus"].ToString() == "1" ? "Active" : "Deactivated";
                }
                catch (Exception)
                {
                    // to be logged
                }

            }
            // secure boot
            int rc = 0;
            string key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State";
            string subkey = @"UEFISecureBootEnabled";
            object value = Registry.GetValue(key, subkey, rc);
            txtSecureBoot.Text = value.ToString() == "1" ? "Active" : "Deactivated";

            txtShortDeviceName.Text = Environment.MachineName;
            txtDeviceComment.Text = Environment.UserName;

            // disk space
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                    txtDiskSpace.Text += d.Name + " " + string.Format("[{0}] Total {1} Gb", d.VolumeLabel, (d.TotalSize / 1024 / 1024 / 1024)) + " " + string.Format("({0} Gb Free )\n", (d.TotalFreeSpace / 1024 / 1024 / 1024).ToString());
            }

            // network info
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "ipconfig.exe";
            p.StartInfo.Arguments = "/all";
            p.Start();
            p.StartInfo.Arguments = "exit";
            txtNetwork.Text = p.StandardOutput.ReadToEnd();


            ManagementObjectSearcher userObject = new ManagementObjectSearcher("select * from Win32_Account ");
            txtProfile.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            txtHomeDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            txtCorpId.Text = Environment.UserName;
            foreach (var obj in userObject.Get())
            {
                try
                {
                    txtFullName.Text = obj["Name"].ToString();
                    txtLocalRights.Text = obj["AccountType"].ToString();
                    txtLogonName.Text = obj["Name"].ToString();
                }
                catch (Exception)
                {
                    // to be logged
                }

            }
        }   
    }
}
