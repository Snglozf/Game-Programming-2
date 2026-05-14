using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Door
{
    public Texture2D tex;
    public Rectangle HitBox;
    public Door(Texture2D t, Rectangle r) { tex = t; HitBox = r; }
}