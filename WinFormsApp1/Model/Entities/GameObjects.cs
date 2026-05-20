namespace WinFormsApp2.Model.Entities;

public abstract class GameObject
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public GameObject(float x, float y, float width, float height)
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
