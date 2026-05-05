using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2.Model.Entities
{
    public abstract class GameObject
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public GameObject(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }
    }
}
