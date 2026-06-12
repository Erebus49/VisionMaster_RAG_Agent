using System;
using System.Windows.Forms;

namespace ChatDemoCs
{
    /// <summary>
    /// Hosts the VisionMaster render canvas (<see cref="VMControls.Winform.Release.VmRenderControl"/>).
    /// Mirrors the structure used by the sibling demos so the Solution -&gt; ModuleSource binding
    /// in <see cref="MainForm"/> remains compatible.
    /// </summary>
    public partial class RenderControl : UserControl
    {
        public RenderControl()
        {
            InitializeComponent();
        }
    }
}
