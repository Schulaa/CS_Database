using LiteDB;
using System;
using System.Data;
using System.Collections.Generic;

namespace Datenbank
{

    class DBCon
    {
        private static string dbName = @"Database.db";
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
        public static DataSet getDataSetPerson()
        {
            DataSet retVal = new DataSet();
            DataTable dt = new DataTable("person");
            DataColumn dc;
            DataRow dr;
            Person dmy = new Person();


            //add id col:
            dc = new DataColumn();
            dc.DataType = dmy.id.GetType();
            dc.ColumnName = "id";
            dc.Unique = true;
            dt.Columns.Add(dc);

            //add firstname col:
            dc = new DataColumn();
            dc.DataType = dmy.firstName.GetType();
            dc.ColumnName = "firstName";
            dc.Caption = "Vorname";
            dc.Unique = false;
            dt.Columns.Add(dc);

            //add id col:
            dc = new DataColumn();
            dc.DataType = dmy.lastName.GetType();
            dc.ColumnName = "lastName";
            dc.Caption = "Nachname";
            dc.Unique = false;
            dt.Columns.Add(dc);

            //set primary key col:
            DataColumn[] PrimaryKeyCols = new DataColumn[1];
            PrimaryKeyCols[0] = dt.Columns["id"];
            dt.PrimaryKey = PrimaryKeyCols;

            //init dataset:
            retVal.Tables.Add(dt);

            //insert data:
            List<DBObject> lst = new List<DBObject>();
            readDb(new Person(), lst);
            foreach (Person p in lst)
            {
                dr = dt.NewRow();
                dr["id"] = p.id;
                dr["firstName"] = p.firstName;
                dr["lastName"] = p.lastName;
                dt.Rows.Add(dr);
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