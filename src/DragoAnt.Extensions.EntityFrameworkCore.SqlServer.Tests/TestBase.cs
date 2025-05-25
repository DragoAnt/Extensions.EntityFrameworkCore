namespace DragoAnt.Extensions.EntityFrameworkCore.SqlServer.Tests;

public class TestBase
{
#if NET7_0
        protected const string DbName = "stenn_efcore_tests_net7";
#elif NET8_0
        protected const string DbName = "stenn_efcore_tests_net8";
#elif NET9_0
    protected const string DbName = "stenn_efcore_tests_net9";
#elif NET10_0
        protected const string DbName = "stenn_efcore_tests_net10";
#elif NET11_0
        protected const string DbName = "stenn_efcore_tests_net11";
#elif NET12_0
        protected const string DbName = "stenn_efcore_tests_net12";
#elif NET13_0
        protected const string DbName = "stenn_efcore_tests_net13";
#elif NET14_0
        protected const string DbName = "stenn_efcore_tests_net14";
#endif

    protected static string GetConnectionString(string dbName)
    {
        return $@"Data Source=.\SQLEXPRESS;Initial Catalog={dbName};Integrated Security=SSPI;Encrypt=False";
    }
}