using System;
using System.Windows.Input;
using Yus.Mirai.Tester.Data.UI;

namespace Yus.Mirai.Tester.Views
{
    /// <summary>
    /// 基础ViewModel
    /// </summary>
    public class BaseViewModel : BaseVM
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        public ICommand ExecuteCommand { get; protected set; }

        /// <summary>
        /// 默认构造
        /// </summary>
        public BaseViewModel()
        {
            ExecuteCommand = new RelayCommand<string>(f => !string.IsNullOrWhiteSpace(f), Execute);
        }

        /// <summary>
        /// 执行命令方法
        /// </summary>
        /// <param name="args">命令标记</param>
        public virtual void Execute(string args)
        {
            #region DEBUG

            Console.WriteLine($"Execute:{args}");

            #endregion DEBUG
        }
    }
}
