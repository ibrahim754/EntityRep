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
         SqlDataAdapter adp = new SqlDataAdapter();

       

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
        Wallet wallet1 = GetInput();
    

        var sqlInsert = "Insert into WALLETS  (Holder,Balance) values " 
            + $"(@Holder,@Balance)";
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
    public static void InsertUsingExcuteSclar()
    {

        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var conn = new SqlConnection(configuration.GetSection("constr").Value);

        string sql = "Insert into WALLETS (Holder,Balance) Values "
            + $" (@Holder,@Balance); " // to prevent SQL injection;
            + $" Select Cast (scope_identity() as int )"; // to return  the id;
        var wallet = GetInput();

        var HolderParmeter = new SqlParameter
        {
            ParameterName = "@Holder",
            SqlDbType = SqlDbType.VarChar,
            Direction = ParameterDirection.Input,
            Value = wallet.Holder
        };
        var BalanceParameter = new SqlParameter
        {
            ParameterName = "@Balance",
            SqlDbType = SqlDbType.Decimal,
            Direction = ParameterDirection.Input,
            Value = wallet.Balance
        };

        conn.Open();
        var sqlCommand = new SqlCommand(sql,conn);
        sqlCommand.Parameters.Add(HolderParmeter);
        sqlCommand.Parameters.Add(BalanceParameter);
        wallet.Id =  (int )sqlCommand.ExecuteScalar(); // return the first column of the first row
        Console.WriteLine(wallet.ToString());

        conn.Close();
        
    }
    public static Wallet GetInput()
    {
        Console.Write("Enter the HolderName: ");
        string name = Console.ReadLine();
        Console.Write("\nEnter the BalanceName: ");
        decimal balance = Convert.ToDecimal(Console.ReadLine());
        Wallet wallet = new Wallet
        {
            Holder = name,
            Balance = balance
        };
        return wallet;
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