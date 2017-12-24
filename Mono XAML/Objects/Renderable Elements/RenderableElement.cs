using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MonoXAML.Objects
{
    public abstract class RenderableElement
    {
        private RenderableElement() { }
        public RenderableElement(FrameworkElement element)
        {
            _element = element;
        }

        private readonly FrameworkElement _element;

        public FrameworkElement Element { get { return _element; } }
        public Microsoft.Xna.Framework.Rectangle Rect { get { return Utility.GetRectangle(_element); } }
        public Microsoft.Xna.Framework.Vector2 Position { get { return Rect.Location.ToVector(); } }
        public Microsoft.Xna.Framework.Vector2 Size { get { return Rect.Size.ToVector(); } }

        public abstract void Render();
    }
}
