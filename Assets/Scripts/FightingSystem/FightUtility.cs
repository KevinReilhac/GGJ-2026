using UnityEngine;

public static class FightUtility
{
    public static int MagicRound(float value)
    {
        int intValue = Mathf.FloorToInt(value);
        float rest = value - intValue;

        if (Random.Range(0f, 1f) < rest)
            intValue++;
        return intValue;
    }
}