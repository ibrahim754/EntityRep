namespace MyProject;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Data.SqlClient;
using System.Data;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


        var conn = new SqlConnection(config.GetSection("constr").Value);

        conn.Open();
        PrintAllTable();
        conn.Close();

         
    }

    public static void PrintAllTable()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


        var conn = new SqlConnection(config.GetSection("constr").Value);
        conn.Open();

        var sql = "Select * from WALLETS";

        var command = new SqlCommand(sql, conn);

        command.CommandType = System.Data.CommandType.Text;

        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Wallet wallet = new Wallet
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Holder = reader.GetString(reader.GetOrdinal("Holder")),
                Balance = reader.GetDecimal(reader.GetOrdinal("Balance"))

            };
            Console.WriteLine(wallet);
        }
    }
    public void insertion()
    {
        // i have made error, i passed wrong serverName, it took me 2 hours to find the error ^_^;
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


        var conn = new SqlConnection(config.GetSection("constr").Value);
        conn.Open();

        string name = Console.ReadLine();
        decimal balance = Convert.ToDecimal(Console.ReadLine());
        Wallet wallet1 = new Wallet
        {
            Holder = name,
            Balance = balance
        };

        var sqlInsert = "Insert into WALLETS  (Holder,Balance) values " + $"(@Holder,@Balance)";
        SqlParameter par1 = new SqlParameter
        {
            ParameterName = "@Holder",
            SqlDbType = System.Data.SqlDbType.VarChar,
            Direction = System.Data.ParameterDirection.Input,
            Value = wallet1.Holder
        };
        SqlParameter par2 = new SqlParameter
        {
            ParameterName = "@Balance",
            SqlDbType = System.Data.SqlDbType.Decimal,
            Direction = System.Data.ParameterDirection.Input,
            Value = wallet1.Balance
        };

        SqlCommand insertCommand = new SqlCommand(sqlInsert, conn);
        insertCommand.Parameters.Add(par1);
        insertCommand.Parameters.Add(par2);
        insertCommand.CommandType = CommandType.Text;

        if (insertCommand.ExecuteNonQuery() > 0)
        {
            Console.WriteLine("Added successfully");

        }
        else
        {
            Console.WriteLine("Couldnot be added");
        }
    }
    public class Wallet
    {
        public int Id { get; set; }
        public string? Holder { get; set; }
        public decimal? Balance { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
    }
}