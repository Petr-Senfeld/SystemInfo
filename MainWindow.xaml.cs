using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

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
            ObjectSearcher os = new ObjectSearcher();
            SetValuesToTextboxes(os);
        }
        
        private void SetValuesToTextboxes(ObjectSearcher os)
        {
            // System info Tab
            txtShortDeviceName.Text = os.ShortDeviceName;
            txtDeviceComment.Text = os.DeviceComment;
            txtOSversion.Text = os.OSversion;
            txtOSplatform.Text = os.OSplatform;
            txtOSlanguage.Text = os.OSlanguage;
            txtOSlanguageUI.Text = os.OSlanguageUI;
            txtSecureBoot.Text = os.SecureBoot;
            txtBitlocker.Text = os.Bitlocker;
            txtInstallationDate.Text = os.InstallationDate;
            txtUptime.Text = os.Uptime;

            // HW info Tab
            txtDeviceVendor.Text = os.DeviceVendor;
            txtDeviceModel.Text = os.DeviceModel;
            txtSerialNumber.Text = os.SerialNumber;
            txtUUID.Text = os.UUID;
            txtOperatingMemory.Text = os.OperatingMemory;
            txtDiskSpace.Text = os.DiskSpace;

            // Network info Tab
            txtNetwork.Text = os.NetworkInfo;

            // User info Tab
            txtFullName.Text = os.FullName;
            txtLogonName.Text = os.LogonName;
            txtCorpId.Text = os.CorpId;
            txtLogonServer.Text = os.LogonServer;
            txtHomeDirectory.Text = os.HomeDir;
            txtProfile.Text = os.Profile;
            txtLocalRights.Text = os.LocalRights;
        }

        private string CopyToClip()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("System info:\n");
            sb.AppendFormat("====================\n");
            sb.AppendFormat("Device short name: {0}\n", txtShortDeviceName.Text);
            sb.AppendFormat("Device comment: {0}\n", txtDeviceComment.Text);
            sb.AppendFormat("OS version: {0}\n", txtOSversion.Text);
            sb.AppendFormat("OS platform: {0}\n", txtOSplatform.Text);
            sb.AppendFormat("OS language: {0}\n", txtOSlanguage.Text);
            sb.AppendFormat("OS language UI: {0}\n", txtOSlanguageUI.Text);
            sb.AppendFormat("Secure boot: {0}\n", txtSecureBoot.Text);
            sb.AppendFormat("Bitlocker: {0}\n", txtBitlocker.Text);
            sb.AppendFormat("Installation date: {0}\n", txtInstallationDate.Text);
            sb.AppendFormat("Up time: {0}\n", txtUptime.Text);
            sb.AppendLine();
            sb.AppendFormat("HW info:\n");
            sb.AppendFormat("====================\n");
            sb.AppendFormat("Device vendor: {0}\n", txtDeviceVendor.Text);
            sb.AppendFormat("Device model: {0}\n", txtDeviceModel.Text);
            sb.AppendFormat("Serial number: {0}\n", txtSerialNumber.Text);
            sb.AppendFormat("UUID: {0}\n", txtUUID.Text);
            sb.AppendFormat("RAM: {0}\n", txtOperatingMemory.Text);
            sb.AppendFormat("Disk space: {0}\n", txtDiskSpace.Text);
            sb.AppendLine();
            sb.AppendFormat("Network info:\n");
            sb.AppendFormat("====================\n");
            sb.AppendFormat("{0}\n", txtNetwork.Text);
            sb.AppendLine();
            sb.AppendFormat("User info:\n");
            sb.AppendFormat("====================\n");
            sb.AppendFormat("Full name: {0}\n", txtFullName.Text);
            sb.AppendFormat("Full logon name: {0}\n", txtLogonName.Text);
            sb.AppendFormat("CorpId: {0}\n", txtCorpId.Text);
            sb.AppendFormat("Logon server: {0}\n", txtLogonServer.Text);
            sb.AppendFormat("Home dir: {0}\n", txtHomeDirectory.Text);
            sb.AppendFormat("Profile: {0}\n", txtProfile.Text);
            sb.AppendFormat("Local rights: {0}\n", txtLocalRights.Text);
            return sb.ToString();
        }
        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(CopyToClip(), TextDataFormat.UnicodeText);
        }
    }
}
