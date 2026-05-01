using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PROJECT
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        Level level;
        Texture2D pixel;
        int levelNum = 1;

        enum State { Menu, Play, Over, Win }
        State gameState = State.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        void LoadLevel(int n)
        {
            level = new Level();
            // Starts on the ground level, not falling from sky
            player = new Player(pixel, pixel, new Vector2(50, 350));

            if (n == 1)
            {
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 400, 800, 50)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(300, 280, 200, 20)));
                level.Door = new Door(pixel, new Rectangle(720, 350, 40, 50));
            }
            else if (n == 2)
            {
                // Level 2: Climb up from bottom to top
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 400, 800, 50))); 
                level.Platforms.Add(new Platform(pixel, new Rectangle(150, 300, 100, 20)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(350, 200, 100, 20)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(550, 100, 100, 20)));
                level.Door = new Door(pixel, new Rectangle(580, 50, 40, 50));
            }
            else if (n == 3)
            {
                // Level 3: Drop down to fight the boss
                player.Pos = new Vector2(50, 50); // Starts high for this specific level
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 100, 150, 20)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 400, 800, 50)));
                level.Enemy = new Enemy(pixel, pixel, new Vector2(600, 350));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            if (gameState == State.Menu && ks.IsKeyDown(Keys.Enter))
            {
                levelNum = 1;
                LoadLevel(levelNum);
                gameState = State.Play;
            }

            if (gameState == State.Play)
            {
                player.Update(level.Platforms);

                if (level.Enemy != null)
                {
                    level.Enemy.Update(gameTime, player.Pos);
                    foreach (var b in player.Bullets)
                        if (b.HitBox.Intersects(level.Enemy.HitBox)) { level.Enemy.HP -= 25; b.Active = false; }
                    foreach (var b in level.Enemy.Bullets)
                        if (b.HitBox.Intersects(player.HitBox)) { player.HP -= 10; b.Active = false; }
                    if (level.Enemy.HP <= 0) gameState = State.Win;
                }

                if (level.Door != null && player.HitBox.Intersects(level.Door.HitBox))
                {
                    levelNum++;
                    if (levelNum > 3) gameState = State.Win;
                    else LoadLevel(levelNum);
                }

                if (player.HP <= 0 || player.Pos.Y > 600) gameState = State.Over;
            }

            if (ks.IsKeyDown(Keys.R)) gameState = State.Menu;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (gameState == State.Play) { level.Draw(spriteBatch); player.Draw(spriteBatch); }
            else { spriteBatch.Draw(pixel, new Rectangle(300, 200, 200, 100), Color.Gray); }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}