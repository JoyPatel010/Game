using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garden_Collector
{
    public class Collections
    {
        public Vector2 Position;
        public String Type;
        public Texture2D Text;

        public Collections(Vector2 position,String type,Texture2D text)
        {
            Position = position;
            Type = type;
            Text = text;
        }
    }
}
