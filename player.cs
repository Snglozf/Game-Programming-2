using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class Player
{
    Texture2D tex;
    Texture2D bullTex;
    public Vector2 Pos;
    Vector2 vel;
    bool jumping;
    public int HP = 100;
    public List<Bullet> Bullets = new List<Bullet>();
    KeyboardState oldKeys;
    float timer = 0;

    public Rectangle HitBox => new Rectangle((int)Pos.X, (int)Pos.Y, 40, 40);

    public Player(Texture2D t, Texture2D bt, Vector2 p)
    {
        tex = t; bullTex = bt; Pos = p;
    }

    public void Update(List<Platform> platforms)
    {
        var ks = Keyboard.GetState();
        if (ks.IsKeyDown(Keys.D)) Pos.X += 5;
        if (ks.IsKeyDown(Keys.A)) Pos.X -= 5;
        if (ks.IsKeyDown(Keys.W) && !jumping) { vel.Y = -12; jumping = true; }

        timer -= 0.016f;
        if (ks.IsKeyDown(Keys.Space) && oldKeys.IsKeyUp(Keys.Space) && timer <= 0)
        {
            Bullets.Add(new Bullet(bullTex, new Vector2(Pos.X + 20, Pos.Y + 15), new Vector2(10, 0)));
            timer = 0.4f;
        }

        vel.Y += 0.5f;
        Pos += vel;

        foreach (var p in platforms)
        {
            if (HitBox.Intersects(p.HitBox))
            {
                // Land on top
                if (vel.Y > 0 && Pos.Y + 20 < p.HitBox.Top)
                {
                    Pos.Y = p.HitBox.Y - 40;
                    vel.Y = 0;
                    jumping = false;
                }
                // Hit head from bottom
                else if (vel.Y < 0 && Pos.Y > p.HitBox.Bottom - 15)
                {
                    Pos.Y = p.HitBox.Bottom;
                    vel.Y = 0;
                }
            }
        }

        foreach (var b in Bullets) b.Update();
        Bullets.RemoveAll(b => !b.Active);
        oldKeys = ks;
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(tex, HitBox, Color.White);
        foreach (var b in Bullets) b.Draw(sb);
    }
}