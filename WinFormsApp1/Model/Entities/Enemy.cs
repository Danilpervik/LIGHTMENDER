using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Model.Entities
{
    internal class Enemy : GameObject
    {
        public double VelocityX { get; set; }
        public bool IsVisible { get; set; }
        public double Speed { get; set; } = 2;
        public bool IsActive
        {
            get { return IsVisible; }
        }

        public Enemy(double x, double y, double width, double height) : base(x, y, width, height)
        {
            VelocityX = 0;
            IsVisible = false;
        }

        public double CalculateDistanceToPlayer(Player player, Point point)
        {
            var deltaX = player.X - point.X;
            var deltaY = player.Y - point.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public void SeekPlayer(Player player)
        {
            if (IsVisible)
            {
                if (player.X < this.X)
                    MoveLeft();
                else if (player.X > this.X)
                    MoveRight();
            }
        }

        public void SetVisible(Player player)
        {
            var listOfCorners = new List<Point>
            {
                new Point((int)this.X, (int)this.Y),
                new Point((int)this.X + (int)this.Width, (int)this.Y),
                new Point((int)this.X, (int)this.Y + (int)this.Height),
                new Point((int)this.X + (int)this.Width, (int)this.Y + (int)this.Height)
            };
            this.IsVisible = listOfCorners.Any(corner => CalculateDistanceToPlayer(player, corner) < player.LightRadius);
        }

        public void MoveLeft()
        {
            this.VelocityX = -Speed;
        }

        public void MoveRight()
        {
            this.VelocityX = Speed;
        }

        public void StopMove()
        {
            this.VelocityX = 0;
        }

        public void UpdatePosition()
        {
            this.X += this.VelocityX;
        }
    }
}