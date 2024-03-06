namespace Robook.Helpers;

public static class RandomDataGenerator {
    public static T[] NumericArray<T>(int length, T minValue = default, T maxValue = default)
        where T : struct, IComparable<T> {
        if (length < 0) {
            throw new ArgumentException("Length must be non-negative.");
        }

        if (minValue.CompareTo(maxValue) >= 0) {
            throw new ArgumentException("MaxValue must be greater than minValue.");
        }

        T[]     result = new T[length];
        dynamic range  = Convert.ToDouble(maxValue) - Convert.ToDouble(minValue);

        for (int i = 0; i < length; i++) {
            dynamic randomValue = Convert.ToDouble(minValue) + Random.Shared.NextDouble() * range;
            result[i] = (T)Convert.ChangeType(randomValue, typeof(T));
        }

        return result;
    }

    public static string[] StringArray(int length, int stringLength = 8) {
        if (length < 0 || stringLength < 0) {
            throw new ArgumentException("Length and stringLength must be non-negative.");
        }

        const string chars  = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string[]     result = new string[length];

        for (int i = 0; i < length; i++) {
            char[] randomChars = new char[stringLength];
            for (int j = 0; j < stringLength; j++) {
                randomChars[j] = chars[Random.Shared.Next(chars.Length)];
            }

            result[i] = new string(randomChars);
        }

        return result;
    }
}