using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class Level
{
    public List<Platform> Platforms = new List<Platform>();
    public Door Door;
    public Enemy Enemy;

    public void Draw(SpriteBatch sb)
    {
        foreach (var p in Platforms) sb.Draw(p.tex, p.HitBox, Color.Gray);
        if (Door != null) sb.Draw(Door.tex, Door.HitBox, Color.Brown);
        if (Enemy != null) Enemy.Draw(sb);
    }
}