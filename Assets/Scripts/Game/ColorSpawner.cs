using ColorSorter.Abstractions;

namespace ColorSorter.Game
{
    public sealed class ColorSpawner
    {
        private readonly float blueWeight;
        private readonly float redWeight;

        private readonly IRandom rng;


        // 난수 생성기는 외부에서 주입
        public ColorSpawner(float blueWeight, float redWeight, IRandom rng)
        {
            this.rng = rng ?? throw new System.ArgumentNullException(nameof(rng));

            if (blueWeight < 0f || float.IsNaN(blueWeight) || float.IsInfinity(blueWeight))
                throw new System.ArgumentOutOfRangeException(nameof(blueWeight));
            if (redWeight < 0f || float.IsNaN(redWeight) || float.IsInfinity(redWeight))
                throw new System.ArgumentOutOfRangeException(nameof(redWeight));

            var sum = blueWeight + redWeight;
            if (sum <= 0f || float.IsNaN(sum) || float.IsInfinity(sum))
                throw new System.ArgumentOutOfRangeException(nameof(sum));

            this.blueWeight = blueWeight;
            this.redWeight = redWeight;

        }

        // 2가중치 샘플링 방식 - 가중치에 비례하여 색상을 선택한다
        public ColorType SpawnColor()
        {
            float totalWeight = blueWeight + redWeight;
            float randomValue = rng.Value * totalWeight;

            return (randomValue < blueWeight) ? ColorType.Blue : ColorType.Red;

        }
    }
}
