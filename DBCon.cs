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

        #region Login Mgt
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
        #endregion

        #region Write Database (general)
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
        #endregion

        #region Read Database (general)
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

        #endregion

        #region Person filters
        public static DataSet PersonFilterMemberType(Person.type memberType)
        {
            DataSet result = new DataSet();
            DataTable table;
            DataColumn[] PrimaryKeyCols = new DataColumn[1];
            DataRow row;

            using (var db = new LiteDatabase(dbName))
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
                result.Tables.Add(table);

                var col = db.GetCollection<Person>(Person.CollectionName);
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

            using (var db = new LiteDatabase(dbName))
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
                result.Tables.Add(table);

                var col = db.GetCollection<Person>(Person.CollectionName);
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
        public static Person GetPersonById(int id)
        {
            using (var db = new LiteDatabase(dbName))
            {
                var col = db.GetCollection<Person>(Person.CollectionName);
                var res = col.Query()
                    .Where(x => x.id == id);
                return res.First();
            }
        }
        public static DataSet GetPersonDSById(int id)
        {
            DataSet result = new DataSet();
            result.DataSetName = Person.CollectionName;
            DataTable table;
            DataColumn[] PrimaryKeyCols = new DataColumn[1];
            DataRow row;

            using (var db = new LiteDatabase(dbName))
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
                result.Tables.Add(table);

                var col = db.GetCollection<Person>(Person.CollectionName);
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
                var col = db.GetCollection<Person>(Person.CollectionName);
                col.EnsureIndex(x=>x.id);
                Person person = col.Query()
                    .OrderByDescending(x=>x.id)
                    .FirstOrDefault();        
                
                return person is null ? 1 : person.id +1;            
            }
        }
        
        #endregion

        #region Write Database (Person)
        public static void UpsertPerson(Person person1)
        {
            using (var db = new LiteDatabase(dbName))
            {
                var col = db.GetCollection<Person>(Person.CollectionName);
                col.Upsert(person1);
                // col.Update(person1);

            }
        }

        public static void DeletePerson(Person person1)
        {
            using (var db = new LiteDatabase(dbName))
            {
                var col = db.GetCollection<Person>(Person.CollectionName);
                col.Delete(person1.id);
            }
        }

        public static Person UpdateIdPerson(Person person1, int newId)
        {
            DeletePerson(person1);
            person1.id = newId;
            UpsertPerson(person1);
            return person1;
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