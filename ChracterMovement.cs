using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Garden_Collector
{
    public class ChracterMovement
    {
        public Vector2 charPosition;
        public float speed;
        public int scrHeight;
        public int scrWidth;
        public int charHeight;
        public int charWidth;
    
        
        public ChracterMovement(Vector2 charPosition, float speed)
        {
            this.charPosition = charPosition;
            this.speed = speed;
        }

        public void setBound(int scrHeight,int scrWidth, int charHeight, int charWidth)
        {
            this.scrWidth = scrWidth;
            this.scrHeight = scrHeight;
            this.charHeight = charHeight;
            this.charWidth = charWidth;
        }

        public void Update(KeyboardState keyboardState)
        {
            speed = 4f;
            if(keyboardState.IsKeyDown(Keys.Left))
            {
                charPosition.X -= speed;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                charPosition.Y -= speed;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                charPosition.X += speed;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                charPosition.Y += speed;
            }

            charPosition.X = MathHelper.Clamp(charPosition.X, 0, scrWidth - charWidth);
            charPosition.Y = MathHelper.Clamp(charPosition.Y, 0, scrHeight - charHeight);

           
        }
    }
}
