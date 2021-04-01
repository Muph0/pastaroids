using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamedev_attempt_01 {
    sealed class Tileset {

        public Texture2D Texture { get; private set; }

        private int framesInRow, frameCount;
        private Rectangle zerothFrame;

        public Tileset(Texture2D Texture, int framesInRow, int frameCount, Rectangle zerothFrame) {
            this.Texture = Texture;
            this.framesInRow = framesInRow;
            this.frameCount = frameCount;
            this.zerothFrame = zerothFrame;
        }

        public Rectangle GetFrame(int frame) {

            int x = (frame % framesInRow) * zerothFrame.Width;
            int y = (frame / framesInRow) * zerothFrame.Height;

            Rectangle result = zerothFrame;
            result.Offset(x, y);
            return result;
        }
    }
}
