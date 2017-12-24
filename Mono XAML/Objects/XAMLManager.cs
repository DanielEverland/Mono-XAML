using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoXAML
{
    /// <summary>
    /// Base Mono XAML manager that keeps references to all MonoUI Objects
    /// </summary>
    public class XAMLManager
    {
        private XAMLManager() { }

        /// <summary>
        /// Reference to manager instance. Call initialize before accessing this
        /// </summary>
        public static XAMLManager Instance { get { return _instance; } }
        private static XAMLManager _instance;
        
        public static GraphicsDeviceManager GraphicsDeviceManager { get { return _graphicsDeviceManager; } }
        private static GraphicsDeviceManager _graphicsDeviceManager;
        
        public static SpriteBatch SpriteBatch { get { return _spriteBatch; } }
        private static SpriteBatch _spriteBatch;

        private List<UIObject> _uiObjects = new List<UIObject>();

        public static void Initialize(GraphicsDeviceManager graphicsDevice, SpriteBatch spriteBatch)
        {
            _instance = new XAMLManager();
            _spriteBatch = spriteBatch;
            _graphicsDeviceManager = graphicsDevice;
        }

        /// <summary>
        /// Creates a new UI object using the XAML designer data
        /// </summary>
        /// <typeparam name="T">Mono UI Object</typeparam>
        /// <param name="content">XAML Data</param>
        /// <returns></returns>
        public static UIObject CreateEntity<T>() where T : UserControl
        {
            return new UIObject(Activator.CreateInstance<T>());
        }

        /// <summary>
        /// Will draw UI
        /// </summary>
        public static void Draw()
        {
            foreach (UIObject obj in _instance._uiObjects)
            {
                obj.Draw();
            }
        }

        internal void AddObject(UIObject obj)
        {
            _uiObjects.Add(obj);
        }
        
    }
}
