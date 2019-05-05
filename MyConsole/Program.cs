using System;
using Autofac;
using Autofac.Extras.DynamicProxy;
using MyConsole.Interceptors;
using MyWalletLib;
using MyWalletLib.Models;

namespace MyConsole
{
    internal class FakeBankingAdapter : IBanking
    {
        public void Deposit(string bankingAccount, decimal amount)
        {
            Console.WriteLine($"{nameof(Deposit)}({bankingAccount}, {amount})");
        }

        public void Withdraw(string bankingAccount, decimal amount)
        {
            Console.WriteLine($"{nameof(Withdraw)}({bankingAccount}, {amount})");
        }
    }

    internal class FakeContext : IContext
    {
        public Account GetCurrentUser()
        {
            return new Account { Id = "test", UserType = UserType.Guest };
        }
    }

    internal class FakeFeeAdapter : IFee
    {
        public decimal GetFee(string account)
        {
            Console.WriteLine($"{nameof(GetFee)}({account})");
            return 10;
        }
    }

    internal class FakeLogger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine($"[Log] {message}");
        }
    }

    internal class FakeWalletRepo : IWalletRepo
    {
        public void UpdateDelta(string account, decimal amount)
        {
            Console.WriteLine($"{nameof(UpdateDelta)}({account}, {amount})");
        }
    }

    internal class Program
    {
        private static IContainer _container;

        private static void Main(string[] args)
        {
            //var fakeWalletRepo = new FakeWalletRepo();
            //var fakeBankingAdapter = new FakeBankingAdapter();
            //var fakeFeeAdapter = new FakeFeeAdapter();
            //var fakeLogger = new FakeLogger();

            //var wallet = new Wallet(fakeWalletRepo, fakeBankingAdapter, fakeFeeAdapter);
            //var loggerWallet = new LoggerDecorator(wallet, fakeLogger);

            RegisterContainer();

            var wallet = _container.Resolve<IWallet>();

            wallet.Deposit("joey", 1000, "123456789");
            Console.WriteLine(new string('-', 50));
            wallet.Withdraw("joey", 1000, "123456789");
        }

        private static void RegisterContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FakeWalletRepo>().As<IWalletRepo>();
            builder.RegisterType<FakeBankingAdapter>().As<IBanking>();
            builder.RegisterType<FakeFeeAdapter>().As<IFee>();
            builder.RegisterType<FakeLogger>().As<ILogger>();
            builder.RegisterType<FakeContext>().As<IContext>();

            //builder.RegisterType<Wallet>().As<IWallet>();
            //builder.RegisterDecorator<LoggerDecorator, IWallet>();

            //builder.RegisterType<LogInterceptorInLib>();
            //builder.RegisterType<Wallet>()
            //    .As<IWallet>()
            //    .EnableInterfaceInterceptors();

            builder.RegisterType<LogInterceptor>();
            builder.RegisterType<AuthorizationInterceptor>();
            builder.RegisterType<Wallet>()
                .As<IWallet>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(
                    typeof(LogInterceptor),
                    typeof(AuthorizationInterceptor));

            _container = builder.Build();
        }
    }
}