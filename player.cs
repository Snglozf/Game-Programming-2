using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class Player
{
    
    Texture2D tex;
    Texture2D bulletTex;
    public Vector2 Pos;
    public Vector2 Velocity;
    public int HP = 10;
    public int MaxHP = 100;
    public bool isJumping = false;
    
    
    public int Direction = 1;
    
    public List<Bullet> Bullets = new List<Bullet>();
    private KeyboardState oldState;
    public Rectangle PlayerHitBox => new Rectangle((int)Pos.X, (int)Pos.Y, 52, 68);

    public Player(Texture2D t, Texture2D bt, Vector2 p)
    {
        tex = t;
        bulletTex = bt;
        Pos = p;
    }

    public void Update(List<Platform> platforms)
    {
        KeyboardState ks = Keyboard.GetState();

        
        if (ks.IsKeyDown(Keys.A))
        {
            Pos.X -= 5;
            Direction = -1;
           
            foreach (var p in platforms)
            {
                if (PlayerHitBox.Intersects(p.HitBox))
                    Pos.X = p.HitBox.Right;
            }
        }
        if (ks.IsKeyDown(Keys.D))
        {
            Pos.X += 5;
            Direction = 1;
            
            foreach (var p in platforms)
            {
                if (PlayerHitBox.Intersects(p.HitBox))
                    Pos.X = p.HitBox.Left - PlayerHitBox.Width;
            }
        }

        
        if (Pos.X < 0) Pos.X = 0;
        if (Pos.X > 800 - PlayerHitBox.Width) Pos.X = 800 - PlayerHitBox.Width;

        
        Velocity.Y += 0.6f; 
        Pos.Y += Velocity.Y;

        
        if (Pos.Y < 0)
        {
            Pos.Y = 0;
            Velocity.Y = 0;
        }

        foreach (var p in platforms)
        {
            if (PlayerHitBox.Intersects(p.HitBox))
            {
                if (Velocity.Y > 0) 
                {
                    Pos.Y = p.HitBox.Top - PlayerHitBox.Height;
                    Velocity.Y = 0;
                    isJumping = false;
                }
                else if (Velocity.Y < 0) 
                {
                    Pos.Y = p.HitBox.Bottom;
                    Velocity.Y = 0;
                }
            }
        }

         if (ks.IsKeyDown(Keys.W) && !isJumping)
        {
            Velocity.Y = -13f; 
            isJumping = true;
        }

         if (ks.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
        {
            
            Bullets.Add(new Bullet(bulletTex, new Vector2(Pos.X + 10, Pos.Y + 15), new Vector2(12 * Direction, 0)));
        }

        foreach (var b in Bullets) b.Update();
        
        
        Bullets.RemoveAll(b => !b.Active || b.Pos.X > 800 || b.Pos.X < 0);

        oldState = ks;
    }

    public void Draw(SpriteBatch sb)
    {
        
        sb.Draw(tex, PlayerHitBox, Color.CornflowerBlue);
        
        
        foreach (var b in Bullets)
            b.Draw(sb);
    }
}