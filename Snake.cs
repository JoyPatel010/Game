using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Garden_Collector
{
    public class Snake 
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

        public Snake(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }
    }
}
