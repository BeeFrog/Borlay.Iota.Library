﻿using BeeFrog.Iota.Api.Exceptions;
using BeeFrog.Iota.Api.Iri;
using BeeFrog.Iota.Api.Models;
using BeeFrog.Iota.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeeFrog.Iota.Api
{
    public static class IotaApiExtensions
    {
        /// <summary>
        /// Sends transfer with money.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="transferItem">Transfer item to send</param>
        /// <param name="seed">Seed from which you want to send money.</param>
        /// <param name="startFromIndex">Index to start search for address</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public static async Task<APIResult<TransactionItem[]>> AttachTransfer(this IotaApi api, TransferItem transferItem, string seed, int startFromIndex, CancellationToken cancellationToken)
        {
            var transactionItemsToSend = await api.CreateTransactions(transferItem, seed, startFromIndex, cancellationToken);
            var transactionItems = await api.AttachTransactions(transactionItemsToSend, cancellationToken);

            return transactionItems.Successful ? transactionItems : transactionItems.RePackage(r => new TransactionItem[0]);
        }

        /// <summary>
        /// Sends transfer without money
        /// </summary>
        /// <param name="api"></param>
        /// <param name="transferItem">Transfer item</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public static Task<APIResult<TransactionItem[]>> AttachTransfer(this IotaApi api, TransferItem transferItem, CancellationToken cancellationToken)
        {
            var transactionItems = transferItem.CreateTransactions();
            return api.AttachTransactions(transactionItems, cancellationToken);
        }

        public static string[] GetTrytes(this IEnumerable<TransactionItem> transactions)
        {
            var transactionTrytes = transactions
                .OrderByDescending(o => o.CurrentIndex)
                .Select(t => t.ToTransactionTrytes())
                .ToArray();
            return transactionTrytes;
        }

        //public static async Task<TransactionItem> GetTransactions(this AddressItem)

        private static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                yield return string.Empty;
                yield break;
            }

            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }

        private static IEnumerable<TransactionItem> CreateDepositTransaction(TransferItem transferItem)
        {
            var tag = transferItem.Tag.ValidateTrytes(nameof(transferItem.Tag)).Pad(27);

            var messages = ChunksUpto(transferItem.Message, 2187).ToArray();
            for (int i = 0; i < messages.Length; i++)
            {
                var message = messages[i].ValidateTrytes(nameof(transferItem.Message)).Pad(2187);

                var transactionItem = new TransactionItem()
                {
                    Address = transferItem.Address,
                    SignatureFragment = message,
                    Value = i == 0 ? transferItem.Value : 0,
                    Tag = tag,
                    ObsoleteTag = tag
                };

                yield return transactionItem;
            }
        }

        private static IEnumerable<TransactionItem> CreateWithdrawalTransactions(string tag, long withdrawAmount, string reminderAddress, params AddressItem[] addressItems)
        {
            if (string.IsNullOrWhiteSpace(reminderAddress))
                throw new ArgumentNullException(nameof(reminderAddress));

            var curl = new Crypto.Curl();
            tag = tag.ValidateTrytes(nameof(tag)).Pad(27);

            foreach (var addressItem in addressItems)
            {
                if (addressItem.Balance <= 0)
                    continue;

                var amount = addressItem.Balance;
                withdrawAmount -= amount;

                var transactionItem = new TransactionItem()
                {
                    Address = addressItem.Address,
                    Value = -amount, // withdraw all amount
                    Tag = tag
                };
                yield return transactionItem;

                transactionItem = new TransactionItem()
                {
                    Address = addressItem.Address,
                    Value = 0,
                    Tag = tag
                };
                yield return transactionItem;



                if (withdrawAmount < 0) // deposit remind amount to reminder address
                {
                    var message = "".Pad(2187);

                    transactionItem = new TransactionItem()
                    {
                        Address = reminderAddress,
                        Value = Math.Abs(withdrawAmount),
                        SignatureFragment = message,
                        Tag = tag
                    };
                    yield return transactionItem;
                }

                if (withdrawAmount <= 0)
                    break;
            }
        }

        public static TransactionItem[] CreateTransactions(this TransferItem transferItem)
        {
            return CreateTransactions(transferItem, null);
        }

        public static TransactionItem[] CreateTransactions(this TransferItem transferItem, string remainderAddress, params AddressItem[] fromAddressItems)
        {
            if (fromAddressItems == null)
                fromAddressItems = new AddressItem[] { };

            var totalBalance = fromAddressItems.Sum(f => f.Balance);
            var needBalance = transferItem.Value; //transferItems.Sum(t => t.Value);
            if (needBalance > totalBalance)
                throw new NotEnoughBalanceException(needBalance, totalBalance);

            InputValidator.ValidateTransfer(transferItem);

            var depositTransaction = CreateDepositTransaction(transferItem).ToArray();

            TransactionItem[] withdrawalTransactions = null;
            if (needBalance > 0)
            {
                withdrawalTransactions = CreateWithdrawalTransactions(transferItem.Tag, needBalance, remainderAddress, fromAddressItems)
                    .ToArray();
            }

            var transactions = new List<TransactionItem>();
            transactions.AddRange(depositTransaction);

            if (withdrawalTransactions != null)
                transactions.AddRange(withdrawalTransactions);

            var bundleHash = transactions.FinalizeAndNormalizeBundleHash(new Crypto.Kerl());

            if (withdrawalTransactions != null)
            {
                withdrawalTransactions.SignSignatures(fromAddressItems);
            }

            var transactionsBalance = transactions.Sum(t => t.Value);
            if (transactionsBalance != 0)
                throw new IotaException($"Total transactions balance should be zero. Current is '{transactionsBalance}'. There is some bug in code.");

            return transactions.ToArray();
        }

        public static Task<string[]> DoPow(this string[] trytes, string trunkTransaction, string branchTransaction, int minWeightMagnitude, CancellationToken cancellationToken)
        {
            return trytes.DoPow(trunkTransaction, branchTransaction, minWeightMagnitude, 0, cancellationToken);
        }

        public static async Task<string[]> DoPow(this string[] transactionsTrytes, string trunkTransaction, string branchTransaction, int minWeightMagnitude, int numberOfThreads, CancellationToken cancellationToken)
        {
            var trunk = trunkTransaction;
            var branch = branchTransaction;

            // Recalculate Bundles
            // NOT sure i should do this but hey let's check it in a but.
            var trytes = transactionsTrytes;// ReCalculateAndSetBundles(transactionsTrytes).ToArray();

            List<string> resultTrytes = new List<string>();
            for (int i = 0; i < trytes.Length; i++)
            {
                if (i == 0)
                    branch = branchTransaction;
                else
                    branch = trunkTransaction;

                var tryte = trytes[i];
                tryte = tryte.SetApproveTransactions(trunk, branch);

                var diver = new PowDiver();
                var tryteWithNonce = await diver.DoPow(tryte, minWeightMagnitude, numberOfThreads, cancellationToken);

                //var diver = new PearlDiver();                
                //var tryteWithNonce = diver.DoPow(tryte, minWeightMagnitude);

                var transaction = new TransactionItem(tryteWithNonce);
                trunk = transaction.Hash;

                resultTrytes.Add(tryteWithNonce);
            }

            return resultTrytes.ToArray();
        }

        public static IEnumerable<string> ReCalculateAndSetBundles(this IEnumerable<string> trytes)
        {
            List<TransactionItem> trans = new List<TransactionItem>();
            foreach(string s in trytes)
            {
                var tran = new TransactionItem(s);
                trans.Add(tran);
            }

            //var trans = trytes.Select(s => new TransactionItem(s));

            trans.FinalizeBundleHash(new Crypto.Kerl());

            return trans.Select(s => s.ToTransactionTrytes());
        }

        public static string Pad(this string value, int size)
        {
            return value.Pad('9', size);
        }

        public static string PadLast(this string value, int size)
        {
            value = value ?? "";
            if (string.IsNullOrWhiteSpace(value))
                value = "9";

            var lastSimbol = value[value.Length - 1];
            return value.Pad(lastSimbol, size);
        }

        public static string Pad(this string value, char simbol, int size)
        {
            var pad = value ?? "";
            while (pad.Length < size) pad += simbol;
            return pad;
        }

        public static string SetApproveTransactions(this string trytes, string trunkTransaction, string branchTransaction)
        {
            var trytesConstruct = trytes.Substring(0, 2430);
            trytesConstruct += trunkTransaction;
            trytesConstruct += branchTransaction;

            trytesConstruct += trytes.Substring(2592, trytes.Length - 2592);

            return trytesConstruct;
        }

        public static string SetApproveTrunk(this string trytes, string trunkTransaction)
        {
            var branchTransaction = trytes.GetBranchTransaction();
            var trytesConstruct = trytes.Substring(0, 2430);
            trytesConstruct += trunkTransaction;
            trytesConstruct += branchTransaction;
            trytesConstruct += trytes.Substring(2592, trytes.Length - 2592);

            return trytesConstruct;
        }

        public static string SetApproveBranch(this string trytes, string branchTransaction)
        {
            var trytesConstruct = trytes.Substring(0, 2430 + 81);
            trytesConstruct += branchTransaction;
            trytesConstruct += trytes.Substring(2592, trytes.Length - 2592);            

            return trytesConstruct;
        }

        public static string SetTag(this string trytes, string tag)
        {
            if(trytes.Length < 2619) throw new InvalidTryteException("Tryte length too small.");
            
            var tagToSet = tag.ValidateTrytes(nameof(tag)).Pad(27);

            var trytesConstruct = trytes.Substring(0, 2592);
            trytesConstruct += tagToSet;
            trytesConstruct += trytes.Substring(2619, trytes.Length - 2619);

            return trytesConstruct;
        }

        public static string GetTrunkTransaction(this string trytes)
        {
            return trytes.Substring(2430, 81);
        }

        public static string GetBranchTransaction(this string trytes)
        {
            return trytes.Substring(2430 + 81, 81);
        }

        public static string EmptyHash(int length = 81)
        {
            return new string('9', length);
        }
    }
}
