using System;
using System.Windows.Forms;

namespace ChatDemoCs
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using (AuthStartForm auth = new AuthStartForm())
                {
                    Application.Run(auth);
                    if (!auth.LoginSucceeded)
                    {
                        return;
                    }
                }
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);
                if (null != vmEx)
                {
                    string strMsg = "InitControl failed. Error Code: 0x" + Convert.ToString(vmEx.errorCode, 16);
                    MessageBox.Show(strMsg);
                }
                else
                {
                    MessageBox.Show("Startup failed: " + ex.Message, "ChatDemoCs",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
