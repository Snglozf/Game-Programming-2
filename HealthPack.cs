using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class HealthPack
{
    public Texture2D Tex;
    public Rectangle HitBox;
    public bool Active = true;

    public HealthPack(Texture2D tex, Rectangle rect)
    {
        Tex = tex;
        HitBox = rect;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (Active)
            spriteBatch.Draw(Tex, HitBox, Color.White);
    }
}