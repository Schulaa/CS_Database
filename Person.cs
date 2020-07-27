using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;


namespace Datenbank
{
    class Person : DBObject
    {

        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string city { get; set; }
        public string postcode { get; set; }
        public string address { get; set; }
        public type memberType { get; set; }
        public paymentType pmtType { get; set; }
        public DateTime birthday { get; set; }
        public DateTime entryDate { get; set; }
        public DateTime leftDate { get; set; }
        public string comment { get; set; }
        public string CollectionName
        {
            get => "person";
        }
        public DataColumn[] dataColumns
        {
            get => new DataColumn[]{
                new DataColumn {
                    DataType = typeof(int),
                    ColumnName = "ID",
                    Unique = true,
                    },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "Vorname",
                    },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "Nachname",
                    },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "Ort",
                    },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "PLZ",
                    },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "Adresse",
                    },
                new DataColumn {
                    DataType = typeof(Person.type),
                    ColumnName = "Art",
                    },
                new DataColumn {
                    DataType = typeof(Person.paymentType),
                    ColumnName = "Zahlungsart",
                    },
                new DataColumn {
                    DataType = typeof(DateTime),
                    ColumnName = "Geburtsdatum",
                    },
                new DataColumn {
                    DataType = typeof(DateTime),
                    ColumnName = "Eintrittsdatum",
                    },
                new DataColumn {
                    DataType = typeof(DateTime),
                    ColumnName = "Austrittsdatum",
                    },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "Bemerkung",
                    }

            };
        }
        public enum type
        {
            Active,
            Passive,
            Honor,
            Left

        }
        public enum paymentType
        {
            None,
            SEPA,
            Cash
        }

        public DataRow getAsRow(DataRow row)
        {

            object[] data = this.getAsObjArr();
            for (int i = 0; i < data.Length; i++)
            {
                row[dataColumns[i].ColumnName] = data[i];
            }
            return row;
        }

        public object[] getAsObjArr()
        {
            return new object[]{this.id,
                            this.firstName,
                            this.lastName,
                            this.city,
                            this.postcode,
                            this.address,
                            this.memberType,
                            this.pmtType,
                            this.birthday,
                            this.entryDate,
                            this.leftDate,
                            this.comment
                            };
        }

        public void setAsObjArr(object[] val)
        {
            this.id = (int)val[0];
            this.firstName = (string)val[1];
            this.lastName = (string)val[2];
            this.city = (string)val[3];
            this.postcode = (string)val[4];
            this.address = (string)val[5];
            this.memberType = (Person.type)val[6];
            this.pmtType = (Person.paymentType)val[7];
            this.birthday = (DateTime)val[8];
            this.entryDate = (DateTime)val[9];
            this.leftDate = (DateTime)val[10];
            this.comment = (string)val[11];
        }

        public void Upsert()
        {
            DBOperations.Upsert<Person>(this);
        }

        public void Delete()
        {
            PersonAccountInfo pai = new PersonAccountInfo();
            try
            {
                pai = (PersonAccountInfo)pai.FindById(this.id);
                pai.Delete();
            }
            catch (System.InvalidOperationException)
            {

            }
            finally
            {
                DBOperations.DeleteRecord<Person>(this);
            }
        }

        public DBObject UpdateId(int newId)
        {
            PersonAccountInfo pai = new PersonAccountInfo();
            try
            {
                pai = (PersonAccountInfo)pai.FindById(this.id);
                pai.UpdateId(newId);
            }
            catch (System.InvalidOperationException)
            {

            }
            return DBOperations.UpdatePrimaryKey<Person>(this, newId);

        }
        public DBObject FindById(int id)
        {
            List<Person> lst = DBOperations.GetAllRecords<Person>(this);
            return lst.Where(x => x.id == id).First();
        }
        public int GetNextId()
        {
            List<Person> lst = DBOperations.GetAllRecords<Person>(this);
            try
            {
                int id = lst.OrderByDescending(x => x.id).Select(x => x.id).First();
                return id;
            }
            catch (System.InvalidOperationException)
            {
                return 1;
            }
        }
    }

}