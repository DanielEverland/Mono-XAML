using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoXAML
{
    public static class DefaultTextures
    {
        public static Texture2D White
        {
            get
            {
                if (_white == null)
                    CreateWhite();

                return _white;
            }
        }
        private static Texture2D _white;

        private static void CreateWhite()
        {
            _white = Create(Color.White);   
        }
        private static Texture2D Create(Color color)
        {
            Texture2D texture = new Texture2D(XAMLManager.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            texture.SetData(new Color[1] { color });

            return texture;
        }
    }
}
