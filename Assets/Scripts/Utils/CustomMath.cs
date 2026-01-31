public static class CustomMath
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static int GreatesCommonDivisor(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public static (int width, int height) GetAspectRatio(int w, int h)
    {
        int gcd = GreatesCommonDivisor(w, h);
        return (w / gcd, h / gcd);
    }
}