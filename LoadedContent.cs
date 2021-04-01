using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace gamedev_attempt_01 {
    class LoadedContent {

        public readonly Tileset Kolinko, Maslicka, Raviola, Vrtulka, Raketa, KapkaKechupu;

        public LoadedContent(ContentManager content) {

            Kolinko = new Tileset(content.Load<Texture2D>("kolinko"), 1, 1, new Rectangle(0, 0, 256, 256));
            Maslicka = new Tileset(content.Load<Texture2D>("maslicka"), 1, 1, new Rectangle(0, 0, 256, 256));
            Raviola = new Tileset(content.Load<Texture2D>("Raviola"), 1, 1, new Rectangle(0, 0, 256, 256));
            Vrtulka = new Tileset(content.Load<Texture2D>("vrtulka"), 1, 1, new Rectangle(0, 0, 256, 256));
            Raketa = new Tileset(content.Load<Texture2D>("raketa"), 1, 1, new Rectangle(0, 0, 256, 256));
            KapkaKechupu = Raketa;
        }

    }
}
