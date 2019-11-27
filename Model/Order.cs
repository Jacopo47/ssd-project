using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;
using System;
class Order {
    public string id;
    public string customer;
    public string time;
    public double quant;


    private static string sqLiteConnectionString = @"Data Source=ordiniMI2018.sqlite; Version=3";


    public Order(string id, string customer, string time, double quant) {
        this.id = id;
        this.customer = customer;
        this.time = time; 
        this.quant = quant;
    }

    public Order(string id) {
        Order app = Order.select(id);

        if (app != null) {
            this.id = app.id;
            this.customer = app.customer;
            this.time = app.time;
            this.quant = app.quant;
        }
    }

    public static Order select(string id) {
        IDbConnection conn = null;
        IDataReader reader = null;

        try {
            conn = Config.Instance.getDatabaseConnection();
            conn.Open();

            IDbCommand command = conn.CreateCommand();

            string query = "SELECT * FROM ordini WHERE id = '" + id + "'";
            command.CommandText = query;

            reader = command.ExecuteReader();
            
            
            reader.Read(); 
            
            return new Order(reader["id"].ToString(), reader["customer"].ToString(), reader["time"].ToString(), Convert.ToDouble(reader["quant"]));
        } finally {
            if (reader != null) reader.Close();
            if (conn != null) conn.Close();
        }
    }

    public void delete() {
        IDbConnection conn = null;

        try {
            conn = Config.Instance.getDatabaseConnection();
            conn.Open();

            IDbCommand command = conn.CreateCommand();

            string query = "DELETE FROM Ordini WHERE id = " + this.id;
            command.CommandText = query;

            command.ExecuteNonQuery();
        } finally {
            if (conn != null) conn.Close();
        }
    }

    public void update() {
        IDbConnection conn = null;

        try {
            conn = Config.Instance.getDatabaseConnection();
            conn.Open();

            IDbCommand command = conn.CreateCommand();

            string query = "UPDATE Ordini SET customer = '" + this.customer + "', time = " + this.time + ", quant = " + this.quant + " WHERE id = " + this.id;
            command.CommandText = query;

            command.ExecuteNonQuery();
            
        } finally {
            if (conn != null) conn.Close();
        }
    }

    public void create() {
        IDbConnection conn = null;
    
        try {
            conn = Config.Instance.getDatabaseConnection();
            conn.Open();

            IDbCommand command = conn.CreateCommand();

            string query = "INSERT INTO ordini (id, customer, time, quant) VALUES ((SELECT id + 1 FROM ordini ORDER BY id DESC LIMIT 1), '" + this.customer + "', " + this.time + ", " + this.quant +")";
            command.CommandText = query;

            command.ExecuteNonQuery();
        } finally {
            if (conn != null) conn.Close();
        }
    }

    public override string ToString() {
        return this.id + " / " + this.customer + " / " + this.time + " / " + this.quant;
    }
}