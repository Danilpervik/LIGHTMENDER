using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.Model.Systems
{
    public class EnergySystem
    {
        public float DrainRate { get; set; } = 0.5f;

        public EnergySystem()
        {
        }

        public EnergySystem(float drainRate)
        {
            DrainRate = drainRate;
        }

        public void Update(Player player, float deltaTime)
        {
            if (player == null) return;
            if (player.Energy <= 0) return;

            player.Energy -= DrainRate * deltaTime;

            if (player.Energy < 0) player.Energy = 0;
        }

        public bool IsPlayerDead(Player player)
        {
            if (player == null) return true;
            return player.Energy <= 0;
        }

        public void AddEnergy(Player player, float amount)
        {
            if (player == null) return;
            player.Energy += amount;
            if (player.Energy > 100) player.Energy = 100;
        }
    }
}