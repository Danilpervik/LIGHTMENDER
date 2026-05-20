namespace WinFormsApp1.Model.Entities;

public class Enemy : EnemyAndPlayer
{
    public bool IsVisible { get; set; }
    public bool IsActive
    {
        get { return IsVisible; }
    }

    public Enemy(float x, float y, float width, float height, float velocityX, float speed) : base(x, y, width, height, velocityX, speed)
    {
        IsVisible = false;
    }

    public void UpdatePosition()
    {
        this.X += this.VelocityX;
    }
}