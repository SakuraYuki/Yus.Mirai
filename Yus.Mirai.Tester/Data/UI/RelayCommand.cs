using System;
using System.Windows.Input;

namespace Yus.Mirai.Tester.Data.UI
{
    /// <summary>
    /// 条件命令
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> canExecute;

        private readonly Action<object> execute;

        /// <summary>
        /// 使用执行条件和执行动作进行初始化
        /// </summary>
        /// <param name="canExecute">是否能执行命令的条件</param>
        /// <param name="execute">执行命令动作</param>
        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        /// <summary>
        /// 命令是否能执行发生变化时执行
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// 命令是否能执行
        /// </summary>
        /// <param name="parameter">命令参数</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter">命令参数</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }

    /// <summary>
    /// 条件命令
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> canExecute;

        private readonly Action<T> execute;

        /// <summary>
        /// 使用执行条件和执行动作进行初始化
        /// </summary>
        /// <param name="canExecute">是否能执行命令的条件</param>
        /// <param name="execute">执行命令动作</param>
        public RelayCommand(Predicate<T> canExecute, Action<T> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        /// <summary>
        /// 命令是否能执行发生变化时执行
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// 命令是否能执行
        /// </summary>
        /// <param name="parameter">命令参数</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return canExecute((T)parameter);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter">命令参数</param>
        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}
