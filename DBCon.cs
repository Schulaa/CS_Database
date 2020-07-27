using LiteDB;
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Datenbank
{

    class DBCon
    {
        private static string dbName = @"Database.db";

        #region Read Person (filters)
        public static DataSet PersonFilterMemberType(Person.type memberType)
        {
            DataSet result = new DataSet();
            DataTable table;
            DataColumn[] PrimaryKeyCols = new DataColumn[1];
            DataRow row;

            using (var db = new LiteDatabase(dbName))
            {
                Person pers =new Person();
                table = new DataTable(pers.CollectionName);
                table.Columns.AddRange(pers.dataColumns);
                foreach (DataColumn dmyDc in pers.dataColumns)
                {
                    if (dmyDc.Unique)
                    {
                        PrimaryKeyCols[0] = table.Columns[dmyDc.ColumnName];
                        table.PrimaryKey = PrimaryKeyCols;
                        break;
                    }
                }
                result.Tables.Add(table);

                var col = db.GetCollection<Person>(pers.CollectionName);
                col.EnsureIndex(x => x.memberType);
                var query = col.Query()
                    .Where(x => x.memberType == memberType)
                    .ToList();
                foreach (Person person in query)
                {
                    row = table.NewRow();
                    person.getAsRow(row);
                    table.Rows.Add(row);
                }
            }
            return result;
        }
        public static DataSet PersonFilterPmtType(Person.paymentType pmtType)
        {
            DataSet result = new DataSet();
            DataTable table;
            DataColumn[] PrimaryKeyCols = new DataColumn[1];
            DataRow row;
            Person pers = new Person();

            using (var db = new LiteDatabase(dbName))
            {
                table = new DataTable(pers.CollectionName);
                table.Columns.AddRange(pers.dataColumns);
                foreach (DataColumn dmyDc in pers.dataColumns)
                {
                    if (dmyDc.Unique)
                    {
                        PrimaryKeyCols[0] = table.Columns[dmyDc.ColumnName];
                        table.PrimaryKey = PrimaryKeyCols;
                        break;
                    }
                }
                result.Tables.Add(table);

                var col = db.GetCollection<Person>(pers.CollectionName);
                col.EnsureIndex(x => x.pmtType);
                var query = col.Query()
                    .Where(x => x.pmtType == pmtType)
                    .ToList();
                foreach (Person person in query)
                {
                    row = table.NewRow();
                    person.getAsRow(row);
                    table.Rows.Add(row);
                }
            }
            return result;
        }
        public static DataSet GetPersonDSById(int id)
        {
            Person pers = new Person();
            DataSet result = new DataSet();
            result.DataSetName = pers.CollectionName;
            DataTable table;
            DataColumn[] PrimaryKeyCols = new DataColumn[1];
            DataRow row;

            using (var db = new LiteDatabase(dbName))
            {
                table = new DataTable(pers.CollectionName);
                table.Columns.AddRange(pers.dataColumns);
                foreach (DataColumn dmyDc in pers.dataColumns)
                {
                    if (dmyDc.Unique)
                    {
                        PrimaryKeyCols[0] = table.Columns[dmyDc.ColumnName];
                        table.PrimaryKey = PrimaryKeyCols;
                        break;
                    }
                }
                result.Tables.Add(table);

                var col = db.GetCollection<Person>(pers.CollectionName);
                col.EnsureIndex(x => x.id);
                var query = col.Query()
                    .Where(x => x.id == id)
                    .ToList();
                foreach (Person person in query)
                {
                    row = table.NewRow();
                    person.getAsRow(row);
                    table.Rows.Add(row);
                }
            }
            return result;
        }

        public static int GetNextPersonId()
        {
            using (var db = new LiteDatabase(dbName))
            {
                Person pers = new Person();
                var col = db.GetCollection<Person>(pers.CollectionName);
                col.EnsureIndex(x => x.id);
                Person person = col.Query()
                    .OrderByDescending(x => x.id)
                    .FirstOrDefault();

                return person is null ? 1 : person.id + 1;
            }
        }

        #endregion


    }
    enum mode
    {
        Update,
        Insert,
        Delete
    }
}