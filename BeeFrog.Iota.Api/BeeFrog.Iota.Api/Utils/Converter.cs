﻿using BeeFrog.Iota.Api.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeeFrog.Iota.Api.Utils
{
    /// <summary>
    /// This class provides a set of utility methods to are used to convert between different formats
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// The radix
        /// </summary>
        public static readonly int Radix = 3;

        /// <summary>
        /// The maximum trit value
        /// </summary>
        public static readonly int MaxTritValue = (Radix - 1) / 2;

        /// <summary>
        /// The minimum trit value
        /// </summary>
        public static readonly int MinTritValue = -MaxTritValue;

        /// <summary>
        /// The number of trits in a byte
        /// </summary>
        public static readonly int NumberOfTritsInAByte = 5;

        /// <summary>
        /// The number of trits in a tryte
        /// </summary>
        public static readonly int NumberOfTritsInATryte = 3;

        static readonly int[][] ByteToTritsMappings = new int[243][];
        static readonly int[][] TryteToTritsMappings = new int[27][];

        static Converter()
        {
            int[] trits = new int[NumberOfTritsInAByte];

            for (int i = 0; i < 243; i++)
            {
                ByteToTritsMappings[i] = new int[NumberOfTritsInAByte];
                ByteToTritsMappings[i] = new int[NumberOfTritsInAByte];
                Array.Copy(trits, ByteToTritsMappings[i], NumberOfTritsInAByte);
                Increment(trits, NumberOfTritsInAByte);
            }

            for (int i = 0; i < 27; i++)
            {
                TryteToTritsMappings[i] = new int[NumberOfTritsInATryte];
                Array.Copy(trits, TryteToTritsMappings[i], NumberOfTritsInATryte);
                Increment(trits, NumberOfTritsInATryte);
            }
        }

        /// <summary>
        /// Converts the specified trits array to bytes
        /// </summary>
        /// <param name="trits">The trits.</param>
        /// <param name="offset">The offset to start from.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static byte[] ToBytes(int[] trits, int offset, int size)
        {
            byte[] bytes = new byte[(size + NumberOfTritsInAByte - 1) / NumberOfTritsInAByte];
            for (int i = 0; i < bytes.Length; i++)
            {
                int value = 0;
                for (
                    int j = (size - i * NumberOfTritsInAByte) < 5
                        ? (size - i * NumberOfTritsInAByte)
                        : NumberOfTritsInAByte;
                    j-- > 0;)
                {
                    value = value * Radix + trits[offset + i * NumberOfTritsInAByte + j];
                }
                bytes[i] = (byte)value;
            }

            return bytes;
        }

        /// <summary>
        /// Converts the specified trits array to bytes
        /// </summary>
        /// <param name="trits">The trits.</param>
        /// <param name="offset">The offset to start from.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static sbyte[] ToSBytes(int[] trits, int offset, int size)
        {
            sbyte[] bytes = new sbyte[(size + NumberOfTritsInAByte - 1) / NumberOfTritsInAByte];
            for (int i = 0; i < bytes.Length; i++)
            {
                int value = 0;
                for (
                    int j = (size - i * NumberOfTritsInAByte) < 5
                        ? (size - i * NumberOfTritsInAByte)
                        : NumberOfTritsInAByte;
                    j-- > 0;)
                {
                    value = value * Radix + trits[offset + i * NumberOfTritsInAByte + j];
                }
                bytes[i] = (sbyte)value;
            }

            return bytes;
        }

        /// <summary>
        /// Converts the specified trits array to bytes
        /// </summary>
        /// <param name="trits">The trits.</param>
        /// <param name="offset">The offset to start from.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static sbyte[] ToSBytes2(int[] trits, int offset, int size)
        {
            var bytes = ToBytes(trits, offset, size);
            var sbytes = bytes.Select(b => Convert.ToSByte(b)).ToArray();
            return sbytes;
        }

        /// <summary>
        /// Converts the specified trits to trytes
        /// </summary>
        /// <param name="trits">The trits.</param>
        /// <returns></returns>
        public static byte[] ToBytes(int[] trits)
        {
            return ToBytes(trits, 0, trits.Length);
        }

        /// <summary>
        /// Gets the trits from the specified bytes and stores it into the provided trits array
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="trits">The trits.</param>
        public static void GetTrits(sbyte[] bytes, int[] trits)
        {
            int offset = 0;
            for (int i = 0; i < bytes.Length && offset < trits.Length; i++)
            {
                Array.Copy(
                    ByteToTritsMappings[bytes[i] < 0 ? (bytes[i] + ByteToTritsMappings.Length) : bytes[i]], 0,
                    trits, offset,
                    trits.Length - offset < NumberOfTritsInAByte
                        ? (trits.Length - offset)
                        : NumberOfTritsInAByte);

                offset += NumberOfTritsInAByte;
            }
            while (offset < trits.Length)
            {
                trits[offset++] = 0;
            }
        }

        /// <summary>
        /// Converts the specified trinary encoded string into trits
        /// </summary>
        /// <param name="trytes">The trytes.</param>
        /// <returns></returns>
        public static int[] ToTritsString(string trytes)
        {
            int[] d = new int[3 * trytes.Length];
            for (int i = 0; i < trytes.Length; i++)
            {
                var tryteIndex = Constants.TryteAlphabet.IndexOf(trytes[i]);
                if (tryteIndex < 0)
                    throw new InvalidTryteException($"The specified tryte is invalid. It contains '{trytes[i]}'");

                Array.Copy(TryteToTritsMappings[tryteIndex], 0, d,
                    i * NumberOfTritsInATryte, NumberOfTritsInATryte);
            }
            return d;
        }

        /// <summary>
        /// Converts the specified trinary encoded string into a trits array of the specified length.
        /// If the trytes string results in a shorter then specified trits array, then the remainder is padded we zeroes
        /// </summary>
        /// <param name="trytes">The trytes.</param>
        /// <param name="length">The length.</param>
        /// <returns>a trits array</returns>
        public static int[] ToTrits(string trytes, int length)
        {
            int[] tritss = ToTrits(trytes);

            List<int> tritsList = new List<int>();

            foreach (int i in tritss)
                tritsList.Add(i);

            while (tritsList.Count < length)
                tritsList.Add(0);

            return tritsList.ToArray();
        }


        /// <summary>
        /// Converts the specified trinary encoded trytes string to trits
        /// </summary>
        /// <param name="trytes">The trytes.</param>
        /// <returns></returns>
        public static int[] ToTrits(string trytes)
        {
            List<int> trits = new List<int>();
            if (InputValidator.IsValue(trytes))
            {
                long value = long.Parse(trytes);

                long absoluteValue = value < 0 ? -value : value;

                int position = 0;

                while (absoluteValue > 0)
                {
                    int remainder = (int)(absoluteValue % Radix);
                    absoluteValue /= Radix;

                    if (remainder > MaxTritValue)
                    {
                        remainder = MinTritValue;
                        absoluteValue++;
                    }

                    trits.Insert(position++, remainder);
                }
                if (value < 0)
                {
                    for (int i = 0; i < trits.Count; i++)
                    {
                        trits[i] = -trits[i];
                    }
                }
            }
            else
            {
                return ToTritsString(trytes);
            }
            return trits.ToArray();
        }

        /// <summary>
        /// Copies the trits from the input string into the destination array
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="destination">The destination array.</param>
        /// <returns></returns>
        public static int[] CopyTrits(string input, int[] destination)
        {
            for (int i = 0; i < input.Length; i++)
            {
                int index = Constants.TryteAlphabet.IndexOf(input[i]);
                destination[i * 3] = TryteToTritsMappings[index][0];
                destination[i * 3 + 1] = TryteToTritsMappings[index][1];
                destination[i * 3 + 2] = TryteToTritsMappings[index][2];
            }
            return destination;
        }

        /// <summary>
        /// Copies the trits in long representation into the destination array
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="destination">The destination array.</param>
        /// <param name="offset">The offset from which copying is started.</param>
        /// <param name="size">The size.</param>
        public static void CopyTrits(long value, int[] destination, int offset, int size)
        {
            long absoluteValue = value < 0 ? -value : value;
            for (int i = 0; i < size; i++)
            {
                int remainder = (int)(absoluteValue % Radix);
                absoluteValue /= Radix;
                if (remainder > MaxTritValue)
                {
                    remainder = MinTritValue;
                    absoluteValue++;
                }
                destination[offset + i] = remainder;
            }

            if (value < 0)
            {
                for (int i = 0; i < size; i++)
                {
                    destination[offset + i] = -destination[offset + i];
                }
            }
        }

        /// <summary>
        /// Converts the trits array to a trytes string
        /// </summary>
        /// <param name="trits">The trits.</param>
        /// <param name="offset">The offset from which copying is started.</param>
        /// <param name="size">The size.</param>
        /// <returns>a trytes string</returns>
        public static string ToTrytes(int[] trits, int offset, int size)
        {
            StringBuilder trytes = new StringBuilder();
            for (int i = 0; i < (size + NumberOfTritsInATryte - 1) / NumberOfTritsInATryte; i++)
            {
                int j = trits[offset + i * 3] + trits[offset + i * 3 + 1] * 3 + trits[offset + i * 3 + 2] * 9;
                if (j < 0)
                {
                    j += Constants.TryteAlphabet.Length;
                }
                trytes.Append(Constants.TryteAlphabet[j]);
            }
            return trytes.ToString();
        }

        /// <summary>
        /// Converts the trits array to a trytes string
        /// </summary>
        /// <param name="trits">The trits.</param>
        /// <returns>a trytes string</returns>
        public static string ToTrytes(int[] trits)
        {
            return ToTrytes(trits, 0, trits.Length);
        }

        /// <summary>
        /// Converts the specified trits array to trytes in integer representation
        /// </summary>
        /// <param name="trits">The trits.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>trytes in integer representation</returns>
        public static int ToTryteValue(int[] trits, int offset)
        {
            return trits[offset] + trits[offset + 1] * 3 + trits[offset + 2] * 9;
        }

        /// <summary>
        /// Converts the specified trits to its corresponding integer value
        /// </summary>
        /// <param name="trits">The trits.</param>
        /// <returns>an integer value representing the corresponding trits</returns>
        public static int ToValue(int[] trits)
        {
            int value = 0;

            for (int i = trits.Length; i-- > 0;)
            {
                value = value * 3 + trits[i];
            }
            return value;
        }

        /// <summary>
        ///  Converts the specified trits to its corresponding integer value
        /// </summary>
        /// <param name="trits">The trits.</param>
        /// <returns></returns>
        public static long ToLongValue(int[] trits)
        {
            long value = 0;

            for (int i = trits.Length; i-- > 0;)
            {
                value = value * 3 + trits[i];
            }
            return value;
        }

        /// <summary>
        /// Converts a raw trytes value into the long value.
        /// </summary>
        /// <param name="trytes">The trytes to use</param>
        /// <param name="startOffset">Where within the given trytes string the value starts</param>
        /// <param name="endOffset">Where within the given trytes string the value ends</param>
        /// <returns></returns>
        public static long ToLongValue(string trytes, int startOffset, int endOffset)
        {
            var tryteValue = trytes.Substring(startOffset, endOffset - startOffset);
            if (tryteValue == new string('9', tryteValue.Length))
            {
                return 0;
            }

            var trites = Converter.ToTrits(tryteValue);
            return Converter.ToLongValue(trites);
        }

        /// <summary>
        /// Increments the specified trits.
        /// </summary>
        /// <param name="trits">The trits.</param>
        /// <param name="size">The size.</param>
        public static void Increment(int[] trits, int size)
        {
            for (int i = 0; i < size; i++)
            {
                if (++trits[i] > MaxTritValue)
                {
                    trits[i] = MinTritValue;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Converts ascii to trytes
        /// </summary>
        /// <param name="input">The ascii string.</param>
        public static string AsciiToTrytes(string input)
        {
            string trytes = "";
            for (var i = 0; i < input.Length; i++)
            {
                var asciiValue = input[i];

                // If not recognizable ASCII character, return null
                if (asciiValue > 255)
                {
                    //asciiValue = 32
                    return null;
                }

                var firstValue = asciiValue % 27;
                var secondValue = (asciiValue - firstValue) / 27;

                string trytesValue = $"{Constants.TryteAlphabet[firstValue]}{Constants.TryteAlphabet[secondValue]}";

                trytes += trytesValue;
            }
            return trytes;
        }

        /// <summary>
        /// Converts trytes string to ascii string
        /// </summary>
        /// <param name="input">The trytes string.</param>
        public static string TrytesToAscii(string input)
        {
            string asciiString = "";

            for (var i = 0; i < input.Length; i += 2)
            {
                var trytes = $"{input[i]}{input[i + 1]}";

                var firstValue = Constants.TryteAlphabet.IndexOf(trytes[0]);
                var secondValue = Constants.TryteAlphabet.IndexOf(trytes[1]);
                var decimalValue = firstValue + secondValue * 27;
                var character = (char)decimalValue;

                asciiString += character;
            }

            return asciiString;
        }



        public const int RADIX = 3;
        public const int RADIX_BYTES = 256;
        public const int MAX_TRIT_VALUE = 1;
        public const int MIN_TRIT_VALUE = -1;
        public const int BYTE_HASH_LENGTH = 48;

        /// <summary>
        /// All possible tryte values
        /// </summary>
        public static string trytesAlphabet = "9ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        static sbyte[,] trytesTrits = new sbyte[,] {
            {0, 0, 0}, {1, 0, 0}, {-1, 1, 0}, {0, 1, 0}, {1, 1, 0}, {-1, -1, 1},
            {0, -1, 1}, {1, -1, 1}, {-1, 0, 1}, {0, 0, 1}, {1, 0, 1}, {-1, 1, 1},
            {0, 1, 1}, {1, 1, 1}, {-1, -1, -1}, {0, -1, -1}, {1, -1, -1}, {-1, 0, -1},
            {0, 0, -1}, {1, 0, -1}, {-1, 1, -1}, {0, 1, -1}, {1, 1, -1}, {-1, -1, 0},
            {0, -1, 0}, {1, -1, 0}, {-1, 0, 0} };

        /// <summary>
        /// Converts trytes into trits.
        /// </summary>
        /// <param name="trytes">The trytes. Can be string value or int</param>
        /// <param name="state">The state. Optional.</param>
        /// <returns></returns>
        public static sbyte[] GetTrits(object trytes, sbyte[] state = null)
        {
            if (trytes == null)
                throw new ArgumentNullException(nameof(trytes));

            var trits = state?.ToList() ?? new List<sbyte>();

            if (!(trytes is string))
            {
                var intValue = (int)trytes;
                return GetTritsFromInt(intValue);
            }
            else
            {
                var stringValue = (trytes as string) ?? trytes.ToString();

                for (var i = 0; i < stringValue.Length; i++)
                {
                    var index = trytesAlphabet.IndexOf(stringValue[i]);
                    trits.Add(trytesTrits[index, 0]);
                    trits.Add(trytesTrits[index, 1]);
                    trits.Add(trytesTrits[index, 2]);
                }
            }

            return trits.ToArray();
        }

        /// <summary>
        /// Get trytes from int value
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns></returns>
        public static sbyte[] GetTritsFromInt(int value)
        {
            var destination = new List<sbyte>();
            var absoluteValue = Math.Abs(value);
            var i = 0;

            while (absoluteValue > 0)
            {

                var remainder = (sbyte)(absoluteValue % RADIX);
                absoluteValue = (int)Math.Floor((double)absoluteValue / RADIX);

                if (remainder > MAX_TRIT_VALUE)
                {
                    remainder = MIN_TRIT_VALUE;
                    absoluteValue++;
                }

                destination.Add(remainder);
                i++;

            }

            if (value < 0)
            {
                for (var j = 0; j < destination.Count; j++)
                {
                    // switch values
                    destination[j] = (sbyte)(-destination[j]);

                }
            }

            return destination.ToArray();
        }

        /// <summary>
        /// Get trytes from trits
        /// </summary>
        /// <param name="trits">The trits</param>
        /// <returns></returns>
        public static string GetTrytes(sbyte[] trits)
        {
            var trytes = "";

            for (var i = 0; i < trits.Length; i += 3)
            {
                // Iterate over all possible tryte values to find correct trit representation
                for (var j = 0; j < trytesAlphabet.Length; j++)
                {
                    if (trytesTrits[j, 0] == trits[i] && trytesTrits[j, 1] == trits[i + 1] && trytesTrits[j, 2] == trits[i + 2])
                    {
                        trytes += trytesAlphabet[j];
                        break;

                    }
                }
            }

            return trytes;
        }

        /// <summary>
        /// Get int value from trits
        /// </summary>
        /// <param name="trits">The trits</param>
        /// <returns></returns>
        public static int GetInt(sbyte[] trits)
        {
            var returnValue = 0;

            for (var i = trits.Length; i-- > 0;)
            {
                returnValue = returnValue * 3 + trits[i];
            }

            return returnValue;
        }

    }
}
