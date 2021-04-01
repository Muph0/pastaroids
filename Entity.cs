using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace gamedev_attempt_01 {

    // has position, orientation and velocity
    abstract class Entity : IUpdateable, IDrawable {
        public Vector2 Position;
        public float Orientation { get; set; }
        public RectangleF BoundingBox;
        public bool Destroyed { get; set; } = false;
        // TODO: test this
        public Vector2 Forward => Vector2Ex.FromAngle(Orientation);
        public virtual Tileset Tileset => throw new NotImplementedException();
        public int Frame { get; set; }
        protected float Depth = 0f;

        public abstract void Update(Context context);
        public virtual void Draw(Context context) {
            Vector2 screenPos = context.Camera.WorldToScreen(Position);
            Rectangle frame = Tileset.GetFrame(Frame);
            context.SpriteBatch.Draw(
                Tileset.Texture,
                screenPos,
                frame,
                Color.White,
                Orientation + MathF.PI / 2,
                frame.Size.ToVector2() / 2,
                scale: 1f,
                SpriteEffects.None,
                Depth);
        }
    }

    abstract class MovingEntity : Entity {
        public Vector2 Velocity { get; set; }
        public float AngularVelocity { get; set; }

        public override void Update(Context context) {
            float dt = (float)context.Time.ElapsedGameTime.TotalSeconds;
            Position += Velocity * dt;
            Orientation += AngularVelocity * dt;
        }
        public void Freeze() {
            Velocity = Vector2.Zero;
            AngularVelocity = 0;
        }
    }


    // has health and type of pasta information
    class Pasta : MovingEntity {
        public float Health { get; set; }
        public PastaType Type { get; set; }
        public int Size { get; set; }
        public bool Killed { get; set; } = false;
        public override Tileset Tileset => Type.Tileset;

        public void Hit(float damage) {
            Health -= damage;
            if (Health <= 0) {
                Killed = true;
            }
        }

        public void Explode(Context ctx) {
            Die();

            if (Size > 1) {
                int pieces = Type.ReplicationFactor;
                for (int i = 0; i < pieces; i++) {
                    float angle = 2 * MathF.PI * i / pieces;
                    Pasta smallPasta = Type.CreatePasta(ctx.Random, Size - 1);
                    smallPasta.Position = Position;
                    smallPasta.Velocity = Velocity + Vector2Ex.FromAngle(angle) * Type.InitSpeed;
                    ctx.World.AddEntity(smallPasta);
                }
            }
            var ketchup = Ketchup.GenerateKetchup(ctx.Random, Type.KetchupGenProb);
            if (ketchup != null) { ctx.World.AddEntity(ketchup); }
        }

        public override void Update(Context context) {
            if (Killed) {
                Explode(context);
            }
            base.Update(context);
        }

        public void Die() {
            Destroyed = true;
        }

    }

    class PastaType {
        public float InitHealth { get; set; }
        public float InitSpeed { get; set; }
        public float SpinSpeed { get; set; }
        public int InitSize { get; set; }
        public int ReplicationFactor { get; set; }
        public Tileset Tileset { get; set; }

        public float KetchupGenProb { get; set; } = 0.5f;

        public PastaType(float initHealth, float initVelocity, float spinSpeed, int initialSize, int replicationFactor, Tileset tileset) {
            InitHealth = initHealth;
            InitSpeed = initVelocity;
            SpinSpeed = spinSpeed;
            InitSize = initialSize;
            ReplicationFactor = replicationFactor;
            Tileset = tileset;
        }

        public static PastaType Kolinko;
        public static PastaType Maslicka;
        public static PastaType Vrtulka;
        public static PastaType Raviola;

        public static void Initialize(Context ctx) {
            Kolinko = new PastaType(4, 10, 1, 2, 3, ctx.Content.Kolinko);
            Maslicka = new PastaType(4, 10, 1, 2, 3, ctx.Content.Maslicka);
            Vrtulka = new PastaType(8, 10, 1, 3, 2, ctx.Content.Vrtulka);
            Raviola = new PastaType(10, 10, 1, 3, 2, ctx.Content.Raviola);
        }
        public static List<PastaType> AllTypes = new List<PastaType> { Kolinko, Maslicka, Vrtulka, Raviola };

        private float getNewHealth(int size) {
            return InitHealth / MathF.Pow(2, size) + 1;
        }

        public Pasta CreatePasta(Random rnd) {
            return CreatePasta(rnd, InitSize);
        }

        public Pasta CreatePasta(Random rnd, int size) {
            Pasta p = new Pasta();
            p.Orientation = rnd.NextFloat(0, 2 * MathF.PI);
            p.Velocity = new Vector2(MathF.Cos(p.Orientation), MathF.Sin(p.Orientation));
            p.AngularVelocity = SpinSpeed * (rnd.Bernoulli(.5f) ? 1 : -1) * rnd.NextFloat(0.5f, 1.5f, 1.6f);
            p.Type = this;
            p.Size = size;
            p.Health = getNewHealth(size);
            return p;
        }
    }

    class Ketchup : Entity {
        public const float Nourishment = 10; // ToDo: come up with nourishment depending on Health
        public override Tileset Tileset => tileset;
        Tileset tileset;

        public float Collect() {
            Destroyed = true;
            return Nourishment;
        }

        public static Ketchup GenerateKetchup(Random random, float probability) {
            if (random.Bernoulli(probability)) {
                var ketchup = new Ketchup();
                return ketchup;
            }
            return null;
        }

        public override void Update(Context ctx) {}

        public override void Draw(Context ctx) {
            tileset = ctx.Content.KapkaKechupu;
            base.Draw(ctx);
        }
    }
}
