using System;
using System.Transactions;
using Autofac;
using Autofac.Extras.DynamicProxy;
using MyConsole.Interceptors;
using MyWalletLib;
using MyWalletLib.Models;

namespace MyConsole
{
    public class TransactionScopeAdapter : ITransactionScope
    {
        private readonly TransactionScope _transactionScope;

        public TransactionScopeAdapter()
        {
            _transactionScope = new TransactionScope();
        }

        public void Complete()
        {
            Console.WriteLine("transaction complete");
            _transactionScope.Complete();
        }

        public void Dispose()
        {
            Console.WriteLine("transaction dispose");
            _transactionScope.Dispose();
        }
    }

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

    internal class FakeNotification : INotification
    {
        public void Push(Role role, string message)
        {
            Console.WriteLine($"{role}, transaction ex:{message}");
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
        public static IContainer _container;

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

            Console.WriteLine(wallet.CreateGuid("Joey", 91));
            Console.WriteLine(wallet.CreateGuid("Joey", 91));
            Console.WriteLine(wallet.CreateGuid("Tom", 66));
            Console.WriteLine(wallet.CreateGuid("Joey", 91));
            Console.WriteLine(new string('-', 50));

            wallet.Deposit("joey", 1000, "123456789");
            Console.WriteLine(new string('-', 50));

            wallet.Withdraw("joey", 1000, "123456789");
            Console.WriteLine(new string('-', 50));

            wallet.Withdraw("transaction error", 1000, "123456789");
        }

        private static void RegisterContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FakeWalletRepo>().As<IWalletRepo>();
            builder.RegisterType<FakeBankingAdapter>().As<IBanking>();
            builder.RegisterType<FakeFeeAdapter>().As<IFee>();
            builder.RegisterType<FakeLogger>().As<ILogger>();
            builder.RegisterType<FakeContext>().As<IContext>();
            builder.RegisterType<MemoryCacheProvider>().As<ICacheProvider>();
            builder.RegisterType<TransactionScopeAdapter>().As<ITransactionScope>();
            builder.RegisterType<FakeNotification>().As<INotification>();

            //builder.RegisterType<Wallet>().As<IWallet>();
            //builder.RegisterDecorator<LoggerDecorator, IWallet>();

            //builder.RegisterType<LogInterceptorInLib>();
            //builder.RegisterType<Wallet>()
            //    .As<IWallet>()
            //    .EnableInterfaceInterceptors();

            builder.RegisterType<LogInterceptor>();
            builder.RegisterType<AuthorizationInterceptor>();
            builder.RegisterType<CacheInterceptor>();
            builder.RegisterType<TransactionInterceptor>();
            builder.RegisterType<Wallet>()
                .As<IWallet>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(
                    typeof(LogInterceptor),
                    typeof(AuthorizationInterceptor),
                    typeof(CacheInterceptor),
                    typeof(TransactionInterceptor));

            _container = builder.Build();
        }
    }
}