public abstract class CsvData
{
    // “Ç‚İ‚ñ‚¾csvƒf[ƒ^‚Ì“o˜^
    public abstract void SetData(string[] data);

    // ÔŒo‚ğŠp“x‚É•ÏŠ·
    public float RightAscensionToDegree(int hour, int min = 0, float sec = 0.0f)
    {
        var h = 360.0f / 24.0f;
        var m = h / 60.0f;
        var s = m / 60.0f;

        return (h * hour + m * min + s * sec) * -1.0f;
    }

    // ÔˆÜ‚ğŠp“x‚É•ÏŠ·
    public float DeclinationToDegree(int deg, int min = 0, float sec = 0.0f)
    {
        var plusMinus = 1.0f;
        if (deg < 0.0f)
        {
            plusMinus = -1.0f;
            deg *= -1;
        }
        return DeclinationToDegree(plusMinus, deg, min, sec);
    }

    // ÔˆÜ‚ğŠp“x‚É•ÏŠ·
    public float DeclinationToDegree(float plusMinus, int deg, int min = 0, float sec = 0.0f)
    {
        return (deg * plusMinus + min / 60.0f * plusMinus + sec / (60.0f * 60.0f) * plusMinus) * -1.0f;
    }
}