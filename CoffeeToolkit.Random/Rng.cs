namespace CoffeeToolkit.Random;

/// <summary>
/// Generate cryptographically secure objects.
/// Uses System.Security.Cryptography.RandomNumberGenerator.
/// </summary>
public static class Rng
{
    private static readonly RandomNumberGenerator RngProvider
           = RandomNumberGenerator.Create();

    /// <summary>
    /// Generates a random Boolean
    /// </summary>
    /// <returns></returns>
    public static bool Bool()
        => BitConverter.ToBoolean(BaseGen(1));

    /// <summary>
    /// Generates a random UInt16
    /// </summary>
    /// <returns></returns>
    public static ushort UInt16()
        => BitConverter.ToUInt16(BaseGen(2));

    /// <summary>
    /// Generates a random Int16
    /// </summary>
    /// <returns></returns>
    public static short Int16()
        => BitConverter.ToInt16(BaseGen(2));

    /// <summary>
    /// Generates a random UInt32
    /// </summary>
    /// <returns></returns>
    public static uint UInt32()
        => BitConverter.ToUInt32(BaseGen(4));

    /// <summary>
    /// Generates a random Int32
    /// </summary>
    /// <returns></returns>
    public static int Int32()
        => BitConverter.ToInt32(BaseGen(4));

    /// <summary>
    /// Generates a random UInt64
    /// </summary>
    /// <returns></returns>
    public static ulong UInt64()
        => BitConverter.ToUInt64(BaseGen(8));

    /// <summary>
    /// Generates a random Int64
    /// </summary>
    /// <returns></returns>
    public static long Int64()
        => BitConverter.ToInt64(BaseGen(8));

    /// <summary>
    /// Generates a random Single
    /// </summary>
    /// <returns></returns>
    public static float Single()
        => BitConverter.ToSingle(BaseGen(4));

    /// <summary>
    /// Generates a random Double
    /// </summary>
    /// <returns></returns>
    public static double Double()
        => BitConverter.ToDouble(BaseGen(8));

    /// <summary>
    /// Generates a random byte
    /// </summary>
    /// <returns></returns>
    public static byte Byte()
        => BaseGen(1).First();

    /// <summary>
    /// Generates a random byte[]
    /// </summary>
    /// <returns></returns>
    public static byte[] ByteArray(ushort length)
        => BaseGen(length);

    /// <summary>
    /// Generate a random Int32 between two specified values.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int Generate(int min, int max)
    {
        // Handle invalid input
        if (max < min)
            throw new ArgumentOutOfRangeException($"{nameof(max)} be less than {nameof(min)}");
        else if (max == min)
            throw new ArgumentOutOfRangeException($"{nameof(min)} and {nameof(max)} should not be the same");

        // Return random number in range
        int range = max - min + 1;
        return (int)((UInt32() % range) + min);
    }

    /// <summary>
    /// Generate a random Int32 between 0 and provided max
    /// </summary>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int Generate(int max)
        => Generate(0, max);

    /// <summary>
    /// Generate a random Int32 between 0 and Int32.MaxValue
    /// </summary>
    /// <returns></returns>
    public static int Generate()
        => Generate(0, int.MaxValue);

    /// <summary>
    /// Base random number generator function. Other implementations in this library rely on this.
    /// Supports up to 65535 bytes. Write a custom implementation if you need more.
    /// </summary>
    /// <returns>Random byte[]</returns>
    private static byte[] BaseGen(ushort byteCount)
    {
        // Detail OutOfRangeException
        if (byteCount == 0)
            throw new ArgumentOutOfRangeException($"{nameof(byteCount)} must be greater than 0.");

        // Generate byte[]
        byte[] resultBytes = new byte[byteCount];
        lock (RngProvider)
            RngProvider.GetBytes(resultBytes);
        return resultBytes;
    }
}
