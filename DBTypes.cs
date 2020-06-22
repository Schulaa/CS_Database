using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Datenbank
{
    // class ColumnInfo
    // {

    //     public string name;
    //     public Type colType;

    //     public ColumnInfo(string name, Type colType)
    //     {
    //         this.name = name;
    //         this.colType = colType;
    //     }
    // }
    interface DBObject
    {
        public static string CollectionName { get; }
        // public static ColumnInfo[] columnInfos { get; }
        public static DataColumn[] dataColumns { get; }
        public DataRow getAsRow(DataRow row);
        public object[] getAsObjArr();
        public void setAsObjArr(object[] val);


        public static List<TEnum> GetEnumList<TEnum>() where TEnum : Enum
            => ((TEnum[])Enum.GetValues(typeof(TEnum))).ToList();
    }
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
        public static string CollectionName
        {
            get => "person";
        }
        public static DataColumn[] dataColumns
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
    }


    class SecurityObject : DBObject
    {
        public SecurityObject(string pwd)
        {
            this.password = pwd;
        }
        public string password { get; set; }
        public static string CollectionName
        {
            get => "security";
        }
        public static DataColumn[] dataColumns
        {
            get => new DataColumn[]{
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "Password",
                    Unique = true,
                    }
            };
        }


        DataRow DBObject.getAsRow(DataRow row)
        {
            row[dataColumns[0].ColumnName] = this.password;
            return row;
        }

        public override bool Equals(object obj)
        {
            if (obj is SecurityObject)
            {
                SecurityObject so = (SecurityObject)obj;

                if (so.password == this.password)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(password);
        }

        public object[] getAsObjArr()
        {
            return new object[] {this.password};
        }

        public void setAsObjArr(object[] val)
        {
            this.password = (string)val[0];
        }
    }
}