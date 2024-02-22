namespace MyProject;
using Microsoft.Extensions.Configuration.Json;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.Intrinsics.Arm;

class Program
{
        

    static void Main(string[] args)
    {
        selectSql();
        Console.WriteLine("-------------------------------------");
        delete();
        Console.WriteLine("-------------------------------------");

        selectSql();

    }
    public static void delete()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        IDbConnection db = new SqlConnection(config.GetSection("constr").Value);
        var sql = "Delete from Wallets where Id = @id";

        var parms = new { id = 4 };
        db.Execute(sql, parms);
    } 
    public static void update()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        IDbConnection db = new SqlConnection(config.GetSection("constr").Value);
        var sql = "Update Wallets set Balance = (@balance) " +
            " Where Id = @id ;";
        Wallet wallet = getWallet();
        wallet.Id = 4;
        var parms = new { balance = wallet.Balance,id = wallet.Id };
        db.Execute(sql,parms);


    }
    public static void insert()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        IDbConnection db = new SqlConnection(config.GetSection("constr").Value);
        var insertSql = "Insert Into WALLETS (Holder,Balance) " +
                        "Values (@Holder,@Balance);" +
                        " Select CAST(SCOPE_IDENTITY()  AS INT ;";
        var sql = "INSERT INTO Wallets (Holder, Balance) " +
          "VALUES (@Holder, @Balance);" + // Note the semicolon here
          "SELECT CAST(SCOPE_IDENTITY() AS INT)";

        Wallet wallet = getWallet();
        var parms = new { Holder = wallet.Holder, Balance = wallet.Balance };
        var res = db.Execute(sql, parms); // don not return any rows, return number of rows Affected;
        wallet.Id = db.Query<int>(sql, parms).Single();  // used to get return Select value
        Console.WriteLine(wallet);


    }
    public static void selectSql()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        IDbConnection db = new SqlConnection(config.GetSection("constr").Value);
      
        var sql = " Select * from WALLETS ";
        var dynamicResults = db.Query(sql);
        foreach (var row in dynamicResults)
        {
            Console.WriteLine(row);
        }
        var wallets = db.Query<Wallet>(sql);
        foreach (var wallet in wallets)
        {
            Console.WriteLine(wallet);
        }

    }
    public static Wallet getWallet()
    {
        Wallet wallet = new Wallet();
        Console.Write("Enter Holder Name: ");
        wallet.Holder = Console.ReadLine();
        Console.Write("\nEnter Balance Amount: ");
        wallet.Balance = Convert.ToDecimal(Console.ReadLine());
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