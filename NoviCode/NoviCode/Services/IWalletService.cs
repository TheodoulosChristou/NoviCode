using NoviCode.Entities;

namespace NoviCode.Services
{
    public interface IWalletService
    {
        public Task<Wallet> CreateWallet(Wallet walletRequest);

        public Task<Wallet> RetrieveWalletBalance(long walletId, string? currency);

        public Task<Wallet> GetWalletByWalletId(long walletId);

        public Task<Wallet> AdjustWalletBalance(long walletId, decimal amount, string currency, string strategy);
    }
}
