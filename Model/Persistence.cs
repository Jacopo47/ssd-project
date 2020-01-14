using System;
using System.Collections.Generic;
using System.Data;
using ssdProject.Model;

public class Persistence
{
    private string sqlServerConnectionString = "";

    // Reads an instance from the db

    public void readGAPinstance(GAPClass G)
    {
        int i, j;
        List<int> lstCap = new List<int>();
        List<double> lstCosts = new List<double>();
        
        IDbConnection conn = null;
        IDataReader reader = null;
        IDataReader readerCosti = null;

        try
        {
            conn = Config.Instance.getDatabaseConnection();
            conn.Open();

            var command = conn.CreateCommand();

            var query = "SELECT cap from capacita";
            command.CommandText = query;

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                lstCap.Add(Int32.Parse(reader["cap"].ToString()));
            }

            G.m = lstCap.Count;
            G.cap = new int[G.m];
            for (i = 0; i < G.m; i++)
                G.cap[i] = lstCap[i];

            var commandCost = conn.CreateCommand();
            
            query = "SELECT cost from costi";
            commandCost.CommandText = query;

            readerCosti = commandCost.ExecuteReader();

            while (readerCosti.Read())
            {
                lstCosts.Add(Double.Parse(readerCosti["cost"].ToString()));
            }
            
            G.n = lstCosts.Count / G.m;
            G.c = new double[G.m, G.n];
            G.req = new int[G.n];
            G.sol = new int[G.n];
            G.solbest = new int[G.n];
            G.zub = double.MaxValue;
            G.zlb = double.MinValue;

            for (i = 0; i < G.m; i++)
            for (j = 0; j < G.n; j++)
                G.c[i, j] = lstCosts[i * G.n + j];

            for (j = 0; j < G.n; j++)
                G.req[j] = -1; // placeholder
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            reader?.Close();
            readerCosti?.Close();
            conn?.Close();
        }

        Console.WriteLine("Fine lettura dati istanza GAP");
    }

    public List<string> readOrdini()
    {
        IDbConnection conn = null;
        IDataReader reader = null;

        try
        {
            conn = Config.Instance.getDatabaseConnection();
            conn.Open();

            var command = conn.CreateCommand();

            var query = "SELECT * FROM ordini";
            command.CommandText = query;

            reader = command.ExecuteReader();

            var data = new List<string>();

            while (reader.Read())
            {
                var customer = reader["id"] + " " + reader["customer"];
                data.Add(customer);
            }

            return data;
        }
        catch (Exception ex)
        {
            var errors = new List<string> {ex.ToString()};

            return errors;
        }
        finally
        {
            reader?.Close();
            conn?.Close();
        }
    }

    public List<string> readOrdiniByID(string idCustomer)
    {
        IDbConnection conn = null;
        IDataReader reader = null;

        try
        {
            conn = Config.Instance.getDatabaseConnection();
            conn.Open();

            var command = conn.CreateCommand();

            var query = "SELECT * FROM ordini WHERE customer = '" + idCustomer + "' LIMIT 100";
            command.CommandText = query;

            reader = command.ExecuteReader();

            var data = new List<string>();

            while (reader.Read())
            {
                var customer = reader["id"] + " " + reader["customer"];
                data.Add(customer);
            }

            return data;
        }
        catch (Exception ex)
        {
            var errors = new List<string>();
            errors.Add(ex.ToString());

            return errors;
        }
        finally
        {
            reader?.Close();
            conn?.Close();
        }
    }
}
