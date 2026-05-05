using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Model.Entities
{
    internal class Platform : GameObject
    {
        public enum PlatformType { Normal, Slippery }

        public PlatformType Type { get; set; }

        public Platform(double x, double y, double width, double height, PlatformType type) : base(x, y, width, height)
        { 
            this.Type = type;
        }
    }
}
