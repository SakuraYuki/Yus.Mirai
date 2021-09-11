using System;
using System.ComponentModel;

namespace Yus.Mirai.Tester.Views
{
    public class BaseWindow : HandyControl.Controls.Window
    {
        /// <summary>
        /// 是否正在设计模式中
        /// </summary>
        public bool IsDesignMode => DesignerProperties.GetIsInDesignMode(this);

        /// <summary>
        /// 显示Growl通知的面板，不重写则不设置Growl通知
        /// </summary>
        public virtual System.Windows.Controls.Panel GrowlPanel { get; }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (GrowlPanel != null)
            {
                HandyControl.Controls.Growl.SetGrowlParent(GrowlPanel, true);
            }
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);

            if (GrowlPanel != null)
            {
                HandyControl.Controls.Growl.SetGrowlParent(GrowlPanel, false);
            }
        }
    }
}
