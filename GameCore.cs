using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace gamedev_attempt_01 {
    public class GameCore : Game {

        GraphicsDeviceManager graphics;
        Context context;
        World world;
        Renderer renderer;

        public GameCore() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent() {
            // TODO: use this.Content to load your game content here
            context = new Context();

            context.Content = new LoadedContent(Content);
            context.GraphicsDevice = GraphicsDevice;
            PastaType.Initialize(context);
            context.SpriteBatch = new SpriteBatch(GraphicsDevice);
            context.Starfield = new Starfield(context.Random.Next(), 1000);
            context.World = world = new World(context);

            renderer = new Renderer(context);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            context.KeyboardState = Keyboard.GetState();
            context.Time = gameTime;

            world.Update(context);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            renderer.Draw(context);

            base.Draw(gameTime);
        }
    }
}
