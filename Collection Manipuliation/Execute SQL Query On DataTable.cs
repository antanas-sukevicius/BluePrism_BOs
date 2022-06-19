using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace SQLite_Demo
{
    internal class SQLite
    {
        static DataTable ExecuteQuery(string SQLQuery, DataTable inputTable1)
        {
            string connectionString = "Data Source=:memory:";
            string tableName1 = "Input1";
            DataTable outputDataTable = new DataTable();

            string sql_createTable1 = GetCreateTableQuery(tableName1, inputTable1);
            string[] sql_insertQueries1 = GetInsertDataTableQueries(tableName1, inputTable1);

            var cn = new SqliteConnection(connectionString);
            var command = cn.CreateCommand();

            cn.Open();

            //Create Table
            command.CommandText = sql_createTable1;
            command.ExecuteNonQuery();

            for (int i = 0; i < sql_insertQueries1.Length; i++)
            {
                command.CommandText = sql_insertQueries1[i];
                command.ExecuteNonQuery();
            }

            //Execute Query
            command.CommandText = SQLQuery;
            outputDataTable.Load(command.ExecuteReader());

            cn.Close();

            return outputDataTable;
        }


        static DataTable ExecuteQuery(string SQLQuery, DataTable inputTable1, DataTable inputTable2)
        {
            string connectionString = "Data Source=:memory:";
            string tableName1 = "Input1";
            string tableName2 = "Input2";

            DataTable outputDataTable = new DataTable();

            var cn = new SqliteConnection(connectionString);
            var command = cn.CreateCommand();

            cn.Open();

            //Create Tables 1
            string sql_createTable1 = GetCreateTableQuery(tableName1, inputTable1);
            string[] sql_insertQueries1 = GetInsertDataTableQueries(tableName1, inputTable1);

            command.CommandText = sql_createTable1;
            command.ExecuteNonQuery();

            for (int i = 0; i < sql_insertQueries1.Length; i++)
            {
                command.CommandText = sql_insertQueries1[i];
                command.ExecuteNonQuery();
            }


            //Create Tables 2
            string sql_createTable2 = GetCreateTableQuery(tableName2, inputTable2);
            string[] sql_insertQueries2 = GetInsertDataTableQueries(tableName2, inputTable2);

            command.CommandText = sql_createTable2;
            command.ExecuteNonQuery();

            for (int i = 0; i < sql_insertQueries2.Length; i++)
            {
                command.CommandText = sql_insertQueries2[i];
                command.ExecuteNonQuery();
            }

            //Execute Query
            command.CommandText = SQLQuery;
            outputDataTable.Load(command.ExecuteReader());

            cn.Close();

            return outputDataTable;
        }


        static string[] GetInsertDataTableQueries(string tableName, DataTable dt)
        {
            string[] columnsNamesArray = new string[dt.Columns.Count];
            string columnsNames;
            string columnsValues;
            string columnName;
            string columnValue;
            string sqlQuery;
            string[] sqlInsterQueries = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                columnsNamesArray[i] = dt.Columns[i].ColumnName;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                columnsNames = "";
                columnsValues = "";
                for (int j = 0; j < columnsNamesArray.Length; j++)
                {
                    columnName = columnsNamesArray[j].ToString();
                    columnValue = dt.Rows[i][columnsNamesArray[j]].ToString();

                    columnsValues = columnsValues + "'" + columnValue + "',";
                    columnsNames = columnsNames + columnName + ",";

                }

                columnsValues = columnsValues.Remove(columnsValues.Length - 1);
                columnsNames = columnsNames.Remove(columnsNames.Length - 1);

                sqlQuery = String.Format(@"INSERT INTO {0} ({1}) VALUES({2});", tableName, columnsNames, columnsValues);
                sqlInsterQueries[i] = sqlQuery;
            }


            return sqlInsterQueries;
        }

        static string GetCreateTableQuery(string tableName, DataTable table)
        {
            string sqlsc;
            sqlsc = "CREATE TABLE " + tableName + " (";

            for (int i = 0; i < table.Columns.Count; i++)
            {
                sqlsc += "\n [" + table.Columns[i].ColumnName + "] ";
                string columnType = table.Columns[i].DataType.ToString();
                switch (columnType)

                {
                    case "System.Int32":
                        sqlsc += " int ";
                        break;

                    case "System.Int64":
                        sqlsc += " bigint ";
                        break;

                    case "System.Int16":
                        sqlsc += " smallint";
                        break;

                    case "System.Byte":
                        sqlsc += " tinyint";
                        break;

                    case "System.Decimal":
                        sqlsc += " decimal ";
                        break;

                    case "System.DateTime":
                        sqlsc += " datetime ";
                        break;

                    case "System.String":

                    default:

                        sqlsc += string.Format(" nvarchar({0}) ", table.Columns[i].MaxLength == -1 ? "4000" : table.Columns[i].MaxLength.ToString());
                        break;

                }

                if (table.Columns[i].AutoIncrement)
                    sqlsc += " IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ") ";

                if (!table.Columns[i].AllowDBNull)
                    sqlsc += " NOT NULL ";
                sqlsc += ",";

            }

            return sqlsc.Substring(0, sqlsc.Length - 1) + "\n)";

        }
    }
}