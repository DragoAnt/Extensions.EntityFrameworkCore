namespace DragoAnt.EntityFrameworkCore.SqlServer.Tests;

public class TestBase
{
#if NET7_0
        private const string DbName = "efcore_tests_net7";
#elif NET8_0
        private const string DbName = "efcore_tests_net8";
#elif NET9_0
    private const string DbName = "efcore_tests_net9";
#elif NET10_0
        private const string DbName = "efcore_tests_net10";
#elif NET11_0
        private const string DbName = "efcore_tests_net11";
#elif NET12_0
        private const string DbName = "efcore_tests_net12";
#elif NET13_0
        private const string DbName = "efcore_tests_net13";
#elif NET14_0
        private const string DbName = "efcore_tests_net14";
#endif
        protected static string GenerateUniqueDbName() => $"{DbName}_{Guid.NewGuid():N}";
        
    protected static string GetConnectionString(string dbName)
        => $"Server=localhost,14330;Database={dbName};User Id=sa;Password=SqlQWERTY123;; TrustServerCertificate=True;";
    //      => $@"Data Source=.\SQLEXPRESS;Initial Catalog={dbName};Integrated Security=SSPI;Encrypt=False";
}