using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Identity.Client;
using NoviCode.Controllers;
using NoviCode.DTO;
using NoviCode.Entities;

namespace NoviCode.Services
{
    public class WalletService : IWalletService
    {
        private readonly NoviCodeDbContext.NoviCodeDbContext _dbContext;

        private readonly ICurrencyRateService _currencyRateService;

        public WalletService(NoviCodeDbContext.NoviCodeDbContext dbContext, ICurrencyRateService currencyRateService)
        {
            _dbContext = dbContext;
            _currencyRateService = currencyRateService;
        }


        public async Task<Wallet> CreateWallet(Wallet walletRequest)
        {
            try
            {
                if (walletRequest == null)
                {
                    throw new Exception("Wallet Object is null please check your values again");
                }

                if (walletRequest.Currency == null)
                {
                    throw new Exception("Wallet Currency is null. Please asign a value to it");
                }

                if (walletRequest.Balance <= 0)
                {
                    throw new Exception("Wallet Balance must greater than zero");
                }

                _dbContext.Wallet.Add(walletRequest);
                _dbContext.SaveChanges();
                return walletRequest;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Wallet> GetWalletByWalletId(long walletId)
        {
            try
            {
                var result = _dbContext.Wallet.FirstOrDefault(x => x.Id == walletId);
                if (result == null)
                {
                    return null;
                }
                else
                {
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Wallet> RetrieveWalletBalance(long walletId, string? currency)
        {
            if (walletId == null)
            {
                throw new Exception("WalletId is required");
            }

            var wallet = await this.GetWalletByWalletId(walletId);

            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }

            if (currency == null)
            {
                return wallet;
            }
            else
            {
                var fromDate = await _currencyRateService.GetLatestRateByCurrencyAsync(wallet.Currency);
                var toRate = await _currencyRateService.GetLatestRateByCurrencyAsync(currency);

                if (fromDate == null || toRate == null)
                {
                    throw new Exception("Currency rate not found");
                }
                else
                {
                    var converted = (wallet.Balance / fromDate.Rate) * toRate.Rate;

                    return new Wallet
                    {
                        Id = wallet.Id,
                        Balance = Math.Round(converted, 2),
                        Currency = currency
                    };
                }
            }
        }

        public async Task<Wallet> AdjustWalletBalance(long walletId, decimal amount, string currency, string strategy)
        {

            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be a positive number");
            }

            var wallet = await this.GetWalletByWalletId(walletId);

            if (wallet.Currency != currency)
            {
                var fromRate = await _currencyRateService.GetLatestRateByCurrencyAsync(wallet.Currency);
                var toRate = await _currencyRateService.GetLatestRateByCurrencyAsync(currency);

                if (fromRate == null || toRate == null)
                {
                    throw new Exception("Currency rate not found");
                }
                else
                {
                    amount = (amount / fromRate.Rate) * toRate.Rate;
                }
            }

            switch (strategy)
            {
                case WalletStrategiesDTO.Add:
                    wallet.Balance = wallet.Balance + amount;
                    break;

                case WalletStrategiesDTO.Substract:
                    if (wallet.Balance <= amount)
                    {
                        throw new Exception("Insufficient funds");
                    }
                    else
                    {
                        wallet.Balance = wallet.Balance - amount;
                        break;
                    }
                case WalletStrategiesDTO.ForceSubstract:
                    wallet.Balance = wallet.Balance - amount;
                    break;

                default:
                    throw new Exception("Invalid strategy");
            }

            await _dbContext.SaveChangesAsync();
            return wallet;

        }
    }
}
