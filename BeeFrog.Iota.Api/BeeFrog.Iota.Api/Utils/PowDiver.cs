﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeeFrog.Iota.Api.Utils
{
    public class PowDiver
    {
        public const int TRANSACTION_LENGTH = 8019;

        public const int CURL_HASH_LENGTH = 243;
        public const int CURL_STATE_LENGTH = CURL_HASH_LENGTH * 3;

        public const ulong HIGH_BITS = 0xFFFFFFFFFFFFFFFF;
        public const ulong LOW_BITS = 0x0000000000000000;
        public const ulong LOW_0 = 0xDB6DB6DB6DB6DB6D;
        public const ulong HIGH_0 = 0xB6DB6DB6DB6DB6DB;
        public const ulong LOW_1 = 0xF1F8FC7E3F1F8FC7;
        public const ulong HIGH_1 = 0x8FC7E3F1F8FC7E3F;
        public const ulong LOW_2 = 0x7FFFE00FFFFC01FF;
        public const ulong HIGH_2 = 0xFFC01FFFF803FFFF;
        public const ulong LOW_3 = 0xFFC0000007FFFFFF;
        public const ulong HIGH_3 = 0x003FFFFFFFFFFFFF;

        public const int RUNNING = 0;
        public const int CANCELLED = 1;
        public const int COMPLETED = 2;
        private const int REQUIRED_ROUNDS = 81;

        public volatile int state;
        public object syncObj = new object();

        public Task<string> DoPow(string trytes, int minWeightMagnitude, CancellationToken cancellationToken)
        {
            return DoPow(trytes, minWeightMagnitude, 0, cancellationToken);
        }

        public Task<string> DoPow(string trytes, int minWeightMagnitude)
        {
            return DoPow(trytes, minWeightMagnitude, 0, CancellationToken.None);
        }

        public Task<string> DoPow(string trytes, int minWeightMagnitude, int numberOfThreads)
        {
            return DoPow(trytes, minWeightMagnitude, numberOfThreads, CancellationToken.None);
        }

        public async Task<string> DoPow(string trytes, int minWeightMagnitude, int numberOfThreads, CancellationToken CancellationToken)
        {
            var intxTrits = Converter.ToTrits(trytes);
            await Search(intxTrits, minWeightMagnitude, numberOfThreads, CancellationToken);
            var resultTrytes = Converter.ToTrytes(intxTrits);
            return resultTrytes;
        }
        public Task Search(int[] transactionTrits, int minWeightMagnitude, int numberOfThreads)
        {
            return Search(transactionTrits, minWeightMagnitude, numberOfThreads, CancellationToken.None);
        }


        public async Task Search(int[] transactionTrits, int minWeightMagnitude, int numberOfThreads, CancellationToken CancellationToken)
        {

            if (transactionTrits.Length != TRANSACTION_LENGTH)
            {
                throw new Exception("Invalid transaction trits length: " + transactionTrits.Length);
            }
            if (minWeightMagnitude < 0 || minWeightMagnitude > CURL_HASH_LENGTH)
            {
                throw new Exception("Invalid min weight magnitude: " + minWeightMagnitude);
            }

            lock (syncObj)
            {
                state = RUNNING;
            }

            ulong[] midCurlStateLow = new ulong[CURL_STATE_LENGTH], midCurlStateHigh = new ulong[CURL_STATE_LENGTH];

            {
                for (int i = CURL_HASH_LENGTH; i < CURL_STATE_LENGTH; i++)
                {

                    midCurlStateLow[i] = HIGH_BITS;
                    midCurlStateHigh[i] = HIGH_BITS;
                }

                int offset = 0;
                ulong[] curlScratchpadLow = new ulong[CURL_STATE_LENGTH], curlScratchpadHigh = new ulong[CURL_STATE_LENGTH];
                for (int i = (TRANSACTION_LENGTH - CURL_HASH_LENGTH) / CURL_HASH_LENGTH; i-- > 0;)
                {

                    
                    for (int j = 0; j < CURL_HASH_LENGTH; j++)
                    {

                        switch (transactionTrits[offset++])
                        {

                            case 0:
                                midCurlStateLow[j] = HIGH_BITS;
                                midCurlStateHigh[j] = HIGH_BITS;
                                break;
                            case 1:
                                midCurlStateLow[j] = LOW_BITS;
                                midCurlStateHigh[j] = HIGH_BITS;
                                break;
                            default:
                                midCurlStateLow[j] = HIGH_BITS;
                                midCurlStateHigh[j] = LOW_BITS;
                                break;
                        }
                    }

                    Transform(midCurlStateLow, midCurlStateHigh, curlScratchpadLow, curlScratchpadHigh);
                }

                // Added from java lib.
                for (int i = 0; i < 162; i++)
                {
                    switch (transactionTrits[offset++])
                    {
                        case 0:
                            midCurlStateLow[i] = 0b1111111111111111111111111111111111111111111111111111111111111111L;
                            midCurlStateHigh[i] = 0b1111111111111111111111111111111111111111111111111111111111111111L;
                            break;
                        case 1:
                            midCurlStateLow[i] = 0b0000000000000000000000000000000000000000000000000000000000000000L;
                            midCurlStateHigh[i] = 0b1111111111111111111111111111111111111111111111111111111111111111L;
                            break;
                        default:
                            midCurlStateLow[i] = 0b1111111111111111111111111111111111111111111111111111111111111111L;
                            midCurlStateHigh[i] = 0b0000000000000000000000000000000000000000000000000000000000000000L;
                            break;
                    }
                }


                midCurlStateLow[162 + 0] = LOW_0; // 0b1101101101101101101101101101101101101101101101101101101101101101L;
                midCurlStateHigh[162 + 0] = HIGH_0; // 0b1011011011011011011011011011011011011011011011011011011011011011L;
                midCurlStateLow[162 + 1] = LOW_1; // 0b1111000111111000111111000111111000111111000111111000111111000111L;
                midCurlStateHigh[162 + 1] = HIGH_1; // 0b1000111111000111111000111111000111111000111111000111111000111111L;
                midCurlStateLow[162 + 2] = LOW_2; // 0b0111111111111111111000000000111111111111111111000000000111111111L;
                midCurlStateHigh[162 + 2] = HIGH_2; // 0b1111111111000000000111111111111111111000000000111111111111111111L;
                midCurlStateLow[162 + 3] = LOW_3; // 0b1111111111000000000000000000000000000111111111111111111111111111L;
                midCurlStateHigh[162 + 3] = HIGH_3; // 0b0000000000111111111111111111111111111111111111111111111111111111L;
            }

            if (numberOfThreads <= 0)
            {
                numberOfThreads = Environment.ProcessorCount - 1;
                if (numberOfThreads < 1)
                {
                    numberOfThreads = 1;
                }
            }

            List<Task> tasks = new List<Task>();

            while (numberOfThreads-- > 0)
            {

                int threadIndex = numberOfThreads;
                var task = Task.Factory.StartNew(() =>
                {

                    ulong[] midCurlStateCopyLow = new ulong[CURL_STATE_LENGTH], midCurlStateCopyHigh = new ulong[CURL_STATE_LENGTH];
                    System.Array.Copy(midCurlStateLow, 0, midCurlStateCopyLow, 0, CURL_STATE_LENGTH);
                    System.Array.Copy(midCurlStateHigh, 0, midCurlStateCopyHigh, 0, CURL_STATE_LENGTH);
                    for (int i = threadIndex; i-- > 0;)
                    {
                        Increment(midCurlStateCopyLow, midCurlStateCopyHigh, 162 + CURL_HASH_LENGTH / 9, 162 + (CURL_HASH_LENGTH / 9) * 2);
                    }

                    ulong[] curlStateLow = new ulong[CURL_STATE_LENGTH], curlStateHigh = new ulong[CURL_STATE_LENGTH];
                    ulong[] curlScratchpadLow = new ulong[CURL_STATE_LENGTH], curlScratchpadHigh = new ulong[CURL_STATE_LENGTH];
                    ulong mask, outMask = 1;
                    while (state == RUNNING)
                    {
                        CancellationToken.ThrowIfCancellationRequested();
                        Increment(midCurlStateCopyLow, midCurlStateCopyHigh, 162 + (CURL_HASH_LENGTH / 9) * 2, CURL_HASH_LENGTH);
                        System.Array.Copy(midCurlStateCopyLow, 0, curlStateLow, 0, CURL_STATE_LENGTH);
                        System.Array.Copy(midCurlStateCopyHigh, 0, curlStateHigh, 0, CURL_STATE_LENGTH);
                        Transform(curlStateLow, curlStateHigh, curlScratchpadLow, curlScratchpadHigh);

                        mask = HIGH_BITS;
                        for (int i = minWeightMagnitude; i-- > 0;)
                        {
                            mask &= ~(curlStateLow[CURL_HASH_LENGTH - 1 - i] ^ curlStateHigh[CURL_HASH_LENGTH - 1 - i]);
                            if (mask == 0)
                            {
                                break;
                            }
                        }
                        if (mask == 0) continue;

                        lock (syncObj)
                        {
                            if (state == RUNNING)
                            {
                                state = COMPLETED;
                                while ((outMask & mask) == 0)
                                {
                                    outMask <<= 1;
                                }
                                for (int i = 0; i < CURL_HASH_LENGTH; i++)
                                {
                                    transactionTrits[TRANSACTION_LENGTH - CURL_HASH_LENGTH + i] = (midCurlStateCopyLow[i] & outMask) == 0 ? 1 : (midCurlStateCopyHigh[i] & outMask) == 0 ? -1 : 0;
                                }
                            }
                        }
                        break;
                    }
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        private static void Transform(ulong[] curlStateLow, ulong[] curlStateHigh, ulong[] curlScratchpadLow, ulong[] curlScratchpadHigh)
        {
            int curlScratchpadIndex = 0;
            for (int round = 0; round < REQUIRED_ROUNDS; round++)
            {
                System.Array.Copy(curlStateLow, 0, curlScratchpadLow, 0, CURL_STATE_LENGTH);
                System.Array.Copy(curlStateHigh, 0, curlScratchpadHigh, 0, CURL_STATE_LENGTH);

                for (int curlStateIndex = 0; curlStateIndex < CURL_STATE_LENGTH; curlStateIndex++)
                {

                    ulong alpha = curlScratchpadLow[curlScratchpadIndex];
                    ulong beta = curlScratchpadHigh[curlScratchpadIndex];
                    if (curlScratchpadIndex < 365)
                    {
                        curlScratchpadIndex += 364;
                    }
                    else
                    {
                        curlScratchpadIndex += -365;
                    }
                    ulong gamma = curlScratchpadHigh[curlScratchpadIndex];
                    ulong delta = (alpha | (~gamma)) & (curlScratchpadLow[curlScratchpadIndex] ^ beta);

                    curlStateLow[curlStateIndex] = ~delta;
                    curlStateHigh[curlStateIndex] = (alpha ^ gamma) | delta;
                }
            }
        }

        private static void Increment(ulong[] midCurlStateCopyLow, ulong[] midCurlStateCopyHigh, int fromIndex, int toIndex)
        {
            for (int i = fromIndex; i < toIndex; i++)
            {
                if (midCurlStateCopyLow[i] == LOW_BITS)
                {
                    midCurlStateCopyLow[i] = HIGH_BITS;
                    midCurlStateCopyHigh[i] = LOW_BITS;
                }
                else
                {
                    if (midCurlStateCopyHigh[i] == LOW_BITS)
                    {
                        midCurlStateCopyHigh[i] = HIGH_BITS;
                    }
                    else
                    {
                        midCurlStateCopyLow[i] = LOW_BITS;
                    }
                    break;
                }
            }
        }
    }
}
