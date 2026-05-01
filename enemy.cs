using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Enemy
{
    Texture2D tex;
    Texture2D bTex;
    public Vector2 Pos;
    public int HP = 100;
    float timer = 0;
    public List<Bullet> Bullets = new List<Bullet>();
    public Rectangle HitBox => new Rectangle((int)Pos.X, (int)Pos.Y, 50, 50);

    public Enemy(Texture2D t, Texture2D bt, Vector2 p)
    {
        tex = t; bTex = bt; Pos = p;
    }

    public void Update(GameTime gt, Vector2 playerPos)
    {
        float dt = (float)gt.ElapsedGameTime.TotalSeconds;
        timer += dt;
        if (timer > 1.5f)
        {
            float side = (playerPos.X > Pos.X) ? 6 : -6;
            Bullets.Add(new Bullet(bTex, new Vector2(Pos.X, Pos.Y + 20), new Vector2(side, 0)));
            timer = 0;
        }
        foreach (var b in Bullets) b.Update();
        Bullets.RemoveAll(b => !b.Active);
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(tex, HitBox, Color.Red);
        foreach (var b in Bullets) b.Draw(sb);
    }
}