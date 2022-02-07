using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageBank
{
    public class Card
    {
        Client owner;
        Account account;

        string number;
        int pin;
        string expireDate;
        int cvc;



        public Card(Client owner, Account account, string number, int pin, string expireDate, int cvc)
        {
            this.owner = owner;
            this.account = account;
            this.number = number;
            this.pin = pin;
            this.expireDate = expireDate;
            this.cvc = cvc;
        }

        public Client Owner { get => owner; set => owner = value; }
        public Account Account { get => account; set => account = value; }
        public string Number { get => number; set => number = value; }
        public int Pin { get => pin; set => pin = value; }
        public string ExpireDate { get => expireDate; set => expireDate = value; }
        public int Cvc { get => cvc; set => cvc = value; }

        public void Spend() { 
        
        
        }

     

    }


    class CreditCard : Card
    {

        double debt;
        double limit;

        Client owner;

        string number;
        int pin;
        string expireDate;
        int cvc;

        public CreditCard(Client owner, Account account, string number, int pin, string expireDate, int cvc,double debt, double limit) : base(owner, account, number, pin, expireDate, cvc)
        {
            this.debt = debt;
            this.limit = limit;
            this.owner = owner;
            this.number = number;
            this.pin = pin;
            this.expireDate = expireDate;
            this.cvc = cvc;
        }

        public double Debt { get => debt; set => debt = value; }
        public double Limit { get => limit; set => limit = value; }

        
    }


    class DebitCard : Card
    {
        double balance;

        Client owner;
        Account account;

        string number;
        int pin;
        string expireDate;
        int cvc;

        public DebitCard(Client owner, Account account, string number, int pin, string expireDate, int cvc, double balance) : base(owner, account, number, pin, expireDate, cvc)
        {
            this.owner = owner;
            this.account = account;
            this.number = number;
            this.pin = pin;
            this.expireDate = expireDate;
            this.cvc = cvc;
            this.balance = balance;
        }

        public double Balance { get => balance; set => balance = value; }

       
    }





}
