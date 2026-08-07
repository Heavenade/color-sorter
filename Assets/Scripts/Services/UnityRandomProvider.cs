using UnityEngine;
using ColorSorter.Abstractions;

namespace ColorSorter.Services
{
    // 유니티 랜덤을 사용하기 위한 난수 서비스
    public sealed class UnityRandomProvider : IRandom
    {
        public int Range(int min, int max)
        {
            return Random.Range(min, max);
        }

        public float Value => Random.value;
    }
}
