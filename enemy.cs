using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Enemy
{
    public Texture2D tex;
    public Texture2D bulletTex;
    public Vector2 Pos;
    public Vector2 Velocity;
    public int HP = 250;
    public bool Active = true;
    private float shootTimer = 0f;
    private float shootInterval = 1.2f; 
    private static Random rnd = new Random(); 
    private float jumpTimer = 0f;
    public bool IsBattleStarted = false;

    public List<Bullet> Bullets = new List<Bullet>();
    public Rectangle HitBox => new Rectangle((int)Pos.X, (int)Pos.Y, 50, 50);

    public Enemy(Texture2D t, Texture2D bt, Vector2 p)
    {
        tex = t;
        bulletTex = bt;
        Pos = p;
    }

    
   public void Update(GameTime gameTime, Vector2 playerPos, List<Platform> platforms)
{
    
    if (HP <= 0) { Active = false; return; }


    Velocity.Y += 0.5f; 
    Pos.Y += Velocity.Y;

    
    foreach (var p in platforms)
    {
        if (this.HitBox.Intersects(p.HitBox))
        {
            if (Velocity.Y > 0) 
            {
                Pos.Y = p.HitBox.Top - HitBox.Height;
                Velocity.Y = 0;
            }
        }
    }

    
    if (playerPos.Y > 450) 
    {
        IsBattleStarted = true;
    }

    
    if (IsBattleStarted)
    {
        
        if (Pos.X < playerPos.X - 60) Pos.X += 1.5f;
        else if (Pos.X > playerPos.X + 60) Pos.X -= 1.5f;

        
        jumpTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (jumpTimer >= 1.0f) 
        {
            
            if (Velocity.Y == 0 && rnd.Next(0, 100) < 30) 
            {
                Velocity.Y = -12f; 
            }
            jumpTimer = 0f; 
        }

        
        shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (shootTimer >= shootInterval)
        {
            Shoot(playerPos);
            shootTimer = 0f;
        }
    }

   
    foreach (var b in Bullets) b.Update();
    Bullets.RemoveAll(b => !b.Active);
}
    private void Shoot(Vector2 target)
    {
        Vector2 direction = target - Pos;
        direction.Normalize();
        Bullets.Add(new Bullet(bulletTex, new Vector2(Pos.X + 25, Pos.Y + 20), direction * 8f));
    }

    public void Draw(SpriteBatch sb)
    {
        if (!Active) return;
        sb.Draw(tex, HitBox, Color.Red); 
        foreach (var b in Bullets) b.Draw(sb);
    }
}