using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Configuration;
using System;

public class Persistence
{
    private string sqlServerConnectionString = "";

    public Persistence() {
    }

    public List<string> readOrdini() {
        IDbConnection conn = null;
        IDataReader reader = null;

        try {
            conn = Config.Instance.getDatabaseConnection();
            conn.Open();

            IDbCommand command = conn.CreateCommand();

            string query = "SELECT * FROM ordini";
            command.CommandText = query;

            reader = command.ExecuteReader();
            
            List<string> data = new List<string>();
            
            while (reader.Read()){      
                string customer = reader["id"] + " " + reader["customer"];
                data.Add(customer);
            }

            return data;
        }
        catch (Exception ex) {
            List<string> errors = new List<string>();
            errors.Add(ex.ToString());

            return errors;
        } finally {
            if (reader != null) reader.Close();
            if (reader != null) conn.Close();
        }
    }

    public List<string> readOrdiniByID(string idCustomer) {
        IDbConnection conn = null;
        IDataReader reader = null;

        try {
            conn = Config.Instance.getDatabaseConnection();
            conn.Open();

            IDbCommand command = conn.CreateCommand();

            string query = "SELECT * FROM ordini WHERE customer = '" + idCustomer + "' LIMIT 100";
            command.CommandText = query;

            reader = command.ExecuteReader();
            
            List<string> data = new List<string>();
            
            while (reader.Read()){      
                string customer = reader["id"] + " " + reader["customer"];
                data.Add(customer);
            }

            return data;
        }
        catch (Exception ex) {
            List<string> errors = new List<string>();
            errors.Add(ex.ToString());

            return errors;
        } finally {
            if (reader != null) reader.Close();
            if (reader != null) conn.Close();
        }
    }

}