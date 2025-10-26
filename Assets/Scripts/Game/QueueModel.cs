using System;
using System.Collections.Generic;

namespace ColorSorter.Game
{
    public class QueueModel
    {
        private readonly Queue<ColorType> queue = new Queue<ColorType>();

        public int VisibleCount { get; }
        public int Count => queue.Count;

        public QueueModel(int visibleCount)
        {
            if (visibleCount < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(visibleCount),
                    "visibleCount는 0 이상이어야.");

            this.VisibleCount = visibleCount;
        }

        public void Init(List<ColorType> initialColors)
        {
            queue.Clear();
            foreach (var color in initialColors)
            {
                EnqueueBack(color);
            }
        }

        public void EnqueueBack(ColorType color)
        {
            queue.Enqueue(color);
        }

        /// <summary>
        /// 맨 앞 아이템 제거. 성공 시 true 반환.
        /// </summary>
        public bool DequeueFront(out ColorType color)
        {
            if (queue.Count > 0)
            {
                color = queue.Dequeue();
                return true;
            }
            else
            {
                color = default;
                return false;
            }
        }

        public ColorType? PeekFront()
        {
            if (queue.Count == 0)
                return null;
            return queue.Peek();
        }

        public void Clear()
        {
            queue.Clear();
        }

        /// <summary>
        /// 앞에서부터 visibleCount 개의 아이템을 리스트로 반환.
        /// </summary>
        public List<ColorType> GetVisibles()
        {
            var all = queue.ToArray(); // 큐 전체를 배열로
            int n = Math.Min(VisibleCount, all.Length); // 앞 N개만 사용
            var visibles = new ColorType[n];
            Array.Copy(all, 0, visibles, 0, n);
            return new List<ColorType>(visibles);
        }
    }
}
