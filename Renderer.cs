using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gamedev_attempt_01 {

    sealed class Renderer : IDrawable {
        SpriteBatch sb;
        GraphicsDevice device;

        public Renderer(Context context) {
            sb = context.SpriteBatch;
            device = context.GraphicsDevice;
        }

        public void Draw(Context context) {

            context.GraphicsDevice.Clear(Color.Black);
            sb.Begin(sortMode: SpriteSortMode.FrontToBack);

            context.Starfield.Draw(context, context.Camera.ScreenViewport);

            foreach (var entity in context.World.VisibleEntities()) {
                entity.Draw(context);
            }

            sb.End();
        }
    }

}