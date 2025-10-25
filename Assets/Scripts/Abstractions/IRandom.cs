namespace ColorSorter.Abstractions
{
    /// <summary>
    /// 게임 로직에서 UnityEngine.Random을 직접 사용하지 않기 위한 인터페이스
    /// </summary>
    public interface IRandom
    {
        /// <summary>
        /// min 이상 max 미만의 정수를 반환
        /// </summary>
        int Range(int min, int max);

        /// <summary>  
        /// 0.0 이상 1.0 미만의 소수를 반환
        /// </summary>
        float Value { get; }
    }
}
