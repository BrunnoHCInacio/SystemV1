using Npgsql;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace SystemV1.Infrastructure.Test
{
    public class AccessDataBaseTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConnectionDataBase()
        {
            string startupPath = Path.Combine(
                                    Path.Combine(
                                    Directory.GetParent(System.IO.Directory.GetCurrentDirectory())
                                             .Parent
                                             .Parent
                                             .Parent
                                             .FullName,
                                            "SystemV1.API2"),
                                    "appsettings.json"
                                    );
            var appSettings = File.ReadAllText(startupPath);

            var connection = JsonSerializer.Deserialize<AppSettings>(appSettings);
            NpgsqlConnection conn = new NpgsqlConnection(connection?.SqlConnection?.SqlConnectionString ?? "");
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {
                Assert.Pass();
            }

            conn.Close();
        }
    }

    public class SqlConnection
    {
        public string SqlConnectionString { get; set; }
    }

    public class AppSettings
    {
        public SqlConnection SqlConnection { get; set; }
    }
}