using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Bullet
{
    public Vector2 Pos;
    Vector2 speed;
    Texture2D tex;
    public bool Active = true;
    public Rectangle HitBox => new Rectangle((int)Pos.X, (int)Pos.Y, 10, 10);

    public Bullet(Texture2D t, Vector2 p, Vector2 s)
    {
        tex = t; Pos = p; speed = s;
    }

    public void Update()
    {
        Pos += speed;
        if (Pos.X > 800 || Pos.X < 0) Active = false;
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(tex, HitBox, Color.Yellow);
    }
}