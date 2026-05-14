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
        Texture2D backgroundTexture;
        Texture2D DoorTexture;
        Texture2D healthTexture;
        Texture2D spikeTexture;

        SpriteFont myFont;
        int levelNum = 1;

        double spikeTimer = 0;

        enum State { Menu, Play, Over, Win }
        State gameState = State.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            myFont = Content.Load<SpriteFont>("MyFont");
            DoorTexture = Content.Load<Texture2D>("door");
            healthTexture = Content.Load<Texture2D>("health");
            spikeTexture = Content.Load<Texture2D>("spike");

            try { backgroundTexture = Content.Load<Texture2D>("background"); }
            catch { }
        }

        void LoadLevel(int n)
        {
            level = new Level();

            Vector2 spawnPos;

            if (n == 1)
                spawnPos = new Vector2(50, 350);
            else if (n == 2)
                spawnPos = new Vector2(50, 80);
            else
                spawnPos = new Vector2(50, 80);

            if (player == null)
            {
                player = new Player(pixel, pixel, spawnPos);
                player.HP = 10;
            }
            else
            {
                player.Pos = spawnPos;
            }

            if (n == 1)
            {
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 430, 260, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(460, 480, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(580, 480, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(660, 390, 140, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(250, 270, 50, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(360, 290, 260, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 230, 180, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(100, 110, 250, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(350, 110, 80, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(520, 110, 130, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(690, 110, 110, 30)));

                level.Door = new Door(DoorTexture, new Rectangle(720, 10, 113, 125));

                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(150, 370, 80, 45)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(500, 220, 80, 45)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(600, 60, 80, 45)));

                level.Spikes.Add(new Spike(spikeTexture, new Rectangle(0, 560, 800, 40)));
            }
            else if (n == 2)
            {
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 150, 130, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 520, 180, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(260, 480, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(360, 420, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(450, 360, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(500, 240, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(620, 360, 180, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(620, 150, 180, 30)));

                level.Door = new Door(DoorTexture, new Rectangle(730, 50, 113, 125));

                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(280, 420, 80, 45)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(650, 300, 80, 45)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(0, 450, 80, 45)));

                level.Spikes.Add(new Spike(spikeTexture, new Rectangle(0, 560, 800, 40)));
            }
            else if (n == 3)
            {
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 150, 150, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(280, 220, 150, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(530, 320, 220, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(760, 320, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 530, 800, 70)));

                level.Enemy = new Enemy(pixel, pixel, new Vector2(600, 480));

                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(320, 170, 80, 45)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(580, 270, 80, 45)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(700, 270, 80, 45)));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Escape)) Exit();
          
if (ks.IsKeyDown(Keys.R)) 
{
    gameState = State.Menu;
    
    if (player != null) 
    {
        player.HP = 10; 
    }
}

if (gameState == State.Menu)
{
    if (ks.IsKeyDown(Keys.Enter))
    {
        levelNum = 1;
        LoadLevel(levelNum); 
        gameState = State.Play;
    }
}
            else if (gameState == State.Play)
            {
                player.Update(level.Platforms);

                
                foreach (var s in level.Spikes)
                {
                    if (player.PlayerHitBox.Intersects(s.HitBox))
                    {
                        player.HP = 0;
                    }
                }

                foreach (var hp in level.HealthPacks)
                {
                    if (hp.Active &&
                        player.PlayerHitBox.Intersects(hp.HitBox))
                    {
                        player.HP += 10;
                        hp.Active = false;
                    }
                }

                if (level.Enemy != null)
                    level.Enemy.Update(gameTime, player.Pos, level.Platforms);

                if (level.Door != null &&
                    player.PlayerHitBox.Intersects(level.Door.HitBox))
                {
                    levelNum++;

                    if (levelNum > 3)
                        gameState = State.Win;
                    else
                        LoadLevel(levelNum);
                }

                if (player.HP <= 0 || player.Pos.Y > 600)
                    gameState = State.Over;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if (backgroundTexture != null)
                spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 800, 600), Color.White);

            if (gameState == State.Menu)
            {
                spriteBatch.Draw(pixel, new Rectangle(0, 0, 800, 600), Color.Black * 0.6f);
                spriteBatch.DrawString(myFont, "SLOWRISE", new Vector2(300, 150), Color.Yellow);
                spriteBatch.DrawString(myFont, "PRESS ENTER TO START", new Vector2(230, 250), Color.White);
                spriteBatch.DrawString(myFont, "PRESS R TO EXIT", new Vector2(260, 350), Color.White);
            }
            else if (gameState == State.Play)
            {
                foreach (var s in level.Spikes)
                    s.Draw(spriteBatch);

                foreach (var hp in level.HealthPacks)
                    hp.Draw(spriteBatch);

                level.Draw(spriteBatch);
                player.Draw(spriteBatch);

                if (level.Enemy != null)
                    level.Enemy.Draw(spriteBatch);

                spriteBatch.Draw(pixel, new Rectangle(10, 10, 300, 25), Color.Black * 0.5f);
                spriteBatch.Draw(pixel, new Rectangle(10, 10, player.HP * 2, 25), Color.Red);

                spriteBatch.DrawString(myFont, "HP: " + player.HP, new Vector2(15, 10), Color.White);
                spriteBatch.DrawString(myFont, "LEVEL: " + levelNum, new Vector2(650, 10), Color.Gold);
            }
            else if (gameState == State.Over)
            {
                spriteBatch.DrawString(myFont, "GAME OVER", new Vector2(340, 250), Color.White);
            }
            else if (gameState == State.Win)
            {
                spriteBatch.DrawString(myFont, "YOU WIN!", new Vector2(350, 250), Color.Gold);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}