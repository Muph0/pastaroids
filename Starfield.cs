using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gamedev_attempt_01 {
    class Starfield {

        int seed;
        float starDensity;

        public Starfield(int seed, float starsPerMPixel) {
            this.seed = seed;
            this.starDensity = starsPerMPixel;
        }

        public void Draw(Context context, Rectangle viewport) {

            Random rnd = new Random(seed);
            int starAmount = (int)(starDensity * viewport.Width * viewport.Height / 1E6f);

            for (int i = 0; i < starAmount; i++) {

                var sb = context.SpriteBatch;

                Vector2 star;
                star.X = rnd.NextFloat(-10000, 10000);
                star.Y = rnd.NextFloat(-10000, 10000);
                float starZ = rnd.NextFloat(1, 100, 1f);
                float brightness = rnd.NextFloat(10, 800, 8f);

                Vector2 offset = -context.Camera.Center * context.Camera.Zoom;
                star += offset / starZ;

                star.X = MathFx.Mod(star.X, viewport.Width);
                star.Y = MathFx.Mod(star.Y, viewport.Height);

                int rayCount = rnd.Next(2, 9);
                float size = rnd.NextFloat(1, 7, 2);
                int alpha = (int)Math.Clamp(brightness / rayCount + 16, 0, 255);

                for (int r = 0; r < rayCount; r++) {

                    float angle = r * MathF.PI / rayCount;
                    sb.Draw(
                        sb.Pixel(),
                        star,
                        sourceRectangle: null,
                        Color.FromNonPremultiplied(255, 255, 255, alpha),
                        angle,
                        new Vector2(.5f),
                        new Vector2(1, size) * 1f,
                        SpriteEffects.None, 0f);

                }
            }
        }
    }
}