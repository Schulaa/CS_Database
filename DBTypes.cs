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
        public int id { get; set; }
        public string CollectionName { get; }
        // public static ColumnInfo[] columnInfos { get; }
        public DataColumn[] dataColumns { get; }
        public DataRow getAsRow(DataRow row);
        public object[] getAsObjArr();
        public void setAsObjArr(object[] val);

        public void Upsert();
        public void Delete();
        public DBObject UpdateId(int newId);
        public DBObject FindById(int id);
        public static List<TEnum> GetEnumList<TEnum>() where TEnum : Enum
            => ((TEnum[])Enum.GetValues(typeof(TEnum))).ToList();
    }
    class SecurityObject : DBObject
    {
        public void setPassword(string pwd)
        {
            this.id = 0;
            this.hashedPwd = SecurePasswordHasher.Hash(pwd);
        }
        public int id { get; set; }

        public string hashedPwd { get; set; }

        public string CollectionName
        {
            get => "security";
        }
        public DataColumn[] dataColumns
        {
            get => new DataColumn[]{
                new DataColumn {
                    DataType = typeof(int),
                    ColumnName = "ID",
                    Unique = true
                },
                new DataColumn {
                    DataType = typeof(string),
                    ColumnName = "Password"  
                    }
            };
        }

        public object[] getAsObjArr()
        {
            return new object[] { this.id,this.hashedPwd };
        }

        public void setAsObjArr(object[] val)
        {
            this.id = (int)val[0];
            this.hashedPwd = (string)val[1];
        }
        public bool checkPwd(string pwd)
        {
            return SecurePasswordHasher.Verify(pwd, this.hashedPwd);
        }

        public DataRow getAsRow(DataRow row)
        {
            throw new NotImplementedException();
        }

        public void Upsert()
        {
            DBOperations.Upsert<SecurityObject>(this);
        }

        public void Delete()
        {
            DBOperations.DeleteRecord<SecurityObject>(this);
        }

        public DBObject UpdateId(int newId)
        {
            return DBOperations.UpdatePrimaryKey<SecurityObject>(this,newId);
        }

        public DBObject FindById(int id)
        {
            return DBOperations.
        }
    }
 
}