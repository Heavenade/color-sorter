namespace ColorSorter.Game
{
    public static class Judge
    {
        public static JudgeType JudgeHitOrMiss(ColorType inputColor, ColorType? targetColor)
        {
            if (targetColor == null)
            {
                return JudgeType.Miss;
            }

            return inputColor == targetColor ? JudgeType.Hit : JudgeType.Miss;
        }
    }
}
