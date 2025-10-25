using UnityEngine;

namespace ColorSorter.GameSystem
{
    public sealed class BlockSpawner
    {
        public float BlueWeight { get; }
        public float RedWeight { get; }

        public BlockSpawner(float blueWeight, float redWeight)
        {
            BlueWeight = blueWeight;
            RedWeight = redWeight;
        }

        public ColorType SpawnBlock()
        {
            float totalWeight = BlueWeight + RedWeight;
            float randomValue = Random.Range(0f, totalWeight);

            if (randomValue < BlueWeight)
            {
                return ColorType.Blue;
            }
            else
            {
                return ColorType.Red;
            }
        }
    }
}
