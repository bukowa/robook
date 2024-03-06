using Robook.Helpers;
using NUnit.Framework;
using System;

namespace Robook.UnitTest.Helpers;

[TestFixture]
public class RandomDataGeneratorTests {
    [Test]
    public void TestNumericArray_LengthLessThanZero_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() => RandomDataGenerator.NumericArray<int>(-1));
    }

    [Test]
    public void TestNumericArray_MinGreaterThanMax_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() => RandomDataGenerator.NumericArray<int>(5, 10, 2));
    }

    [Test]
    public void TestNumericArray_MinEqualToMax_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() => RandomDataGenerator.NumericArray<int>(5, 10, 10));
    }

    [Test]
    public void TestNumericArray_GeneratesNumericArray() {
        int[] result = RandomDataGenerator.NumericArray<int>(5, 1, 10);

        Assert.That(result.All(value => value >= 1 && value <= 10), Is.True);
        Assert.That(result.Length,                                  Is.EqualTo(5));
    }

    [Test]
    public void TestStringArray_LengthLessThanZero_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() => RandomDataGenerator.StringArray(-1));
    }

    [Test]
    public void TestStringArray_StringLengthLessThanZero_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() => RandomDataGenerator.StringArray(2, -1));
    }

    [Test]
    public void TestStringArray_GeneratesStringArray() {
        string[] result = RandomDataGenerator.StringArray(5, 8);
        Assert.That(result.Length,                          Is.EqualTo(5));
        Assert.That(result.All(value => value.Length == 8), Is.True);
    }
}