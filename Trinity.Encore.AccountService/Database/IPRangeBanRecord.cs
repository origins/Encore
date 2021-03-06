using System;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Encore.AccountService.Bans;
using Trinity.Encore.AccountService.Database.Implementation;
using Trinity.Network;
using Trinity.Persistence.Mapping;

namespace Trinity.Encore.AccountService.Database
{
    public class IPRangeBanRecord : AccountDatabaseRecord
    {
        /// <summary>
        /// Constructs a new IPRangeBanRecord object.
        /// 
        /// Should be used only by the underlying database layer.
        /// </summary>
        protected IPRangeBanRecord()
        {
        }

        /// <summary>
        /// Constructs a new IPRangeBanRecord object.
        /// 
        /// Should be inserted into the database.
        /// </summary>
        /// <param name="lowerAddress"></param>
        /// <param name="upperAddress"></param>
        public IPRangeBanRecord(byte[] lowerAddress, byte[] upperAddress)
        {
            Contract.Requires(lowerAddress != null);
            Contract.Requires(upperAddress != null);

            LowerAddress = lowerAddress;
            UpperAddress = upperAddress;
        }

        public virtual long Id { get; protected /*private*/ set; }

        public virtual byte[] LowerAddress { get; protected /*private*/ set; }

        public virtual byte[] UpperAddress { get; protected /*private*/ set; }

        public virtual string Notes { get; set; }

        public virtual DateTime? Expiry { get; set; }
    }

    public sealed class IPRangeBanMapping : MappableObject<IPRangeBanRecord>
    {
        public IPRangeBanMapping()
        {
            Id(c => c.Id);
            Map(c => c.LowerAddress).Length(IPAddress.IPv6Loopback.GetLength());
            Map(c => c.UpperAddress).Length(IPAddress.IPv6Loopback.GetLength());
            Map(c => c.Notes).Nullable().Length(BanManager.MaxNotesLength);
            Map(c => c.Expiry).Nullable();
        }
    }
}
