namespace ColorSorter.GameSystem
{
    public sealed class Judge
    {
        public JudgeType JudgeHitOrMiss(ColorType inputColor, ColorType targetColor)
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
