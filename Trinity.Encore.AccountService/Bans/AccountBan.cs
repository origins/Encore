using System;
using System.Diagnostics.Contracts;
using Trinity.Core.Runtime.Serialization;
using Trinity.Encore.AccountService.Accounts;
using Trinity.Encore.AccountService.Database;
using Trinity.Encore.Services.Account;

namespace Trinity.Encore.AccountService.Bans
{
    public sealed class AccountBan : IMemberwiseSerializable<AccountBanData>
    {
        public AccountBan(AccountBanRecord record)
        {
            Contract.Requires(record != null);

            Record = record;
        }

        /// <summary>
        /// Deletes the AccountBan from the backing storage. This object is considered invalid once
        /// this method has been executed.
        /// </summary>
        public void Delete()
        {
            Record.Delete();
        }

        public AccountBanData Serialize()
        {
            Contract.Ensures(Contract.Result<AccountBanData>() != null);

            return new AccountBanData
            {
                AccountId = Account.Id,
                Notes = Notes,
                Expiry = Expiry,
            };
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Record != null);
        }

        /// <summary>
        /// Gets the underlying record of this AccountBan. Should not be manipulated
        /// directly.
        /// </summary>
        public AccountBanRecord Record { get; private set; }

        public long Id
        {
            get { return Record.Id; }
        }

        public Accounts.Account Account
        {
            get
            {
                Contract.Ensures(Contract.Result<Accounts.Account>() != null);

                var acc = AccountManager.Instance.FindAccount(x => x.Id == Record.Id);
                Contract.Assume(acc != null);
                return acc;
            }
        }

        public string Notes
        {
            get { return Record.Notes; }
            set
            {
                Record.Notes = value;
                Record.Update();
            }
        }

        public DateTime? Expiry
        {
            get { return Record.Expiry; }
            set
            {
                Record.Expiry = value;
                Record.Update();
            }
        }
    }
}
