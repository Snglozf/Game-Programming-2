using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;   

namespace PROJECT
{
    public class Spike
    {
        public Rectangle HitBox;
        Texture2D texture;

        public Spike(Texture2D texture, Rectangle rect)
        {
            this.texture = texture;
            this.HitBox = rect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, HitBox, Color.White);
        }
    }
}