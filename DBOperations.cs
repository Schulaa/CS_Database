using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LiteDB;

namespace Datenbank
{
    class DBOperations
    {
        private static string dbName = @"Database.db";

        public static void Upsert<K>(DBObject obj)
        {
            using (var db = new LiteDatabase(dbName))
            {
                var col = db.GetCollection<K>(obj.CollectionName);
                col.Upsert((K)obj);
            }
        }
        public static List<K> GetAllRecords<K>(DBObject obj)
        {
            using (var db = new LiteDatabase(dbName))
            {

                var col = db.GetCollection<K>(obj.CollectionName);
                var rec = col.FindAll();

                return rec.ToList();
            }
        }
        public static void DeleteRecord<K>(DBObject obj)
        {
            using (var db = new LiteDatabase(dbName))
            {
                var col = db.GetCollection<K>(obj.CollectionName);
                var rec = col.Delete(obj.id);
            }
        }
        public static DBObject UpdatePrimaryKey<K>(DBObject oldObj, int newId)
        {
            DeleteRecord<K>(oldObj);
            oldObj.id = newId;
            Upsert<K>(oldObj);
            return oldObj;
        }
        public static DBObject GetRecById<K>(int id, DBObject obj)
        {
            using (var db = new LiteDatabase(dbName))
            {
                var col = db.GetCollection<K>(obj.CollectionName);
                var query = col.Query()
                            .Where(x=>x.)
            }
        }
        public static DataSet GetDataSet<K>(DBObject obj)
        {
            DataSet retVal = new DataSet();
            DataTable table = new DataTable(obj.CollectionName);
            DataColumn[] PrimaryKeyCols = new DataColumn[1];
            DataRow row;

            table.Columns.AddRange(obj.dataColumns);

            string colName = obj.dataColumns.Where(x=>x.Unique==true).Select(x=>x.ColumnName).First();

            Console.WriteLine(colName);

            PrimaryKeyCols[0] = table.Columns[colName];
            table.PrimaryKey = PrimaryKeyCols;
            retVal.Tables.Add(table);
            foreach (DBObject item in GetAllRecords<K>(obj))
            {
                row =table.NewRow();
                obj.getAsRow(row);
                table.Rows.Add(row);
            }
            return retVal;
        }   
    }
}