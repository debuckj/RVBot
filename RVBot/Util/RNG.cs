using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RVBot.Util
{
    public static class Util
    {
        public static async Task<string> AnalyzeRNG(CommandContext context, int range, int loop)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            int[][] valueTable = new int[][] { new int[range], new int[range] };

            for (int v = 0; v < range; v++) { valueTable[0][v] = v+1; valueTable[1][v] = 0; }
            for (int i = 1; i <= loop; i++) { valueTable[1][GetCryptoRandom(range)]++; }

            var textTable = Temp.ToString((
                from value in valueTable[0]
                orderby value ascending
                select new
                {
                    Number = valueTable[0][value-1],
                    Occurence = valueTable[1][value-1]
                }).ToList());

            var statusMsg = await context.Channel.SendMessageAsync(String.Format("`Query time: {0} - Offset: {1}`", sw.Elapsed.ToString("mm'm 'ss's 'fff'ms'"), (valueTable[1].Max() - valueTable[1].Min())));
            return textTable;
        }

        public static int GetSecureRandom(int range)
        {
            byte[] intBytes = BitConverter.GetBytes(range);
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(intBytes);
                int randomvalue = BitConverter.ToInt32(intBytes, 0);
                return randomvalue;
            }
        }

        public static int GetPseudoRandom(int range)
        {
            return 0;
        }

        public static int GetCryptoRandom(int range)
        {
            CryptoRandom CR = new CryptoRandom(true);
            return CR.Next(range);
        }

    }


    public class CryptoRandom : Random
    {
        private RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
        private byte[] _buffer;
        private int _bufferPosition;
        public bool IsRandomPoolEnabled { get; private set; }

        public CryptoRandom() : this(true) { }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "ignoredSeed", Justification = "Cannot remove this parameter as we implement the full API of System.Random")]
        public CryptoRandom(int ignoredSeed) : this(true) { }

        public CryptoRandom(bool enableRandomPool)
        {
            IsRandomPoolEnabled = enableRandomPool;
        }

        private void InitBuffer()
        {
            if (IsRandomPoolEnabled)
            {
                if (_buffer == null || _buffer.Length != 512)
                    _buffer = new byte[512];
            }
            else
            {
                if (_buffer == null || _buffer.Length != 4)
                    _buffer = new byte[4];
            }

            _rng.GetBytes(_buffer);
            _bufferPosition = 0;
        }

        public override int Next() { return (int)GetRandomUInt32() & 0x7FFFFFFF; }

        public override int Next(int maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue");

            return Next(0, maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue");

            if (minValue == maxValue)
                return minValue;

            long diff = maxValue - minValue;

            while (true)
            {
                uint rand = GetRandomUInt32();

                long max = 1 + (long)uint.MaxValue;
                long remainder = max % diff;

                if (rand < max - remainder)
                    return (int)(minValue + (rand % diff));
            }
        }

        public override double NextDouble()
        {
            return GetRandomUInt32() / (1.0 + uint.MaxValue);
        }

        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            lock (this)
            {
                if (IsRandomPoolEnabled && _buffer == null)
                    InitBuffer();

                if (IsRandomPoolEnabled && _buffer.Length <= buffer.Length)
                {
                    int count = buffer.Length;

                    EnsureRandomBuffer(count);

                    Buffer.BlockCopy(_buffer, _bufferPosition, buffer, 0, count);

                    _bufferPosition += count;
                }
                else
                {
                    // Draw bytes from the RNGCryptoProvider
                    _rng.GetBytes(buffer);
                }
            }
        }

        private uint GetRandomUInt32()
        {
            lock (this)
            {
                EnsureRandomBuffer(4);

                uint rand = BitConverter.ToUInt32(_buffer, _bufferPosition);

                _bufferPosition += 4;

                return rand;
            }
        }

        private void EnsureRandomBuffer(int requiredBytes)
        {
            if (_buffer == null)
                InitBuffer();

            if (requiredBytes > _buffer.Length)
                throw new ArgumentOutOfRangeException("requiredBytes", "cannot be greater than random buffer");

            if ((_buffer.Length - _bufferPosition) < requiredBytes)
                InitBuffer();
        }
    }

}
