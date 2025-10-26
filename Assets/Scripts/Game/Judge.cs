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

            if (inputColor == targetColor)
            {
                return JudgeType.Hit;
            }
            else
            {
                return JudgeType.Miss;
            }
        }
    }
}
