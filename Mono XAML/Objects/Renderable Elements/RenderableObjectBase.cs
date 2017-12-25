using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoXAML.Objects
{
    public abstract class RenderableObjectBase : IRenderableObject
    {
        public Vector2 WorldPosition
        {
            get
            {
                return HasParent ? Parent.WorldPosition + LocalPosition : LocalPosition;
            }
            set
            {
                LocalPosition = value - Parent.WorldPosition;
            }
        }
        public Vector2 WorldSize
        {
            get
            {
                return HasParent ? Parent.WorldSize + LocalSize : LocalSize;
            }
            set
            {
                LocalSize = value - Parent.LocalSize;
            }
        }

        public Vector2 LocalPosition { get; set; } = Vector2.One * DEFAULT_POSITION;
        public Vector2 LocalSize { get; set; } = Vector2.One * DEFAULT_SIZE;

        public Rectangle Rect { get { return new Rectangle(WorldPosition.ToPoint(), WorldSize.ToPoint()); } }
        public bool HasParent { get { return _parent != null; } }
        public RenderableObjectBase Parent { get { return _parent; } }
        public IEnumerable<RenderableObjectBase> Children { get { return _children; } }

        private const float DEFAULT_POSITION = 0;
        private const float DEFAULT_SIZE = 100;

        private RenderableObjectBase _parent;
        private List<RenderableObjectBase> _children = new List<RenderableObjectBase>();

        private void RemoveChild(RenderableObjectBase obj)
        {
            _children.Remove(obj);
        }
        private void RegisterChild(RenderableObjectBase obj)
        {
            obj._parent = this;

            _children.Add(obj);
        }
        public void SetParent(RenderableObjectBase obj)
        {
            SetParent(obj, true);
        }
        public void SetParent(RenderableObjectBase obj, bool retainWorldTransform)
        {
            if (_parent != null)
            {
                _parent.RemoveChild(this);
            }

            ConvertWorldValues(obj);

            obj.RegisterChild(this);
        }
        /// <summary>
        /// Will recalculate local transform values as to retain current world transform in relation to new parent
        /// </summary>
        private void ConvertWorldValues(RenderableObjectBase newParent)
        {
            LocalPosition = WorldPosition - newParent.WorldPosition;
            LocalSize = WorldSize - newParent.WorldSize;
        }
        public void Render()
        {
            DoRender();

            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Render();
            }
        }
        protected virtual void DoRender() { }
    }
}
