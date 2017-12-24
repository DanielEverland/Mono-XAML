using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MonoXAML
{
    /// <summary>
    /// Base Mono XAML manager that keeps references to all MonoUI Objects
    /// </summary>
    public class XAMLManager
    {
        private XAMLManager() { }

        /// <summary>
        /// Refernce to manager instance. Call initialize before accessing this
        /// </summary>
        public static XAMLManager Instance { get { return _instance; } }
        private static XAMLManager _instance;

        public static void Initialize()
        {
            _instance = new XAMLManager();
        }
        /// <summary>
        /// Creates a new UI object using the XAML designer data
        /// </summary>
        /// <typeparam name="T">Mono UI Object</typeparam>
        /// <param name="content">XAML Data</param>
        /// <returns></returns>
        public static UIObject CreateEntity(UserControl content)
        {
            return new UIObject(content);
        }
    }
}
