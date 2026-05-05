using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Model.Entities
{
    internal class Player : GameObject
    {   
        public double Energy { get; set; }
        public double LightRadius { get; set; }
        public int JumpsLeft { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public bool IsGrounded { get; set; }
        public Player(double x, double y, double width, double height) : base(x, y, width, height)
        {
            this.VelocityX = 0;
            this.VelocityY = 0;
            this.IsGrounded = false;
            this.Energy = 100;
            this.LightRadius = 150;
            this.JumpsLeft = 1;
        }

        public void MoveLeft(double speed)
        {
            this.VelocityX = -speed;
        }

        public void MoveRight(double speed) 
        { 
            this.VelocityX = speed; 
        }

        public void StopMove()
        { 
            this.VelocityX = 0;
        }

        public void Jump(double jumpStrength)
        {
            if (this.JumpsLeft > 0)
            {
                this.VelocityY = -jumpStrength;
                this.IsGrounded = false;
                this.JumpsLeft--;
            }
        }

        public void ApplyGravity(double gravity)
        {
            this.VelocityY += gravity;
        }

        public void UpdatePosition()
        {
            this.X += this.VelocityX;
            this.Y += this.VelocityY;
        }

        public void UpdateEnergy(double EnergyDrain)
        {   
            this.Energy -= EnergyDrain;
            if (this.Energy < 0)
                this.Energy = 0;
        }

        public void UpdateLightRadius(double baseRadius, double extraRadius) 
        { 
            this.LightRadius = baseRadius + (extraRadius * (this.Energy / 100));
        }

        public void AddEnergy(double amount)
        {
            this.Energy += amount;
            if (this.Energy > 100)
                this.Energy = 100;
        }

        public void TakeDamage(double damage)
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
}
