using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoXAML.Objects
{
    public class Stroke : RenderableObjectBase
    {
        private Stroke() { }
        public Stroke(System.Windows.Media.Brush brush, int thickness)
        {
            Thickness = thickness;
            _brush = brush;
        }

        public int Thickness
        {
            get
            {
                return _thickness;
            }
            set
            {
                _thickness = Utility.Clamp(value, 0, int.MaxValue);
            }
        }

        private StrokeSlave Top { get { return _slaves[0]; } set { _slaves[0] = value; } }
        private StrokeSlave Right { get { return _slaves[1]; } set { _slaves[1] = value; } }
        private StrokeSlave Bottom { get { return _slaves[2]; } set { _slaves[2] = value; } }
        private StrokeSlave Left { get { return _slaves[3]; } set { _slaves[3] = value; } }

        private int _thickness;
        private System.Windows.Media.Brush _brush;
        private Rectangle _previousRect;
        private StrokeSlave[] _slaves;

        protected override void DoRender()
        {
            PollSlaves();

            for (int i = 0; i < _slaves.Length; i++)
            {
                XAMLManager.SpriteBatch.Draw(_slaves[i].texture, _slaves[i].rect, _slaves[i].color);
            }
        }
        private void PollSlaves()
        {
            if (_previousRect != Rect || _slaves == null)
                CreateSlaves();
        }
        private void CreateSlaves()
        {
            Top = new StrokeSlave()
            {
                rect = new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, Width, Thickness),
                texture = Utility.BrushToTexture(_brush, )
            }
        }

        private struct StrokeSlave
        {
            public Rectangle rect;
            public Texture2D texture;
            public Color color;
        }
    }
}
