using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.Model.World
{
    internal class Level
    {   
        public int Width { get; set; }
        public int Height { get; set; }

        public List<Platform> Platforms { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<EnergyOrb> EnergyOrbs { get; set; }
        public List<LightSwitch> LightSwitches { get; set; }

        public Level()
        {
            Platforms = new List<Platform>();
            Enemies = new List<Enemy>();
            EnergyOrbs = new List<EnergyOrb>();
            LightSwitches = new List<LightSwitch>();
        }

        public void AddPlatform(Platform platform)
        {
            Platforms.Add(platform);
        }

        public void AddEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
        }

        public void AddEnergyOrb(EnergyOrb energyOrb)
        {
            EnergyOrbs.Add(energyOrb);
        }

        public void AddLightSwitch(LightSwitch lightSwitch)
        {
            LightSwitches.Add(lightSwitch);
        }

        public void RemovePlatform(Platform platform)
        {
            Platforms.Remove(platform);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            Enemies.Remove(enemy);
        }

        public void RemoveEnergyOrb(EnergyOrb energyOrb)
        {
            EnergyOrbs.Remove(energyOrb);
        }

        public void RemoveLightSwitch(LightSwitch lightSwitch)
        {
            LightSwitches.Remove(lightSwitch);
        }
    }
}
