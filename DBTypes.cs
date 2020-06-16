using System;
using System.Data;

namespace Datenbank
{
    class ColumnInfo
    {

        public string name;
        public Type colType;

        public ColumnInfo(string name, Type colType)
        {
            this.name = name;
            this.colType = colType;
        }
    }
    interface DBObject
    {
        static string CollectionName { get; }
        static ColumnInfo[] columnInfos { get; }
        static DataColumn[] dataColumns { get; }

    }
    class Person : DBObject
    {
        // public Person() {
        //     id = 999;
        //     firstName = "dmy";
        //     lastName = "dmy";
        //     city = "";
        //     postcode ="";
        //     address = "";
        //     memberType = type.Left;
        //     pmtType = paymentType.None;
        // }
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string city { get; set; }
        public string postcode { get; set; }
        public string address { get; set; }
        public type memberType { get; set; }
        public paymentType pmtType { get; set; }
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
        public DateTime birthday { get; set; }
        public DateTime entryDate { get; set; }
        public DateTime leftDate { get; set; }
        public string comment { get; set; }
        public static string CollectionName
        {
            get => "person";
        }
        // public static ColumnInfo[] columnInfos
        // {
        //     get => new ColumnInfo[] {
        //         new ColumnInfo("ID",typeof(int)),
        //         new ColumnInfo("Vorname",typeof(string)),
        //         new ColumnInfo("Nachname",typeof(string)),
        //         new ColumnInfo("Ort",typeof(string)),
        //         new ColumnInfo("PLZ",typeof(string)),
        //         new ColumnInfo("Adresse",typeof(string)),
        //         new ColumnInfo("Art",typeof(Person.type)),
        //         new ColumnInfo("Zahlungsart",typeof(Person.paymentType)),
        //         new ColumnInfo("Geburtsdatum",typeof(DateTime)),
        //         new ColumnInfo("Eintrittsdatum",typeof(DateTime)),
        //         new ColumnInfo("Austrittsdatum",typeof(DateTime)),
        //         new ColumnInfo("Bemerkung",typeof(string)),
        //     };
        // }
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
    }
}