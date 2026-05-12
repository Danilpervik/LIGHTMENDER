using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Model.Entities;

public class Platform : GameObject
{
    public enum PlatformType { Normal, Slippery }

    public PlatformType Type { get; set; }

    public Platform(float x, float y, float width, float height, PlatformType type) : base(x, y, width, height)
    { 
        this.Type = type;
    }
}
