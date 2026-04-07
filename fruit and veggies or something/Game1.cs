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
        int rawScore, score; // total score collected
        int fruitCount, veggieCount, gappleCount, bombCount;
        bool debugMode, gappleClicked, howPlay; // boolean variables
        int debugRound;
        int fruitMax, veggieMax, bombMax, gappleMax; // the max amount of rectangles that can exist
        float fruitTimer, veggieTimer, bombTimer, gappleTimer; // the timers for the rectangles
        float fruitRespawn, veggieRespawn, bombRespawn, gappleRespawn; // the respawn times
        float gameStart, gameEnd; // the timer for the game

        MouseState prevMouseState, mouseState;
        KeyboardState prevKeyboardState, keyboardState;
        Screen screen;
        SpriteFont scoreFont, debugFont, gameFont, statsFont;

        Rectangle window;
        List<Rectangle> fruitRects;
        List<Rectangle> veggieRects;
        List<Rectangle> bombRects;
        List<Rectangle> gappleRects;
        Rectangle playBtn, howBtn, creditsBtn, exitBtn, backBtn;
        Texture2D bombTexture, playTexture, howBtnTexture, howPlayTexture, creditsBtnTexture, 
            gappleTexture, statisticsTexture, exitBtnTexture, backBtnTexture;
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

            fruitCount = 0;
            veggieCount = 0;
            bombCount = 0;
            gappleCount = 0;

            debugMode = false;
            gappleClicked = false;
            howPlay = false;
            debugRound = 1;

            fruitRects = new List<Rectangle>();
            for (int i = 0; i < 25; i++)
            {
                fruitRects.Add(new Rectangle(generator.Next(window.Width - (sizeMax + 16)), generator.Next(window.Height - (sizeMax + 16)), sizeMax, sizeMax));
            }
            veggieRects = new List<Rectangle>();
            for (int i = 0; i < 25; i++)
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
            howBtn = new Rectangle(240, 200, 100, 100);
            backBtn = new Rectangle(10, 10, 75, 75);
            exitBtn = new Rectangle(720, 5, 75, 75);
            creditsBtn = new Rectangle(460, 200, 100, 100);

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
            howBtnTexture = Content.Load<Texture2D>("Images/howbutton");
            howPlayTexture = Content.Load<Texture2D>("Images/howtoplay");
            backBtnTexture = Content.Load<Texture2D>("Images/backbutton");
            exitBtnTexture = Content.Load<Texture2D>("Images/leavebutton");
            statisticsTexture = Content.Load<Texture2D>("Images/statistics");
            creditsBtnTexture = Content.Load<Texture2D>("Images/creditsbutton");

            bombTexture = Content.Load<Texture2D>("Images/dynamite");
            gappleTexture = Content.Load<Texture2D>("Images/goldenapple");

            introBack = Content.Load<Texture2D>("Images/introbackground");
            gameBack = Content.Load<Texture2D>("Images/fruitbackground");

            scoreFont = Content.Load<SpriteFont>("Fonts/ScoreFont");
            debugFont = Content.Load<SpriteFont>("Fonts/DebugFont");
            gameFont = Content.Load<SpriteFont>("Fonts/GameFont");
            statsFont = Content.Load<SpriteFont>("Fonts/StatsFont");


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

            //this.Window.Title = $"x = {mouseState.X}, y = {mouseState.Y}";

            // mouse states
            switch (screen)
            {
                case Screen.Intro:
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (playBtn.Contains(mouseState.Position))
                            screen = Screen.Game;
                        if (howBtn.Contains(mouseState.Position))
                            howPlay = true;
                        if (backBtn.Contains(mouseState.Position) && howPlay)
                            howPlay = false;
                    }
                        break;
                case Screen.Game:
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        for (int i = 0; i < fruitRects.Count; i++)
                        {
                            if (fruitRects[i].Contains(mouseState.Position))
                            {
                                fruitCount++;
                                rawScore += 100;
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
                                veggieCount++;
                                rawScore += 50;
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
                                bombCount++;
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
                                gappleCount++;
                                rawScore += 750;
                                score += 750;
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
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (exitBtn.Contains(mouseState.Position))
                            Exit();
                    }
                    break;
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            switch (screen)
            {
                case Screen.Intro:
                    break;
                case Screen.Game:
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
                    break;
                case Screen.Outro:
                    break;
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
                    if (!howPlay)
                    {
                        _spriteBatch.Draw(introBack, window, Color.White);
                        _spriteBatch.Draw(playTexture, playBtn, Color.White);
                        _spriteBatch.Draw(howBtnTexture, howBtn, Color.White);
                        _spriteBatch.Draw(creditsBtnTexture, creditsBtn, Color.White);
                    }
                    else if (howPlay)
                    {
                        _spriteBatch.Draw(howPlayTexture, window, Color.White);
                        _spriteBatch.Draw(backBtnTexture, backBtn, Color.White);
                    }
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
                    _spriteBatch.Draw(statisticsTexture, window, Color.White);
                    _spriteBatch.Draw(exitBtnTexture, exitBtn, Color.White);
                    _spriteBatch.DrawString(statsFont, $"{gappleCount}", new Vector2(140, 100), Color.Black);
                    _spriteBatch.DrawString(statsFont, $"{fruitCount}", new Vector2(140, 200), Color.Black);
                    _spriteBatch.DrawString(statsFont, $"{veggieCount}", new Vector2(140, 300), Color.Black);
                    _spriteBatch.DrawString(statsFont, $"{bombCount}", new Vector2(140, 400), Color.Black);
                    _spriteBatch.DrawString(statsFont, $"{rawScore}", new Vector2(550, 160), Color.Black);
                    _spriteBatch.DrawString(statsFont, $"{score}", new Vector2(550, 360), Color.Black);
                    break;
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
