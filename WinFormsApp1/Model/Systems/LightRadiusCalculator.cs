using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.Model.Systems
{
    public class LightRadiusCalculator
    {
        public float BaseRadius { get; set; } = 50f;
        public float EnergyMultiplier { get; set; } = 0.5f;

        public LightRadiusCalculator()
        {
        }

        public LightRadiusCalculator(float baseRadius, float energyMultiplier)
        {
            BaseRadius = baseRadius;
            EnergyMultiplier = energyMultiplier;
        }

        public float Calculate(float energy)
        {
            return BaseRadius + (energy * EnergyMultiplier);
        }

        public void UpdatePlayerRadius(Player player)
        {
            if (player == null) return;
            player.LightRadius = Calculate(player.Energy);
        }
    }
}