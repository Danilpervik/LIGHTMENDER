using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Model.Entities;

public class LightSwitch : GameObject
{
    public bool IsActivated { get; private set; }
    public float ActivationRadius { get; set; }

    public LightSwitch(float x, float y) : base(x, y, 30, 30)
    {
        IsActivated = false;
        ActivationRadius = 40;
    }

    public LightSwitch(float x, float y, float activationRadius) : base(x, y, 30, 30)
    {
        IsActivated = false;
        ActivationRadius = activationRadius;
    }

    public LightSwitch(float x, float y, float width, float height, float activationRadius) : base(x, y, width, height)
    {
        IsActivated = false;
        ActivationRadius = activationRadius;
    }

    public bool Activate()
    {
        if (!IsActivated)
        {
            IsActivated = true;
            return true;
        }
        return false;
    }

    public bool IsPlayerInRange(float playerX, float playerY)
    {
        float centerX = X + Width / 2;
        float centerY = Y + Height / 2;

        float dx = centerX - playerX;
        float dy = centerY - playerY;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        return distance <= ActivationRadius;
    }

    public bool IsPlayerInRange(Vector2 playerPosition)
    {
        return IsPlayerInRange(playerPosition.X, playerPosition.Y);
    }

    public void Reset()
    {
        IsActivated = false;
    }
}