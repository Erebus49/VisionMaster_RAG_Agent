using System;
using System.Windows.Forms;

namespace ChatDemoCs
{
    /// <summary>
    /// Hosts the VisionMaster main-view configuration controls
    /// (<see cref="VMControls.Winform.Release.VmGlobalToolControl"/> +
    /// <see cref="VMControls.Winform.Release.VmMainViewConfigControl"/>),
    /// shown when the user picks the "参数配置 / Config" tab.
    /// </summary>
    public partial class MainViewControl : UserControl
    {
        public MainViewControl()
        {
            InitializeComponent();
        }
    }
}
