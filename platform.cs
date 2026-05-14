using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Platform
{
    public Texture2D tex;
    public Rectangle HitBox;
    public Platform(Texture2D t, Rectangle r) { tex = t; HitBox = r; }
}