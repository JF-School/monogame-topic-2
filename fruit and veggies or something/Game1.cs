using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace fruit_and_veggies_or_something
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Random generator;

        Rectangle window;
        List<Rectangle> fruitRects;
        List<Rectangle> veggieRects;
        List<Rectangle> bombRects;
        Texture2D bombTexture, explosionTexture, backTexture;
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
            // TODO: Add your initialization logic here

            window = new Rectangle(0, 0, 800, 500);
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;

            generator = new Random();

            fruitRects = new List<Rectangle>();
            for (int i = 0; i < 30; i++)
            {
                fruitRects.Add(new Rectangle(generator.Next(window.Width - 32), generator.Next(window.Height - 32), 32, 32));
            }
            veggieRects = new List<Rectangle>();
            for (int i = 0; i < 30; i++)
            {
                veggieRects.Add(new Rectangle(generator.Next(window.Width - 32), generator.Next(window.Height - 32), 32, 32));
            }
            bombRect = new Rectangle(generator.Next(window.Width - 32), generator.Next(window.Height - 32), 32, 32);

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

            bombTexture = Content.Load<Texture2D>("Images/dynamite");
            explosionTexture = Content.Load<Texture2D>("Images/explosion");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
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
                _spriteBatch.Draw(bombTexture, bombRect, Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
