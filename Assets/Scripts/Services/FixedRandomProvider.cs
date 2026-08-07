using ColorSorter.Abstractions;

namespace ColorSorter.Services
{

    // 테스트용 고정된 난수 반환기
    public sealed class FixedRandomProvider : IRandom
    {
        private readonly float value;

        // 테스트에서 사용할 고정 난수. 기본값 0.5f
        public FixedRandomProvider(float value = 0.5f)
        {
            this.value = value;
        }

        public float Value => value;

        public int Range(int min, int max)
        {
            return min; // 일단 항상 동일한 값 - 최소값으로 반환
        }

    }
}
