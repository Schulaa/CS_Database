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
                SecurityObject so = new SecurityObject();
                var rec = db.GetCollection<SecurityObject>(so.CollectionName);
                if (rec.Count() == 0)
                {
                    throw new IndexOutOfRangeException("No PW set");
                }
                else
                {
                    // var dbPwd = rec.Query().First();
                    rec.EnsureIndex(x=>x.hashedPwd);
                    var dbPwd = rec.Query()
                        .First();

                    if (dbPwd.checkPwd(entry))
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
                SecurityObject so = new SecurityObject();
                var rec = db.GetCollection<SecurityObject>(so.CollectionName);
                rec.DeleteAll();
                SecurityObject dmy = new SecurityObject();
                dmy.setPassword(pwd);
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
                            var rec = db.GetCollection<Person>(bObject.CollectionName);
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
                    table = new DataTable(dBObject.CollectionName);

                    table.Columns.AddRange(dBObject.dataColumns);

                    foreach (DataColumn dmyDc in dBObject.dataColumns)
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
                        var col = db.GetCollection<Person>(dBObject.CollectionName);
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
                        var col = db.GetCollection<Person>(dBObject.CollectionName);
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
        public static Person GetPersonById(int id)
        {
            using (var db = new LiteDatabase(dbName))
            {
                Person pers = new Person();
                var col = db.GetCollection<Person>(pers.CollectionName);
                var res = col.Query()
                    .Where(x => x.id == id);
                return res.First();
            }
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

        #region Read PersonAccountInfo
        public static PersonAccountInfo getPAIforPerson(Person p)
        {
            using (var db = new LiteDatabase(dbName))
            {
                PersonAccountInfo pai = new PersonAccountInfo();
                var col = db.GetCollection<PersonAccountInfo>(pai.CollectionName);
                col.EnsureIndex(x => x.id);
                try
                {
                    var query = col.Query()
                                .Where(x => (x.id == p.id))
                                .First();
                    return query;
                }
                catch (System.InvalidOperationException)
                {
                    return null;
                }

            }
        }
        #endregion

        #region Write Database (Person)
        public static void UpsertPerson(Person person1)
        {
            using (var db = new LiteDatabase(dbName))
            {
                Person pers = new Person();
                var col = db.GetCollection<Person>(pers.CollectionName);
                col.Upsert(person1);
                // col.Update(person1);

            }
        }
        public static void DeletePerson(Person person1)
        {
            Person pers = new Person();
            using (var db = new LiteDatabase(dbName))
            {
                var col = db.GetCollection<Person>(pers.CollectionName);
                col.Delete(person1.id);
            }
        }

        public static Person UpdateIdPerson(Person person1, int newId)
        {
            UpdateIdPersonAccountInfo(person1, newId);
            DeletePerson(person1);
            person1.id = newId;
            UpsertPerson(person1);
            return person1;
        }
        #endregion

        #region Write Database (Account Info)
        public static void UpsertPersonAccountInfo(PersonAccountInfo personAccountInfo)
        {
            using (var db = new LiteDatabase(dbName))
            {
                var col = db.GetCollection<PersonAccountInfo>(personAccountInfo.CollectionName);
                col.Upsert(personAccountInfo);
            }
        }
        public static void UpdateIdPersonAccountInfo(Person person, int newId)
        {
            using (var db = new LiteDatabase(dbName))
            {
                PersonAccountInfo pai = getPAIforPerson(person);
                var col = db.GetCollection<PersonAccountInfo>(pai.CollectionName);
                col.EnsureIndex(x => x.id);
                col.Delete(pai.id);
                pai.id = newId;
                col.Insert(pai);
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