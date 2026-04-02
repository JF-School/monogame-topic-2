using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace fruit_and_veggies_or_something
{
    enum Screen
    {
        Intro,
        Game,
        Outro
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Random generator;
        int gappleSize, bombSize, sizeMax; // the max size for the rectangles
        int score; // total score collected
        bool debugMode, gappleClicked; // boolean variables
        int debugRound;
        int fruitMax, veggieMax, bombMax, gappleMax; // the max amount of rectangles that can exist
        float fruitTimer, veggieTimer, bombTimer, gappleTimer; // the timers for the rectangles
        float fruitRespawn, veggieRespawn, bombRespawn, gappleRespawn; // the respawn times
        float gameStart, gameEnd; // the timer for the game

        MouseState prevMouseState, mouseState;
        KeyboardState prevKeyboardState, keyboardState;
        Screen screen;
        SpriteFont scoreFont, debugFont, gameFont;

        Rectangle window;
        List<Rectangle> fruitRects;
        List<Rectangle> veggieRects;
        List<Rectangle> bombRects;
        List<Rectangle> gappleRects;
        Rectangle playBtn, howBtn, creditsBtn;
        Texture2D bombTexture, explosionTexture, playTexture, howTexture, creditsTexture, gappleTexture;
        Texture2D introBack, gameBack;
        List<Texture2D> frTextures;
        List<Texture2D> vgTextures;
        List<Texture2D> fruitTextures;
        List<Texture2D> veggieTextures;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screen = Screen.Intro;

            // TODO: Add your initialization logic here

            window = new Rectangle(0, 0, 800, 500);
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;

            fruitTimer = 0f; veggieTimer = 0f; bombTimer = 0f; gappleTimer = 0f; // timers (count up)
            gameStart = 0f; gameEnd = 60f; // how long until game end
            fruitRespawn = 3f; veggieRespawn = 2f; gappleRespawn = 8f; // how long it takes for said thing to respawn
            fruitMax = 30; veggieMax = 45; bombMax = 10; gappleMax = 4; // how many rectangles can exist

            generator = new Random();
            gappleSize = 128;
            sizeMax = 48;
            bombSize = 32;

            debugMode = false;
            gappleClicked = false;
            debugRound = 1;

            fruitRects = new List<Rectangle>();
            for (int i = 0; i < 15; i++)
            {
                fruitRects.Add(new Rectangle(generator.Next(window.Width - (sizeMax + 16)), generator.Next(window.Height - (sizeMax + 16)), sizeMax, sizeMax));
            }
            veggieRects = new List<Rectangle>();
            for (int i = 0; i < 15; i++)
            {
                veggieRects.Add(new Rectangle(generator.Next(window.Width - (sizeMax + 16)), generator.Next(window.Height - (sizeMax + 16)), sizeMax, sizeMax));
            }
            bombRects = new List<Rectangle>();
            for (int i = 0; i < 5; i++)
            {
                bombRects.Add(new Rectangle(generator.Next(window.Width - (bombSize + 16)), generator.Next(window.Height - (bombSize + 16)), bombSize, bombSize));
            }
            gappleRects = new List<Rectangle>();
            for (int i = 0; i < 0; i++)
            {
                gappleRects.Add(new Rectangle(generator.Next(window.Width - (gappleSize + 16)), generator.Next(window.Height - (gappleSize + 16)), gappleSize, gappleSize));
            }
            playBtn = new Rectangle(350, 200, 100, 100);

            frTextures = new List<Texture2D>();
            vgTextures = new List<Texture2D>();
            fruitTextures = new List<Texture2D>();
            veggieTextures = new List<Texture2D>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            playTexture = Content.Load<Texture2D>("Images/playbutton");
            howTexture = Content.Load<Texture2D>("Images/howbutton");
            creditsTexture = Content.Load<Texture2D>("Images/creditsbutton");
            bombTexture = Content.Load<Texture2D>("Images/dynamite");
            explosionTexture = Content.Load<Texture2D>("Images/explosion");
            gappleTexture = Content.Load<Texture2D>("Images/goldenapple");

            introBack = Content.Load<Texture2D>("Images/introbackground");
            gameBack = Content.Load<Texture2D>("Images/fruitbackground");

            scoreFont = Content.Load<SpriteFont>("Fonts/ScoreFont");
            debugFont = Content.Load<SpriteFont>("Fonts/DebugFont");
            gameFont = Content.Load<SpriteFont>("Fonts/GameFont");


            for (int i = 1; i <= 40; i++)
            {
                frTextures.Add(Content.Load<Texture2D>($"Images/fruit{i}"));
            }
            for (int i = 1; i <= 30; i++)
            {
                vgTextures.Add(Content.Load<Texture2D>($"Images/veggie{i}"));
            }
            for (int i = 0; i < fruitRects.Count; i++)
            {
                fruitTextures.Add(frTextures[generator.Next(frTextures.Count)]);
            }
            for (int i = 0; i < veggieRects.Count; i++)
            {
                veggieTextures.Add(vgTextures[generator.Next(vgTextures.Count)]);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            switch (screen)
            {
                case Screen.Intro:
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (playBtn.Contains(mouseState.Position))
                            screen = Screen.Game;
                    }
                        break;
                case Screen.Game:
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        for (int i = 0; i < fruitRects.Count; i++)
                        {
                            if (fruitRects[i].Contains(mouseState.Position))
                            {
                                score += 100;
                                fruitRects.RemoveAt(i);
                                fruitTextures.RemoveAt(i);
                                i--;
                            }
                        }
                        for (int i = 0; i < veggieRects.Count; i++)
                        {
                            if (veggieRects[i].Contains(mouseState.Position))
                            {
                                score += 50;
                                veggieRects.RemoveAt(i);
                                veggieTextures.RemoveAt(i);
                                i--;
                            }
                        }
                        for (int i = 0; i < bombRects.Count; i++)
                        {
                            if (bombRects[i].Contains(mouseState.Position))
                            {
                                score -= 250;
                                bombRects.RemoveAt(i);
                                i--;
                            }
                        }
                        for (int i = 0; i < gappleRects.Count; i++)
                        {
                            if (gappleRects[i].Contains(mouseState.Position))
                            {
                                gappleClicked = true;
                                score += 500;
                                fruitRespawn -= 0.25f;
                                fruitMax += 5;
                                veggieRespawn -= 0.25f;
                                veggieMax += 5;
                                gappleRespawn += 0.5f;
                                bombRespawn = 0.25f;
                                bombMax += 15;
                                gappleRects.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    else if (keyboardState.IsKeyDown(Keys.LeftAlt) && prevKeyboardState.IsKeyUp(Keys.LeftAlt))
                    {
                        if (!debugMode)
                        {
                            debugMode = true;
                            debugRound = 1;
                        }
                        else if (debugMode)
                        {
                            if (debugRound == 1)
                                debugRound = 2;
                            else if (debugRound == 2)
                                debugMode = false;
                        }
                    }
                        break;
                case Screen.Outro:
                    break;
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (screen == Screen.Game)
            {
                fruitTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                veggieTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                bombTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                gappleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                gameStart += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (fruitTimer > fruitRespawn)
                {
                    if (fruitRects.Count < fruitMax)
                    {
                        fruitTextures.Add(frTextures[generator.Next(frTextures.Count)]);
                        fruitRects.Add(new Rectangle(generator.Next(window.Width - (sizeMax + 16)), generator.Next(window.Height - (sizeMax + 16)), sizeMax, sizeMax));
                    }
                    fruitTimer = 0f;
                }
                if (veggieTimer > veggieRespawn)
                {
                    if (veggieRects.Count < veggieMax)
                    {
                        veggieTextures.Add(vgTextures[generator.Next(vgTextures.Count)]);
                        veggieRects.Add(new Rectangle(generator.Next(window.Width - (sizeMax + 16)), generator.Next(window.Height - (sizeMax + 16)), sizeMax, sizeMax));
                    }
                    veggieTimer = 0f;
                }
                if (bombTimer > bombRespawn)
                {
                    //bombTextures.Add(bombTexture);
                    if (bombRects.Count < bombMax)
                        bombRects.Add(new Rectangle(generator.Next(window.Width - (bombSize + 16)), generator.Next(window.Height - (bombSize + 16)), bombSize, bombSize));
                    bombTimer = 0f;
                    if (!gappleClicked)
                        bombRespawn = generator.Next(3, 8);
                    else
                        bombRespawn = 0.25f;

                }
                if (gappleTimer > gappleRespawn)
                {
                    if (gappleRects.Count < gappleMax)
                        gappleRects.Add(new Rectangle(generator.Next(window.Width - (gappleSize + 16)), generator.Next(window.Height - (gappleSize + 16)), gappleSize, gappleSize));
                    gappleTimer = 0f;
                }
                if (gameStart > gameEnd)
                {
                    screen = Screen.Outro;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            switch (screen)
            {
                case Screen.Intro:
                    _spriteBatch.Draw(introBack, window, Color.White);
                    _spriteBatch.Draw(playTexture, playBtn, Color.White);
                    break;
                case Screen.Game:
                    _spriteBatch.Draw(gameBack, window, Color.White);
                    for (int i = 0; i < fruitRects.Count; i++)
                    {
                        _spriteBatch.Draw(fruitTextures[i], fruitRects[i], Color.White);
                    }
                    for (int i = 0; i < veggieRects.Count; i++)
                    {
                        _spriteBatch.Draw(veggieTextures[i], veggieRects[i], Color.White);
                    }
                    for (int i = 0; i < bombRects.Count; i++)
                    {
                        _spriteBatch.Draw(bombTexture, bombRects[i], Color.White);
                    }
                    for (int i = 0; i < gappleRects.Count; i++)
                    {
                        _spriteBatch.Draw(gappleTexture, gappleRects[i], Color.White);
                    }
                    // keep at bottom
                    _spriteBatch.DrawString(scoreFont, $"Score: {score}", new Vector2(0, 0), Color.White);
                    if (debugMode)
                    {
                        // fruitMax
                        if (fruitRects.Count < fruitMax)
                            _spriteBatch.DrawString(debugFont, $"Fruit Respawns in {Math.Round((fruitRespawn - fruitTimer), debugRound)}s", new Vector2(0, 48), Color.Red);
                        else
                            _spriteBatch.DrawString(debugFont, $"[FRUIT MAX REACHED]", new Vector2(0, 48), Color.Red);
                        // veggieMax
                        if (veggieRects.Count < veggieMax)
                            _spriteBatch.DrawString(debugFont, $"Veggie Respawns in {Math.Round((veggieRespawn - veggieTimer), debugRound)}s", new Vector2(0, 68), Color.Red);
                        else
                            _spriteBatch.DrawString(debugFont, $"[VEGGIE MAX REACHED]", new Vector2(0, 68), Color.Red);
                        // bombMax
                        if (bombRects.Count < bombMax)
                            _spriteBatch.DrawString(debugFont, $"Bomb Respawns in {Math.Round((bombRespawn - bombTimer), debugRound)}s", new Vector2(0, 88), Color.Red);
                        else
                            _spriteBatch.DrawString(debugFont, $"[BOMB MAX REACHED]", new Vector2(0, 88), Color.Red);
                        // gappleMax
                        if (gappleRects.Count < gappleMax)
                            _spriteBatch.DrawString(debugFont, $"Gapple Respawns in {Math.Round((gappleRespawn - gappleTimer), debugRound)}s", new Vector2(0, 108), Color.Gold);
                        else
                            _spriteBatch.DrawString(debugFont, $"[GAPPLE MAX REACHED]", new Vector2(0, 108), Color.Gold);

                    }
                    _spriteBatch.DrawString(gameFont, $"{Math.Round((gameEnd - gameStart), 0)}", new Vector2(700, 0), Color.White);
                    break;
                case Screen.Outro:
                    break;
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
