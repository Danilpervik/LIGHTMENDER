namespace WinFormsApp1.Model.Entities
{

    public interface IActiveGameObject
    { 
        public float VelocityX { get; set; }
        public float Speed { get; set; }

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
