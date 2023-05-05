using HandyControl.Controls;
using System;
using Yus.Mirai.Data;

namespace Yus.Mirai.Tester.Data.UI
{
    /// <summary>
    /// 基础VM
    /// </summary>
    public class BaseVM : BaseObservable
    {
        /// <summary>
        /// 是否在主线程（UI线程）
        /// </summary>
        public bool IsInMainThread => System.Threading.Thread.CurrentThread.ManagedThreadId == System.Windows.Application.Current.Dispatcher.Thread.ManagedThreadId;

        /// <summary>
        /// 在UI线程运行动作
        /// </summary>
        /// <param name="action">要执行的动作</param>
        public void RunUi(Action action)
        {
            if (action == null)
            {
                return;
            }

            if (IsInMainThread)
            {
                action.Invoke();
            }
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(action);
            }
        }

        /// <summary>
        /// 在UI线程运行方法
        /// </summary>
        /// <typeparam name="T">返回参数类型</typeparam>
        /// <param name="func">要执行的方法</param>
        /// <returns></returns>
        public T RunUi<T>(Func<T> func)
        {
            return func == null ? default : IsInMainThread ? func.Invoke() : System.Windows.Application.Current.Dispatcher.Invoke(func);
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标</param>
        /// <param name="owner">所属窗体</param>
        /// <returns></returns>
        public System.Windows.MessageBoxResult ShowMessageBox(string message, string title = "提示", System.Windows.MessageBoxButton button = System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage icon = System.Windows.MessageBoxImage.Information, Window owner = null)
        {
            return MessageBox.Show(
                owner: owner,
                messageBoxText: message,
                caption: title,
                button: button,
                defaultResult: System.Windows.MessageBoxResult.OK,
                icon: icon);
        }
    }
}
