using ColorSorter.Abstractions;

namespace ColorSorter.Services
{
    /// <summary>
    /// 테스트용 고정된 난수 반환
    /// </summary>

    public class FixedRandomProvider : IRandom
    {
        private readonly float value;

        // 0 이상 1 미만 권장. 기본값 0.5f
        public FixedRandomProvider(float value = 0.5f)
        {
            this.value = value;
        }

        public float Value => value;

        public int Range(int min, int max)
        {
            return min; // 일단 최소값으로 반환
        }

    }
}
