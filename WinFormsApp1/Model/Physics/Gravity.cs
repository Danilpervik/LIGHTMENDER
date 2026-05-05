using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.Model.Physics
{
    internal static class Gravity
    {
        private const double EarthG = 9.81;

        private const double GameRatio = 0.07;

        public static void UpdateGravity(Player player) 
        {
            if (!player.IsGrounded) 
                player.ApplyGravity(EarthG * GameRatio);
            else 
                player.VelocityY = 0;
        }

        public static double GetGravityValue() => EarthG * GameRatio;
    }
}
