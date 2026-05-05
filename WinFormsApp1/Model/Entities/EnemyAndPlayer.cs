using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Model.Entities
{
    internal abstract class EnemyAndPlayer : GameObject
    {
        public double VelocityX { get; set; }
        public double Speed { get; set; }
        public EnemyAndPlayer(double x, double y, double width, double height, double velocityX, double speed) : base(x, y, width, height)
        {
            VelocityX = velocityX;
            Speed = speed;
        }

        public void MoveLeft()
        {
            VelocityX = -Speed;
        }

        public void MoveRight()
        {
            VelocityX = Speed;
        }

        public void StopMove()
        {
            VelocityX = 0;
        }
    }
}
