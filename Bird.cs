using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garden_Collector
{
    public class Bird
    {
        public Texture2D BirdTexture;
        public float ScrollSpeed;
        public Vector2 Position;
        private float screenWidth;
        private float screenHeight;
        public float ScaleFactor;

        // Constructor to initialize bird texture, speed, position, and scale factor
        public Bird(Texture2D birdTexture, float scrollSpeed, Vector2 initialPosition, float scaleFactor)
        {
            BirdTexture = birdTexture;
            ScrollSpeed = scrollSpeed;
            Position = initialPosition;
            ScaleFactor = scaleFactor;
        }

        // Method to update the bird's position
        public void Update(float deltaTime)
        {
            // Move the bird from right to left
            Position.X -= ScrollSpeed * deltaTime;

            // Boundary check: If the bird goes beyond the left side of the screen, reset its position
            if (Position.X + BirdTexture.Width < 0)
            {
                Position.X = screenWidth; // Reset bird position to the right side of the screen
                Position.Y = 0f; // Optional: Reset bird to the top (you can modify this as needed)
            }
        }

        // Method to set the screen width (for boundary check)
        public void SetScreenWidth(float width,float height)
        {
            this.screenWidth = width;
            this.screenHeight = height;
        }

        // Method to draw the bird on the screen
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BirdTexture, Position, null, Color.White, 0f, Vector2.Zero, ScaleFactor, SpriteEffects.None, 0f);
        }
    }
}
