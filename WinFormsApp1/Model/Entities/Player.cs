using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Model.Entities;

public class Player : EnemyAndPlayer
{
    public float Energy { get; set; }
    public float LightRadius { get; set; }
    public int JumpsLeft { get; set; }
    public float VelocityY { get; set; }
    public bool IsGrounded { get; set; }

    public bool IsAlive => Energy > 0;

    public Player(float x, float y, float width, float height, float velocityX, float speed)
        : base(x, y, width, height, velocityX, speed)
    {
        IsGrounded = false;
        Energy = 100;
        LightRadius = 150;
        JumpsLeft = 1000;
    }

    public void Jump(float jumpStrength)
    {
        if (this.JumpsLeft > 0)
        {
            this.VelocityY = -jumpStrength;
            this.IsGrounded = false;
            this.JumpsLeft--;
        }
    }

    public void ApplyGravity(float gravity)
    {
        this.VelocityY += gravity;
    }

    public void UpdatePosition()
    {
        this.X += this.VelocityX;
        this.Y += this.VelocityY;
    }

    public void UpdateEnergy(float EnergyDrain)
    {
        this.Energy -= EnergyDrain;
        if (this.Energy < 0)
            this.Energy = 0;
    }

    public void UpdateLightRadius(float baseRadius, float extraRadius)
    {
        this.LightRadius = baseRadius + (extraRadius * (this.Energy / 100));
    }

    public void AddEnergy(float amount)
    {
        this.Energy += amount;
        if (this.Energy > 100)
            this.Energy = 100;
    }

    public void TakeDamage(float damage)
    {
        this.Energy -= damage;
        if (this.Energy < 0)
            this.Energy = 0;
    }

    public void SetGrounded(bool isGrounded)
    {
        this.IsGrounded = isGrounded;
        if (isGrounded)
            this.JumpsLeft = 1;
    }
}