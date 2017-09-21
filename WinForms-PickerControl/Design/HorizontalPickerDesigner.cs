using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PickerControl.Design
{
    [ToolboxItemFilter("HorizontalPicker.MarqueeBorder", ToolboxItemFilterType.Require)]
    [ToolboxItemFilter("HorizontalPicker.MarqueeText", ToolboxItemFilterType.Require)]
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    class HorizontalPickerDesigner : DocumentDesigner
    {

    }
}