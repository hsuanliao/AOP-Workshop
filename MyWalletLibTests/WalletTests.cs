using MyWalletLib;
using MyWalletLib.Models;
using NSubstitute;
using NUnit.Framework;

namespace MyWalletLibTests
{
    [TestFixture]
    public class WalletTests
    {
        private IBanking _bankingAdapter;
        private IFee _feeAdapter;
        private Wallet _wallet;
        private IWalletRepo _walletRepo;

        [Test]
        public void deposit_from_banking_account_to_wallet_successfully()
        {
            _wallet.Deposit("123456789", 1000m, "joey");

            _bankingAdapter.Received(1).Deposit("123456789", 1000);
            _walletRepo.Received(1).UpdateDelta("joey", 1000);
        }

        [SetUp]
        public void Setup()
        {
            _walletRepo = Substitute.For<IWalletRepo>();
            _bankingAdapter = Substitute.For<IBanking>();
            _feeAdapter = Substitute.For<IFee>();

            _wallet = new Wallet(_walletRepo, _bankingAdapter, _feeAdapter);
        }

        [Test]
        public void withdrawal_from_wallet_to_banking_account_successfully()
        {
            _feeAdapter.GetFee("joey").ReturnsForAnyArgs(99);

            _wallet.Withdraw("joey", 1000m, "123456789");

            _walletRepo.Received(1).UpdateDelta("joey", -1000);
            _bankingAdapter.Received(1).Withdraw("123456789", 901);
        }
    }
}