using System;
using System.Data;

namespace Datenbank
{
       class PersonAccountInfo : DBObject
    {
        public int id { get; set; }
        public int mandateId { get; set; }
        public string iban { get; set; }
        public string bic { get; set; }
        public string personName { get; set; }
        public string bankName { get; set; }
        public DateTime mandateDate { get; set; }
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
                this.id,
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
            this.id = (int)val[0];
            this.mandateId = (int)val[1];
            this.iban = (string)val[2];
            this.bic = (string)val[3];
            this.personName = (string)val[4];
            this.bankName = (string)val[5];
            this.mandateDate = (DateTime)val[6];
        }
    }
}