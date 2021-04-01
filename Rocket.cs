using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace gamedev_attempt_01 {

    class Rocket : MovingEntity {
        private readonly IUserInput userInput;
        private readonly float forwardThrust = 10;
        private readonly float angularThrust = 6;
        private readonly float maxAngularVelocity = 3f;
        private readonly float amunitionLossOnShoot = 1;

        public override Tileset Tileset => tileset;
        Tileset tileset;

        public float KetchupAmount { get; set; }

        public Rocket() {
            userInput = new KeyboardInput();
            Depth = 0.5f;
        }

        public override void Update(Context context) {
            float dt = (float)context.Time.ElapsedGameTime.TotalSeconds;

            userInput.Update(context);

            HandleAcceleration(userInput.GoForwardAmount, dt);
            HandleRotation(userInput.RotateClockwiseAmount, userInput.RotateCounterClockwiseAmount, dt);

            if (userInput.Shoot) {
                Shoot(context);
            }

            base.Update(context);
        }

        private void HandleAcceleration(float forwardAcc, float dt) {
            Velocity += Forward * forwardAcc * forwardThrust * dt;
        }

        private void HandleRotation(float clockwiseAcc, float counterClockwiseAcc, float dt) {
            AngularVelocity += (clockwiseAcc - counterClockwiseAcc) * angularThrust * dt;
            AngularVelocity = Math.Clamp(AngularVelocity, -maxAngularVelocity, maxAngularVelocity);

            if (clockwiseAcc == 0 && counterClockwiseAcc == 0) {
                float angVelDecrease = Math.Min(Math.Abs(AngularVelocity), 1 * angularThrust * dt);
                AngularVelocity -= angVelDecrease * MathF.Sign(AngularVelocity);
            }
        }

        public void Shoot(Context ctx) {
            KetchupAmount -= amunitionLossOnShoot;
            var bullet = new Bullet(this);
            ctx.World.AddEntity(bullet);
        }

        public override void Draw(Context context) {
            tileset = context.Content.Raketa;
            base.Draw(context);
        }

    }

    class Bullet : MovingEntity {
        public float Speed { get; set; } = 50;
        public float Lifetime { get; private set; } = 99999;

        public Bullet(Rocket rocket) {
            Position = rocket.Position;
            Orientation = rocket.Orientation;
            //Velocity = rocket.Forward * Speed + rocket.Velocity;
            Velocity = Vector2.Zero;
            AngularVelocity = 0;
            Depth = 0.6f;
        }

        public override void Update(Context ctx) {
            Lifetime -= ctx.dt;
            if (Lifetime <= 0) {
                Destroyed = true;
                return;
            }
            base.Update(ctx);
        }

        public override void Draw(Context ctx) {
            var sb = ctx.SpriteBatch;
            sb.Draw(sb.Pixel(), ctx.Camera.WorldToScreen(Position),
                null, Color.Red, Orientation, new Vector2(.5f),
                new Vector2(40, 6), SpriteEffects.None, 0f);
        }
    }



    interface IUserInput : IUpdateable {

        float GoForwardAmount { get; }
        float RotateClockwiseAmount { get; }
        float RotateCounterClockwiseAmount { get; }
        bool Shoot { get; }
    }

    class KeyboardInput : IUserInput {
        public float GoForwardAmount { get; private set; }

        public float RotateClockwiseAmount { get; private set; }

        public float RotateCounterClockwiseAmount { get; private set; }

        public bool Shoot { get; private set; }

        KeyboardState lastKbs;

        public void Update(Context context) {
            var kbs = context.KeyboardState;

            if (kbs.IsKeyDown(Keys.W) || kbs.IsKeyDown(Keys.Up)) {
                GoForwardAmount = 1f;
            } else {
                GoForwardAmount = 0f;
            }

            if (kbs.IsKeyDown(Keys.A) || kbs.IsKeyDown(Keys.Left)) {
                RotateCounterClockwiseAmount = 1f;
            } else {
                RotateCounterClockwiseAmount = 0f;
            }

            if (kbs.IsKeyDown(Keys.D) || kbs.IsKeyDown(Keys.Right)) {
                RotateClockwiseAmount = 1f;
            } else {
                RotateClockwiseAmount = 0f;
            }

            Shoot = kbs.IsKeyDown(Keys.Space) && lastKbs.IsKeyUp(Keys.Space);

            lastKbs = context.KeyboardState;
        }
    }

}