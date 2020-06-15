namespace Datenbank
{
    class Person
    {
        public int id {get;set;}
        public string firstName {get;set;}
        public string lastName {get;set;}
        public string city {get;set;}
        public string postcode {get;set;}
        public string address {get;set;}
        public enum type
        {
            Active,
            Passive,
            Honor

        } 

        
    }
}