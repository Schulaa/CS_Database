using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;

namespace Datenbank
{
    interface DBObject
    {
        public string CollectionName { get; }
        // public static ColumnInfo[] columnInfos { get; }
        public DataColumn[] dataColumns { get; }
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
    }
    class SecurityObject : DBObject
    {
        public void setPassword(string pwd) {
            this.hashedPwd = SecurePasswordHasher.Hash(pwd);
        }


        public string hashedPwd{get;set;}

        public string CollectionName
        {
            get => "security";
        }
        public DataColumn[] dataColumns
        {
            get => new DataColumn[]{
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "Password",
                    Unique = true,
                    }
            };
        }

        public object[] getAsObjArr()
        {
            return new object[] { this.hashedPwd };
        }

        public void setAsObjArr(object[] val)
        {
            this.hashedPwd = (string)val[0];
        }
        public bool checkPwd(string pwd)
        {
            return SecurePasswordHasher.Verify(pwd,this.hashedPwd);
        }

        public DataRow getAsRow(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
    class PersonAccountInfo : DBObject
    {
        public int personId{get;set;}
        public int mandateId{get;set;}
        public string iban{get;set;}
        public string bic{get;set;}
        public string personName{get;set;}
        public string bankName{get;set;}
        public DateTime mandateDate{get;set;}
        public string CollectionName
        {
            get => "accountInfo";
        }
        public DataColumn[] dataColumns
        {
            get => new DataColumn[]{
                new DataColumn {
                    DataType = typeof(int),
                    ColumnName = "Personen-ID",
                    Unique = true,
                    },
                new DataColumn {
                    DataType = typeof(int),
                    ColumnName = "Mandatsreferenz"
                },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "IBAN"
                },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "BIC"
                },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "Kontoinhaber"
                },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "Kreditinstitut"
                },
                new DataColumn {
                    DataType = typeof(DateTime),
                    ColumnName = "Mandatsdatum"
                }
            };
        }
        public object[] getAsObjArr()
        {
            return new object[] {
                this.personId,
                this.mandateId,
                this.iban,
                this.bic,
                this.personName,
                this.bankName,
                this.mandateDate
            };
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

        public void setAsObjArr(object[] val)
        {
            this.personId = (int)val[0];
            this.mandateId = (int)val[1];
            this.iban = (string)val[2];
            this.bic = (string)val[3];
            this.personName = (string)val[4];
            this.bankName = (string)val[5];
            this.mandateDate = (DateTime)val[6];
        }
    }
}