using System;

namespace Datenbank
{
    interface DBObject
    {
        static string CollectionName { get; }
    }
    class Person : DBObject 
    {
        public Person() {
            id = 999;
            firstName = "dmy";
            lastName = "dmy";
            city = "";
            postcode ="";
            address = "";
            memberType = type.Left;
            pmtType = paymentType.None;
        }
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string city { get; set; }
        public string postcode { get; set; }
        public string address { get; set; }
        public type memberType{get;set;}
        public paymentType pmtType{get;set;}
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
        public static string CollectionName { 
            get => "person"; 
            }
    }
}