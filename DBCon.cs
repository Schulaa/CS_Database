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
        private static string dbSec = @"Sec.db";
        public static bool unlocked;

        public static bool checkPassword(string entry)
        {
            using(var db = new LiteDatabase(dbSec))
            {
                var rec = db.GetCollection<string>("Security");
                if(rec.Count() == 0)
                {
                    throw new IndexOutOfRangeException("No PW set");
                } else {
                    
                    List<string> recs = new List<string>(rec.FindAll());

                    if (recs[0] == entry) {
                        unlocked = true;
                    } else {
                        unlocked = false;
                    }
                }
            }
            return unlocked;
        }
        public static void writeToDb(DBObject bObject, mode bMode)
        {
            using (var db = new LiteDatabase(dbName))
            {
                switch (bMode)
                {
                    case mode.Insert:
                        //person:
                        if (bObject is Person)
                        {
                            var rec = db.GetCollection<Person>(Person.CollectionName);
                            rec.Insert((Person)bObject);
                        }

                        break;

                }
            }
        }
        public static void readDb(DBObject bObject, List<DBObject> resultObjList, string filter = "")
        {
            using (var db = new LiteDatabase(dbName))
            {
                if (filter == "")
                {
                    if (bObject is Person)
                    {
                        var col = db.GetCollection<Person>(Person.CollectionName);
                        var data = col.FindAll();
                        resultObjList.AddRange(data);
                        return;
                    }
                }
            }
        }


        public static DataSet GetDataSet(DBObject dBObject, string filter = "")
        {
            DataSet retVal = new DataSet();
            DataTable table;
            DataColumn[] PrimaryKeyCols = new DataColumn[1];
            DataRow row;
            using (var db = new LiteDatabase(dbName))
            {
                if (dBObject is Person)
                {
                    table = new DataTable(Person.CollectionName);

                    table.Columns.AddRange(Person.dataColumns);

                    foreach (DataColumn dmyDc in Person.dataColumns)
                    {
                        if (dmyDc.Unique)
                        {
                            PrimaryKeyCols[0] = table.Columns[dmyDc.ColumnName];
                            table.PrimaryKey = PrimaryKeyCols;
                            break;
                        }
                    }
                    retVal.Tables.Add(table);

                    if (filter == "")
                    {
                        var col = db.GetCollection<Person>(Person.CollectionName);
                        var data = col.FindAll();
                        foreach (Person person in data)
                        {
                            row = table.NewRow();
                            person.getAsRow(row);
                            table.Rows.Add(row);
                        }
                    }
                }

            }

            return retVal;
        }
    }
    enum mode
    {
        Update,
        Insert,
        Delete
    }
}