using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.Model.Physics;

public static class Gravity
{
    private const float EarthG = 9.81f;

    private const float GameRatio = 0.07f;

    public static void UpdateGravity(Player player) 
    {
        if (!player.IsGrounded) 
            player.ApplyGravity(EarthG * GameRatio);
        else 
            player.VelocityY = 0;
    }

    public static float GetGravityValue() => EarthG * GameRatio;
}
