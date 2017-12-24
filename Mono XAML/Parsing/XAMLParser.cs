using System;
using System.Collections.Generic;
using System.Text;
using MonoGame;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using MonoXAML.Objects;

namespace MonoXAML.Parsing
{
    public static class XAMLParser
    {
        private static UIObject _currentObject;

        private static readonly Dictionary<Type, Action<FrameworkElement>> contentParsers = new Dictionary<Type, Action<FrameworkElement>>()
        {
            { typeof(Grid), ParseGrid },
            { typeof(Rectangle), ParseRectangle },
        };

        public static void Parse(UserControl content, UIObject owner)
        {
            _currentObject = owner;

            ParseContent(content.Content);
        }
        private static void ParseRectangle(FrameworkElement element)
        {
            _currentObject.AddRenderableElement(new RenderableRectangle(element));
        }
        private static void ParseGrid(UIElement element)
        {
            Grid grid = (Grid)element;

            foreach (FrameworkElement uiElement in grid.Children)
            {
                ParseContent(uiElement);
            }
        }
        private static void ParseContent(object content)
        {
            Type type = content.GetType();

            if (contentParsers.ContainsKey(type))
            {
                contentParsers[type].Invoke((FrameworkElement)content);
            }
            else
            {
#if DEBUG
                throw new NullReferenceException("Cannot parse " + type);
#else
                Console.WriteLine("WARNING: Cannot parse " + type);
#endif
            }
        }
    }
}
