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
        int sizeMax, score;
        float seconds, respawnTime;

        MouseState prevMouseState, mouseState;
        Screen screen;

        Rectangle window;
        List<Rectangle> fruitRects;
        List<Rectangle> veggieRects;
        List<Rectangle> bombRects;
        Rectangle playBtn, howBtn, creditsBtn;
        Texture2D bombTexture, explosionTexture, playTexture, howTexture, creditsTexture;
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

            seconds = 0f;
            respawnTime = 5f;
            generator = new Random();
            sizeMax = 48;

            fruitRects = new List<Rectangle>();
            for (int i = 0; i < 1; i++)
            {
                fruitRects.Add(new Rectangle(generator.Next(window.Width - sizeMax), generator.Next(window.Height - sizeMax), sizeMax, sizeMax));
            }
            veggieRects = new List<Rectangle>();
            for (int i = 0; i < 30; i++)
            {
                veggieRects.Add(new Rectangle(generator.Next(window.Width - sizeMax), generator.Next(window.Height - sizeMax), sizeMax, sizeMax));
            }
            bombRects = new List<Rectangle>();
            for (int i = 0; i < 10; i++)
            {
                bombRects.Add(new Rectangle(generator.Next(window.Width - sizeMax), generator.Next(window.Height - sizeMax), sizeMax, sizeMax));
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
            introBack = Content.Load<Texture2D>("Images/introbackground");
            gameBack = Content.Load<Texture2D>("Images/fruitbackground");



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
                                fruitRects.RemoveAt(i);
                                fruitTextures.RemoveAt(i);
                                i--;
                            }
                        }
                        for (int i = 0; i < veggieRects.Count; i++)
                        {
                            if (veggieRects[i].Contains(mouseState.Position))
                            {
                                veggieRects.RemoveAt(i);
                                veggieTextures.RemoveAt(i);
                                i--;
                            }
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
                seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (seconds > respawnTime)
                {
                    fruitTextures.Add(frTextures[generator.Next(frTextures.Count)]);
                    fruitRects.Add(new Rectangle(generator.Next(window.Width - sizeMax), generator.Next(window.Height - sizeMax), sizeMax, sizeMax));
                    seconds = 0f;
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
                    for (int i = 0; i < 10; i++)
                    {
                        _spriteBatch.Draw(bombTexture, bombRects[i], Color.White);
                    }
                    break;
                case Screen.Outro:
                    break;
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
