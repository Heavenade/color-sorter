namespace ColorSorter.Abstractions
{
    // 게임 로직에서 UnityEngine.Random을 직접 사용하지 않기 위한 인터페이스
    public interface IRandom
    {
        // min 이상 max 미만의 정수를 반환
        int Range(int min, int max);

        // 0.0 이상 1.0 미만의 난수를 반환
        float Value { get; }
    }
}
