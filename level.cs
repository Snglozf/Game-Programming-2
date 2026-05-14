using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PROJECT;

public class Level
{
    public List<Platform> Platforms = new List<Platform>();
    public Door Door;
    public Enemy Enemy;

    public List<HealthPack> HealthPacks = new List<HealthPack>();

    public List<Spike> Spikes = new List<Spike>();

    public void Draw(SpriteBatch sb)
    {
        foreach (var p in Platforms)
            sb.Draw(p.tex, p.HitBox, Color.DarkGreen);

        if (Door != null)
            sb.Draw(Door.tex, Door.HitBox, Color.Brown);

        if (Enemy != null)
            Enemy.Draw(sb);

        foreach (var spike in Spikes)
            spike.Draw(sb);
    }
}