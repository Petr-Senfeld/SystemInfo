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
    }
}
