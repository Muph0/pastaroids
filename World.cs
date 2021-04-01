using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gamedev_attempt_01 {
    // takes care of selecting what to display
    sealed class World : IUpdateable {
        public IReadOnlyList<Entity> Entities => entities.AsReadOnly();
        private List<Entity> entities = new List<Entity>();
        private List<Entity> entitiesToAdd = new List<Entity>();

        public Vector2 Bounds { get; private set; }
        public Camera Camera { get; private set; }
        public Rocket Player { get; private set; }
        Context context;

        public World(Context context) {
            this.context = context;
            context.World = this;

            Camera = new Camera(context.GraphicsDevice);
            Player = new Rocket();

            AddEntity(Player);
            Camera.Target = Player;
        }

        public void AddEntity(Entity e) {
            entitiesToAdd.Add(e);
        }

        public IEnumerable<Entity> VisibleEntities() {
            foreach (Entity e in Entities) {
                if (true || Camera.IsVisible(e.BoundingBox)) {
                    yield return e;
                }
            }
        }

        public void Update(Context context) {
            UpdateEntities(context);
            RemoveDestroyed();
            entities.AddRange(entitiesToAdd);
            entitiesToAdd.Clear();

            Camera.Update(context);
        }

        public void UpdateEntities(Context context) {
            foreach (Entity e in Entities) {
                e.Update(context);
            }
        }

        public void RemoveDestroyed() {
            entities.RemoveAll(e => e.Destroyed);
        }
    }

    sealed class Camera {

        GraphicsDevice graphicsDevice;

        public Camera(GraphicsDevice graphicsDevice) {
            if (graphicsDevice == null) {
                throw new ArgumentNullException();
            }
            this.graphicsDevice = graphicsDevice;
        }

        public Matrix WorldToScreenTransform { get; private set; }
        public Vector2 Center;
        public RectangleF WorldViewport => new RectangleF(ScreenToWorld(Vector2.Zero), ScreenToWorld(ScreenSize));
        public float Zoom { get; set; } = 50;

        /// <summary>
        /// Camera will follow target. If you want to move it on its own, set target to null.
        /// </summary>
        public Entity Target { get; set; }

        public Vector2 WorldToScreen(Vector2 worldPosition) {
            return Vector2.Transform(worldPosition, WorldToScreenTransform);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition) {
            return Vector2.Transform(screenPosition, Matrix.Invert(WorldToScreenTransform));
        }

        public Vector2 ScreenSize => new(
            graphicsDevice.Viewport.Width,
            graphicsDevice.Viewport.Height);

        public Rectangle ScreenViewport => graphicsDevice.Viewport.Bounds;

        public void Update(Context ctx) {
            if (Target != null) {
                Center = Target.Position;
            }

            WorldToScreenTransform = Matrix.CreateTranslation(new Vector3(-Center, 0f)) *
                Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(new Vector3(ScreenSize / 2, 0));
        }

        public bool IsVisible(RectangleF rect) {
            return rect.Intersects(WorldViewport);
        }
    }
}
