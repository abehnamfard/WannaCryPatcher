using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace WannaCryPatch
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnPatch_Click(object sender, EventArgs e)
        {
            bool isElevated;
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            if (isElevated)
            {
                System.Diagnostics.Process.Start("cmd", "/C netsh advfirewall firewall add rule dir=in action=block protocol=TCP localport=135 name=\"Block_TCP - 135\"");
                System.Diagnostics.Process.Start("cmd", "/C netsh advfirewall firewall add rule dir=in action=block protocol=TCP localport=445 name=\"Block_TCP - 445\"");
                System.Diagnostics.Process.Start("cmd", "/C dism /online /norestart /disable-feature /featurename:SMB1Protocol");
                MessageBox.Show(@"Patched");
            }
            else
            {
                MessageBox.Show(@"Run as administrator !!!");
            }
        }
    }
}
