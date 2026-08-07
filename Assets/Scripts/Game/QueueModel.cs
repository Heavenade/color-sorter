using System;
using System.Collections.Generic;

namespace ColorSorter.Game
{
    public sealed class QueueModel
    {
        private readonly Queue<ColorType> queue = new Queue<ColorType>();

        public int VisibleCount { get; }
        public int Count => queue.Count;

        public QueueModel(int visibleCount)
        {
            if (visibleCount < 0)
                throw new ArgumentOutOfRangeException(nameof(visibleCount), "visibleCount must be zero or greater.");

            this.VisibleCount = visibleCount;
        }

        public void Init(IEnumerable<ColorType> initialColors)
        {
            if (initialColors == null)
                throw new ArgumentNullException(nameof(initialColors));

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

        // 맨 앞 아이템 제거. 성공 시 true 반환
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

        // 앞에서부터 visibleCount 개의 아이템을 리스트로 반환
        public List<ColorType> GetVisibles()
        {
            // visibleCount 개만 사용 (큐가 더 작다면 큐 카운트를 사용)
            var visibles = new List<ColorType>(Math.Min(VisibleCount, queue.Count));
            int count = 0;
            foreach (var color in queue)
            {
                if (count >= VisibleCount)
                    break;

                visibles.Add(color);
                count++;
            }
            return visibles;
        }
    }
}
