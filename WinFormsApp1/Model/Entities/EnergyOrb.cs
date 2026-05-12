using System.Drawing;

namespace WinFormsApp1.Model.Entities
{
    public class EnergyOrb : GameObject
    {
        public float EnergyAmount { get; set; }
        public bool IsCollected { get; private set; }

        public EnergyOrb(float x, float y) : base(x, y, 15, 15)
        {
            EnergyAmount = 10;
            IsCollected = false;
        }

        public EnergyOrb(float x, float y, float energyAmount) : base(x, y, 15, 15)
        {
            EnergyAmount = energyAmount;
            IsCollected = false;
        }

        public EnergyOrb(float x, float y, float width, float height, float energyAmount) : base(x, y, width, height)
        {
            EnergyAmount = energyAmount;
            IsCollected = false;
        }

        public float Collect()
        {
            if (!IsCollected)
            {
                IsCollected = true;
                return EnergyAmount;
            }
            return 0;
        }

        public bool CanCollect()
        {
            return !IsCollected;
        }

        public void Reset()
        {
            IsCollected = false;
        }
    }
}