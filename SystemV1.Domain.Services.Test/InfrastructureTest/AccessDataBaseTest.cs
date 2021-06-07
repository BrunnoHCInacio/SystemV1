using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SystemV1.Domain.Test.InfrastructureTest
{
    public class AccessDataBaseTest
    {
        [Fact(DisplayName = "Validar conexão com a base de dados")]
        public void Connection_ValidateConnection_ReturnConnectionHasOpen()
        {
            //Arrange
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

            //Act
            conn.Open();

            //Assert
            Assert.True(conn.State == System.Data.ConnectionState.Open);

            conn.Close();
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
}