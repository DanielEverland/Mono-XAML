using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using MonoXAML.Parsing;
using MonoXAML.Objects;

namespace MonoXAML
{
    public class UIObject
    {
        private UIObject() { }
        internal UIObject(UserControl content)
        {
            _elements = new List<RenderableElement>();

            XAMLParser.Parse(content, this);
            XAMLManager.Instance.AddObject(this);
        }

        public IEnumerable<RenderableElement> Elements { get { return _elements; } }

        private List<RenderableElement> _elements;
        
        public void Draw()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i].Render();
            }   
        }
        internal void AddRenderableElement(RenderableElement element)
        {
            _elements.Add(element);
        }
    }
}
