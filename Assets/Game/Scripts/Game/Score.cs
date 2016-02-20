using UnityEngine;

static public class Score
{
    private static int value = 0;
    private static int max = 1000;

    public static void AddScore(int x)
    {
        value = Mathf.Min(value + x, max);
    }

    public static int GetScore()
    {
        return value;
    }

    public static float GetRatio()
    {
        return 1f * value / max;
    }
}
