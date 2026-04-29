using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Model.Entities
{
    internal class EnergyOrb
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float EnergyAmount { get; set; }
        public bool IsCollected { get; private set; }

        public EnergyOrb(float x, float y)
        {
            X = x;
            Y = y;
            Width = 15;
            Height = 15;
            EnergyAmount = 10;
            IsCollected = false;
        }

        public EnergyOrb(float x, float y, float energyAmount)
        {
            X = x;
            Y = y;
            Width = 15;
            Height = 15;
            EnergyAmount = energyAmount;
            IsCollected = false;
        }

        public EnergyOrb(float x, float y, float width, float height, float energyAmount)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
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