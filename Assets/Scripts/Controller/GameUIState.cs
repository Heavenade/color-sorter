using System.Collections.Generic;
using ColorSorter.Game;

namespace ColorSorter.Controller.ViewData
{
    public sealed class GameUIState
    {
        public GameState State { get; set; }
        public float TimeRemainingSec { get; set; }
        public int Score { get; set; }
        public int MissCount { get; set; }
        public int BestScore { get; set; }

        // 레인에 표시될 블록 리스트 (앞이 index 0)
        public List<ColorType> VisibleQueue { get; set; }

        // 맨 앞 블록 강조 여부
        public bool HighlightFront { get; set; }
    }
}