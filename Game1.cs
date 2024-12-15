using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace Garden_Collector
{
    public enum GameState
    {
        Menu,
        Playing,
        Instruction,
        Contact,
        Quit
    }

    public class Game1 : Game
    {
        private ChracterMovement characteMovement;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D character;
        private Texture2D introPage;
        private Texture2D backImg;
        private Texture2D contactText;
        private Song backGroundSong;
        private SoundEffect collectionSound;
        private Texture2D menuBackImg;

        private Bird bird1;
        private Bird bird2;
        private Texture2D bird1_text;
        private Texture2D bird2_text;

        private int fruitScore = 0;
        private int woodScore = 0;
        private int leafScore = 0;

        //Variables for Menu
        private SpriteFont menuFont;
        private int selectedIndex = 0;
        private string[] menuItem = { "Start Game", "Instruction", "Contact", "Quit" };
        private GameState currentGameState = GameState.Menu;

        private bool keyReleased = true; // Flag to prevent multiple key press actions


        private List<Collections> Collections;
        private Texture2D fruitText;
        private Texture2D woodText;
        private Texture2D leafText;
        private Random random;

        private Texture2D obstacleText;
        private int playerHP = 100;
        private bool isGameOver = false;
        private String gameOverMsg = "";
        private List<Snake> snakeList;
        private Snake snake;
        private Vector2 obstaclePosition;
        private SoundEffect snakeBite;

        private SpriteFont gameOverFont;

        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1500;
            _graphics.PreferredBackBufferHeight = 880;
            _graphics.ApplyChanges();
            characteMovement = new ChracterMovement(new Vector2(100, 100), 0.5f);
            base.Initialize();
            Collections = new List<Collections>();
            random = new Random();

            int scrHeight = _graphics.PreferredBackBufferHeight;
            int scrWidth = _graphics.PreferredBackBufferWidth;
            int charWidth = (int)(character.Width * 0.3f);
            int charHeight = (int)(character.Height * 0.3f);

            characteMovement.setBound(scrHeight, scrWidth, charHeight, charWidth);

            snakeList = new List<Snake>();
            for (int i = 1; i <= 10; i++)
            {
                obstaclePosition = new Vector2(random.Next(0, _graphics.PreferredBackBufferWidth - 50), random.Next(0, _graphics.PreferredBackBufferHeight - 50));
                snakeList.Add(new Snake(obstacleText, obstaclePosition));
            }

            //Take 15 items for collecting
            int totalCollectionsItems = 50;

            //Collections of items in form of Array
            string[] items = { "apple", "leaf", "wood" };

            for (int i = 0; i < totalCollectionsItems; i++)
            {
                // Put items on Random Position
                Vector2 randomPositionOfItems = new Vector2(
                    random.Next(0, _graphics.PreferredBackBufferWidth - 50),
                    random.Next(0, _graphics.PreferredBackBufferHeight - 50));

                // Choose Random item
                string type = items[random.Next(items.Length)];

                // Choose the correct texture based on the item type
                Texture2D itemTexture = null;

                switch (type)
                {
                    case "apple":
                        itemTexture = fruitText;
                        break;
                    case "leaf":
                        itemTexture = leafText;
                        break;
                    case "wood":
                        itemTexture = woodText;
                        break;
                }

                // Add collection items into List
                Collections.Add(new Collections(randomPositionOfItems, type, itemTexture));
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            character = Content.Load<Texture2D>("Image/char");
            menuFont = Content.Load<SpriteFont>("Font/menuFont");
            introPage = Content.Load<Texture2D>("Image/intro");
            backImg = Content.Load<Texture2D>("Image/backImg");
            fruitText = Content.Load<Texture2D>("Image/apple");
            woodText = Content.Load<Texture2D>("Image/woodnew");
            leafText = Content.Load<Texture2D>("Image/leaf");
            contactText = Content.Load<Texture2D>("Image/contactPage");
            backGroundSong = Content.Load<Song>("Sound/backSong");
            collectionSound = Content.Load<SoundEffect>("Sound/collectionSound");
            bird1_text = Content.Load<Texture2D>("Image/bird1");
            bird2_text = Content.Load<Texture2D>("Image/bird2");
            obstacleText = Content.Load<Texture2D>("Image/snake");
            gameOverFont = Content.Load<SpriteFont>("Font/gameOverFont");
            snakeBite = Content.Load<SoundEffect>("Sound/bite");
            menuBackImg = Content.Load<Texture2D>("Image/mountain");

            bird1 = new Bird(bird1_text, 200f, new Vector2(1500f, 0f), 0.5f);
            bird2 = new Bird(bird2_text, 150f, new Vector2(1500f, 0f), 0.5f);

            bird1.SetScreenWidth(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            bird2.SetScreenWidth(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            bird1.Update(deltaTime);
            bird2.Update(deltaTime);

            if (isGameOver)
            {
                return;
            }

            KeyboardState keyboardState = Keyboard.GetState();

            switch (currentGameState)
            {
                case GameState.Menu:
                    UpdateMenu(keyboardState);
                    break;
                case GameState.Playing:
                    characteMovement.Update(keyboardState);
                    checkCollision();
                    break;

                case GameState.Instruction:
                    if (keyboardState.IsKeyDown(Keys.B))   // Press B to back to menu page
                    {
                        currentGameState = GameState.Menu;
                    }
                    break;

                case GameState.Contact:
                    if (keyboardState.IsKeyDown(Keys.B))   // Press B to back to menu page
                    {
                        currentGameState = GameState.Menu;
                    }
                    break;

                case GameState.Quit:
                    Exit();
                    break;
            }


            base.Update(gameTime);
        }

        private void UpdateMenu(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Down) && keyReleased)
            {
                selectedIndex = (selectedIndex + 1) % menuItem.Length;
                keyReleased = false;
            }
            else if (keyboardState.IsKeyDown(Keys.Up) && keyReleased)
            {
                selectedIndex = (selectedIndex - 1 + menuItem.Length) % menuItem.Length;
                keyReleased = false;
            }
            else if (keyboardState.IsKeyDown(Keys.Enter) && keyReleased)
            {
                switch (selectedIndex)
                {
                    case 0:
                        currentGameState = GameState.Playing;
                        MediaPlayer.Play(backGroundSong);
                        MediaPlayer.IsRepeating = true;
                        break;
                    case 1:
                        currentGameState = GameState.Instruction;
                        break;
                    case 2:
                        currentGameState = GameState.Contact;
                        break;
                    case 3:
                        currentGameState = GameState.Quit;
                        break;
                }
                keyReleased = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Down) || keyboardState.IsKeyUp(Keys.Up) || keyboardState.IsKeyUp(Keys.Enter))
            {
                keyReleased = true;
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            switch (currentGameState)
            {
                case GameState.Menu:
                    DrawMenu();
                    break;

                case GameState.Playing:
                    _spriteBatch.Draw(backImg, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    DrawGame();
                    break;

                case GameState.Instruction:
                    DrawIntro();
                    break;

                case GameState.Contact:
                    DrawContact();
                    break;

                case GameState.Quit:
                    Exit();
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void checkCollision()
        {
            // Character's bounding rectangle
            Rectangle charBounds = new Rectangle(
                (int)characteMovement.charPosition.X,
                (int)characteMovement.charPosition.Y,
                (int)(character.Width * 0.3f),
                (int)(character.Height * 0.3f)
            );


            for (int j = snakeList.Count - 1; j >= 0; j--)
            {
                var snake = snakeList[j];

                Rectangle obstacaleBounds = new Rectangle(
               (int)snake.Position.X,
               (int)snake.Position.Y,
               (int)(obstacleText.Width * 0.3f),
               (int)(obstacleText.Height * 0.3f)
                );

                if (charBounds.Intersects(obstacaleBounds))
                {
                    playerHP -= 10;
                    snakeBite.Play();
                    snakeList.RemoveAt(j);

                    if (playerHP <= 0)
                    {
                        isGameOver = true;
                        gameOverMsg = $"Game Over!\n" +
                                      $"Your Scores:\n" +
                                      $"Apples: {fruitScore}\n" +
                                      $"Wood: {woodScore}\n" +
                                      $"Leaves: {leafScore}";
                    }

                }

            }

            // Iterate over all collectible items
            for (int i = Collections.Count - 1; i >= 0; i--)
            {
                var item = Collections[i];

                // Item's bounding rectangle
                Rectangle itemBounds = new Rectangle(
                    (int)item.Position.X,
                    (int)item.Position.Y,
                    (int)(item.Text.Width * 0.2f),
                    (int)(item.Text.Height * 0.2f)
                );

                // Check for collision
                if (charBounds.Intersects(itemBounds))
                {
                    // Increment score based on item type
                    switch (item.Type)
                    {
                        case "apple":
                            fruitScore += 5;
                            break;
                        case "wood":
                            woodScore += 7;
                            break;
                        case "leaf":
                            leafScore += 3;
                            break;
                    }

                    // Play sound and erase item
                    collectionSound.Play();
                    Collections.RemoveAt(i);

                    if (Collections.Count == 0)
                    {
                        isGameOver = true;
                        gameOverMsg = "Congratulations...You Won this Game."; 
                    }
                }
            }
        }

        public bool IsBoundryTouch(Rectangle boundsOfItem, Rectangle boundsOfChar)
        {
            bool horizontalTouch = boundsOfChar.Right >= boundsOfItem.Left && boundsOfChar.Left <= boundsOfItem.Right;

            bool verticalTouch = boundsOfChar.Bottom >= boundsOfItem.Top && boundsOfChar.Top <= boundsOfItem.Bottom;

            return horizontalTouch && verticalTouch;
        }


        public void DrawContact()
        {
            _spriteBatch.Draw(contactText, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
        }
        private void DrawMenu()
        {
            _spriteBatch.Draw(menuBackImg, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),Color.White);
            for (int i = 0; i < menuItem.Length; i++)
            {
                Color color = (i == selectedIndex) ? Color.White : Color.Gray;
                _spriteBatch.DrawString(menuFont, menuItem[i], new Vector2(200, 200 + i * 50), color);

            }
            _spriteBatch.DrawString(menuFont, "Press B to back at Menu Page...", new Vector2(0, 0), Color.Gray);
        }

        private void DrawGame()
        {
            _spriteBatch.Draw(character, characteMovement.charPosition, null, Color.White, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);

            foreach (var snake in snakeList)
            {
                _spriteBatch.Draw(obstacleText, snake.Position, null, Color.White, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
            }

            _spriteBatch.DrawString(menuFont, $"Apple Score: {fruitScore}", new Vector2(10, 10), Color.Red);
            _spriteBatch.DrawString(menuFont, $"Wood Score: {woodScore}", new Vector2(10, 40), new Color(139, 69, 19));
            _spriteBatch.DrawString(menuFont, $"Leaf Score: {leafScore}", new Vector2(10, 70), Color.White);

            if (playerHP >= 80)
            {
                _spriteBatch.DrawString(menuFont, $"HP: {playerHP}", new Vector2(GraphicsDevice.Viewport.Width - 200, 20), Color.White);
            }
            else if (playerHP >= 50)
            {
                _spriteBatch.DrawString(menuFont, $"HP: {playerHP}", new Vector2(GraphicsDevice.Viewport.Width - 200, 20), Color.BurlyWood);
            }
            else if (playerHP >= 20)
            {
                _spriteBatch.DrawString(menuFont, $"HP: {playerHP}", new Vector2(GraphicsDevice.Viewport.Width - 200, 20), Color.Yellow);
            }
            else if (playerHP <= 10)
            {
                _spriteBatch.DrawString(menuFont, $"HP: {playerHP}", new Vector2(GraphicsDevice.Viewport.Width - 200, 20), Color.Red);
            }

            foreach (var collections in Collections)
            {
                _spriteBatch.Draw(collections.Text, collections.Position, null, Color.White, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 0f);
            }

            _spriteBatch.DrawString(gameOverFont,gameOverMsg, new Vector2(300, 300), Color.Red);

            bird1.Draw(_spriteBatch);
            bird2.Draw(_spriteBatch);
        }

        private void DrawIntro()
        {
            _spriteBatch.Draw(introPage, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

        }
    }
}

//References
//Pixabay. "Forest Music." Accessed December 6, 2024. [https://pixabay.com/music/search/forest/](https://pixabay.com/music/search/forest/)
//Pixabay. (n.d.). Coin collection [Sound effects page]. Retrieved December 6, 2024, from https://pixabay.com/sound-effects/search/coin%20collection/
//Pixabay. (n.d.). Bite sound effects. Retrieved December 8, 2024, from https://pixabay.com/sound-effects/search/bite/
//Pinterest. (n.d.). Image of [insert description, e.g., traffic map, painting] [Image]. Retrieved December 10, 2024, from https://i.pinimg.com/originals/19/17/84/191784fb532fd174202e2444c4dfac03.png
//Iconduck. (n.d.). Wood emoji [Image]. Retrieved December 10, 2024, from https://static-00.iconduck.com/assets.00/wood-emoji-1520x2048-t5h5zzso.png
//ClipartBest. (n.d.). Red snake emoji [Image]. Retrieved December 10, 2024, from https://www.clipartbest.com/clipart-dcrxA4jMi
//TopPNG. (n.d.). 2D game character [Image]. Retrieved December 10, 2024, from https://toppng.com/free-image/2d-game-character-PNG-free-PNG-Images_224285
//Tenor. (n.d.). Flying bird GIF [Animated image]. Retrieved December 10, 2024, from https://tenor.com/view/flying-bird-gif-bird-flapping-wings-flying-gif-16031726
//Freepik. (n.d.). Beautiful flying birds [Image]. Retrieved December 10, 2024, from https://img.freepik.com/premium-photo/beautiful-flying-birds-image_954948-23.jpg
//Microsoft. (2024). Image from Bing Image Search. Microsoft. https://tse3.mm.bing.net/th?id=OIG2.SkBt6NAo7N8vgkWPV0yP&pid=ImgGn
//Microsoft. (2024). Image from Bing Image Search. Microsoft. https://tse2.mm.bing.net/th?id=OIG1.J1Uuav754b.04WHcRvK0&pid=ImgGn
//Pinterest. (n.d.). Image of a design featuring a pattern [Image]. Pinterest. https://i.pinimg.com/originals/d2/63/55/d26355a4484cf412669476a57a263abc.png

