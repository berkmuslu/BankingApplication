using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageBank
{
    public class Account
    {
         public string number;
         public Client owner;
         public List<Card> cards = new List<Card>();
         public string iban;
         public double balance;

        public Account(string number, Client owner, double balance, string iban)
        {
            this.number = number;
            this.owner = owner;
            this.balance = balance;
            this.iban = iban;
        }

    }

    class CheckingAccount : Account
    {
        protected string number;
        protected Client owner;
        protected double balance;
        protected string iban;

        public CheckingAccount(string number, Client owner, double balance, string iban) : base(number, owner, balance, iban)
        {
            this.number = number;
            this.owner = owner;
            this.balance = balance;
        }

    }

    class SavingAccount : Account
    {
        int interestRate;
        int period;

        protected string number;
        protected Client owner;
        protected double balance;
        protected string iban;


        public SavingAccount(string number, Client owner, double balance, int interestRate, int period, string iban) : base(number, owner, balance, iban)
        {
            this.number = number;
            this.owner = owner;
            this.balance = balance;
            this.interestRate = interestRate;
            this.period = period;
            this.iban = iban;
        }


    }


    public class CurrencyAccount : Account
    {
        
        protected string number;
        protected Client owner;
        private double euro_balance;
        private double usd_balance;
        protected string iban;

        public double Euro_balance { get => euro_balance; set => euro_balance = value; }
        public double Usd_balance { get => usd_balance; set => usd_balance = value; }

        public CurrencyAccount(string number, Client owner, double balance, string iban, double euro, double usd) : base(number, owner, balance, iban)
        {
            this.number = number;
            this.owner = owner;
            this.balance = balance;
            this.iban = iban;
            this.euro_balance = euro;
            this.usd_balance = usd;



        }


    }


}
