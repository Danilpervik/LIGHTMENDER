using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Model.Entities
{
    internal abstract class EnemyAndPlayer : GameObject
    {
        public float VelocityX { get; set; }
        public float Speed { get; set; }
        public EnemyAndPlayer(float x, float y, float width, float height, float velocityX, float speed) : base(x, y, width, height)
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
