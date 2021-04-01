using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace gamedev_attempt_01 {

    interface IUpdateable {
        void Update(Context context);
    }
    interface IDrawable {
        void Draw(Context context);
    }

    sealed class Context {
        public GameTime Time { get; set; }
        public LoadedContent Content { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public World World { get; set; }
        public Random Random { get; private set; }
        public Camera Camera => World.Camera;
        public Starfield Starfield { get; set; }
        public KeyboardState KeyboardState { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public float dt => (float)Time.ElapsedGameTime.TotalSeconds;

        public Context() {
            Random = new Random();
        }
    }

}