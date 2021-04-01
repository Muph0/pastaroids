using System;
using Microsoft.Xna.Framework;

namespace gamedev_attempt_01 {

    public static class RandomEx {
        public static float NextFloat(this Random rnd) {
            return (float)rnd.NextDouble();
        }
        public static float NextFloat(this Random rnd, float min, float max) {
            return (float)rnd.NextDouble() * (max - min) + min;
        }
        public static float NextFloat(this Random rnd, float min, float max, float tension) {
            return MathF.Pow((float)rnd.NextDouble(), tension) * (max - min) + min;
        }
        public static bool Bernoulli(this Random rnd, float probability) {
            return rnd.NextDouble() <= probability;
        }

    }

    public static class MathFx {
        public static float Mod(float x, float m) {
            return ((x % m) + m) % m;
        }
    }

    public static class Vector2Ex {
        public static Vector2 FromAngle(float angle) {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }
    }

}