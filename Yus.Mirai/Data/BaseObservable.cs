using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Yus.Mirai.Data
{
    /// <summary>
    /// 基础可观察对象
    /// </summary>
    public class BaseObservable : INotifyPropertyChanged
    {
        /// <summary>
        /// 属性改变时发生
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 通知属性变化
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 设置属性变化
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="target">目标值</param>
        /// <param name="source">源值</param>
        /// <param name="propertyName">属性名称</param>
        public void Set<T>(ref T target, T source, [CallerMemberName] string propertyName = null)
        {
            target = source;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
