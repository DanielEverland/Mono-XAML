using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MonoXAML.Objects
{
    public abstract class RenderableElement : RenderableObjectBase
    {
        private RenderableElement() { }
        public RenderableElement(FrameworkElement element)
        {
            _element = element;
        }
        
        public FrameworkElement Element { get { return _element; } }

        private readonly FrameworkElement _element;
    }
}
