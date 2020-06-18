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
            using (var db = new LiteDatabase(dbSec))
            {
                var rec = db.GetCollection<SecurityObject>(SecurityObject.CollectionName);
                if (rec.Count() == 0)
                {
                    throw new IndexOutOfRangeException("No PW set");
                }
                else
                {

                    List<SecurityObject> recs = new List<SecurityObject>(rec.FindAll());

                    if (recs[0].Equals(new SecurityObject(entry)))
                    {
                        unlocked = true;
                    }
                    else
                    {
                        unlocked = false;
                    }
                }
            }
            return unlocked;
        }

        public static void setPassword(string pwd)
        {
            using (var db = new LiteDatabase(dbSec))
            {
                var rec = db.GetCollection<SecurityObject>(SecurityObject.CollectionName);
                rec.DeleteAll();
                SecurityObject dmy = new SecurityObject(pwd);
                rec.Insert(dmy);
            }
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
        public static void readDb(DBObject bObject, List<DBObject> resultObjList, string nameFilter = "")
        {
            using (var db = new LiteDatabase(dbName))
            {
                if (nameFilter == "")
                {
                    if (bObject is Person)
                    {
                        var col = db.GetCollection<Person>(Person.CollectionName);
                        var data = col.FindAll();
                        resultObjList.AddRange(data);
                        return;
                    }
                }
                else
                {
                    if (bObject is Person)
                    {
                        var col = db.GetCollection<Person>(Person.CollectionName);
                        col.EnsureIndex(x => x.firstName);
                        var result = col.Query()
                            .Where(x => x.firstName.Contains(nameFilter) || x.lastName.Contains(nameFilter))
                            .ToList();

                        resultObjList.AddRange(result);
                        Console.WriteLine(resultObjList.Count);
                        return;
                    }
                }
            }
        }


        public static DataSet GetDataSet(DBObject dBObject, string nameFilter = "")
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

                    if (nameFilter == "")
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
                    else
                    {
                        var col = db.GetCollection<Person>(Person.CollectionName);
                        col.EnsureIndex(x => x.firstName);
                        var result = col.Query()
                            .Where(x => x.firstName.Contains(nameFilter) || x.lastName.Contains(nameFilter))
                            .ToList();
                        Console.WriteLine(result.Count);
                        foreach (Person person in result)
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