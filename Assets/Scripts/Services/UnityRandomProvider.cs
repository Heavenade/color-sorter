using UnityEngine;
using ColorSorter.Abstractions;

namespace ColorSorter.Services
{
    /// <summary>
    /// 유니티 랜덤을 사용하기 위한 서비스
    /// </summary>
    public sealed class UnityRandomProvider : IRandom
    {
        public int Range(int min, int max)
        {
            return Random.Range(min, max);
        }

        public float Value => Random.value;
    }
}
