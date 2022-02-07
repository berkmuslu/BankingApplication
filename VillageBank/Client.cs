using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageBank
{
    public class Client
    {

        string name;
        string id;
        public List<Account> accounts = new List<Account>();
        public List<CurrencyAccount> currency_account = new List<CurrencyAccount>();
        public List<Card> cards = new List<Card>();
        public Client(string name, string ID)
        {
           
            this.name = name;
            this.id = ID;

        }

        public String ID()
        {
            return id;
        }

        public String Name()
        {
            return name;
        }

    }

    public class IndividualClient: Client
    {
        protected string name;
        protected string ID;
        public List<Card> cards = new List<Card>();


        public IndividualClient(string name, string ID) : base(name, ID)
        {
            this.name = name;
            this.ID = ID;

        }
        
    }

    public class CorporateClient : Client
    {
        protected string name;
        protected string ID;
        public List<Card> cards = new List<Card>();

        public CorporateClient(string name, string ID) : base(name, ID)
        {
            this.name = name;
            this.ID = ID;

        }

    }

}
