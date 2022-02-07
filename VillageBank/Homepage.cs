using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace VillageBank
{
    public partial class Homepage : Form
    {
        Client client;
        List<AccountBlock> accountBlocks = new List<AccountBlock>();

        bool created = false;
        static int total_blocks = 0;
        static int total_cards = 0;

        public Homepage()
        {
            InitializeComponent();


        }

        public Homepage(Client cl)
        {
            InitializeComponent();

            if(cl is IndividualClient)
            {
                tabControl1.TabPages.RemoveAt(6);
            }

            client = cl;
            welcomeText.Text = "Welcome, \n" + client.Name();

            if (cl is IndividualClient)
            {
                usertype.Text += " Invdividual";
            }
            else if (cl is CorporateClient)
            {
                usertype.Text += " Corporate";
            }


            createAccountBlock();
            createCardBlock();
        }

        private void newAccountBtn_Click(object sender, EventArgs e)
        {
            createAccountBlock();
            
        }

        private void createCardBlock()
        {
        
                foreach (var card in client.cards)
                {

                    CardBlock cardBlock = new CardBlock(card);
                    tabPage2.Controls.Add(cardBlock);


                }

            }


  
        private void createAccountBlock()
        {
            if (!created)
            {
                foreach (var accounts in client.accounts)
                {

                    AccountBlock acc = new AccountBlock(accounts);
                    accountBlocks.Add(acc);
                    tabPage1.Controls.Add(acc);

                }
                created = true;

            }
            else
            {
                string accNumber = "";
                string iban = "";
                Random rnd = new Random();
                accNumber += rnd.Next(1000, 10000);
                accNumber += "-";
                accNumber += rnd.Next(1000, 10000);

                iban += rnd.Next(1000, 10000);
                iban += rnd.Next(1000, 10000);
                iban += rnd.Next(1000, 10000);
                iban += rnd.Next(1000, 10000);
                Account account = new CheckingAccount(accNumber,client, 0, iban);

                using (var connection = new MySqlConnection("Server=sql11.freemysqlhosting.net;User ID=sql11463638;Password=v3GB9RXB5P;Database=sql11463638"))
                {
                    connection.Open();

                    using (var command = new MySqlCommand("INSERT INTO accounts (accountID,owner, balance, iban, accountType) VALUES(@accID, @owner, @balance, @iban, @accountType);", connection))
                    {
                        command.Parameters.Add(new MySqlParameter("@owner", client.ID()));
                        command.Parameters.Add(new MySqlParameter("@balance", account.balance));
                        command.Parameters.Add(new MySqlParameter("@iban", account.iban));

                        if (account is CheckingAccount)
                        {
                            command.Parameters.Add(new MySqlParameter("@accountType", "0"));


                        }
                        else
                        {
                            command.Parameters.Add(new MySqlParameter("@accountType", "1"));

                        }

                        command.Parameters.Add(new MySqlParameter("@accID", account.number));
                        command.ExecuteReader();

                        connection.Close();



                        client.accounts.Add(account);
                        AccountBlock acc = new AccountBlock(account);
                        accountBlocks.Add(acc);


                        tabPage1.Controls.Add(acc);
                    }


                }
            }
        }

        public class AccountBlock : Panel
        {
            public TextBox balanceBox;
            public AccountBlock(Account account)
            {
                int x;
                int y;

                switch (total_blocks)
                {
                    case 0:
                        x = 20;
                        y = 20;
                        break;
                    case 1:
                        x = 20 + 190;
                        y = 20;
                        break;

                    case 2:
                        x = 20 + 190*2;
                        y = 20;
                        break;
                    case 3:
                        x = 20 + 190*3;
                        y = 20;
                        break;
                    case 4:
                        x = 20;
                        y = 220;
                        break;
                    case 5:
                        x = 20 + 190;
                        y = 220;
                        break;
                    case 6:
                        x = 20 + 190*2;
                        y = 220;
                        break;
                    case 7:
                        x = 20 + 190*3;
                        y = 220;
                        break;
                  
                    default:
                        MessageBox.Show("User can only have 8 accounts!");
                        return;
                        break;
                }
               
                Location = new Point(x, y);

                
                
                Size = new Size(175, 180); //700 60 tam
                BackColor = Color.SkyBlue;
           

                Label accountID = new Label()
                {
                    Text = "Account Number",
                    Size = new Size(175, 20),
                    Location = new Point(20, 20)
                };

                TextBox accountIDBox = new TextBox()
                {
                    Enabled = false,
                    Text = account.number,
                    Size = new Size(140, 20),
                    Location = new Point(20, 40)

                };

                Label accountIBAN = new Label()
                {
                    Text = "IBAN",
                    Size = new Size(175, 20),
                    Location = new Point(20, 70)
                };

                TextBox accountIBANBox = new TextBox()
                {
                    ReadOnly = true,
                    Text = account.iban,
                    Size = new Size(140, 20),
                    Location = new Point(20, 90)

                };

                Label accountBalance = new Label()
                {
                    Text = "Balance",
                    Size = new Size(175, 20),
                    Location = new Point(20, 120)
                };

                balanceBox = new TextBox()
                {
                    Enabled = false,
                    Text = account.balance.ToString() + " TL",
                    Size = new Size(140, 20),
                    Location = new Point(20, 140)

                };

                Controls.Add(accountID);
                Controls.Add(accountIDBox);
                Controls.Add(accountIBAN);
                Controls.Add(accountIBANBox);
                
                Controls.Add(accountBalance);
                Controls.Add(balanceBox);
                
                total_blocks++;

               

            }

        }

        public class CardBlock : Panel
        {
            Card card;
            public CardBlock(Card card)
            {
                this.card = card;
                this.Click += new EventHandler(showInfo);
                int x;
                int y;
                string type;

                switch (total_cards)
                {
                    case 0:
                        x = 20;
                        y = 20;
                        break;
                    case 1:
                        x = 20 + 190;
                        y = 20;
                        break;

                    case 2:
                        x = 20 + 190 * 2;
                        y = 20;
                        break;
                    case 3:
                        x = 20 + 190 * 3;
                        y = 20;
                        break;
                    case 4:
                        x = 20;
                        y = 220;
                        break;
                    case 5:
                        x = 20 + 190;
                        y = 220;
                        break;
                    case 6:
                        x = 20 + 190 * 2;
                        y = 220;
                        break;
                    case 7:
                        x = 20 + 190 * 3;
                        y = 220;
                        break;

                    default:
                        MessageBox.Show("User can only have 8 cards!");
                        return;
                        break;
                }

                Location = new Point(x, y);



                Size = new Size(175, 180); //700 60 tam
                BackColor = Color.SkyBlue;


                Label cardNUM = new Label()
                {
                    Text = "Card Number",
                    Size = new Size(175, 20),
                    Location = new Point(20, 20)
                };

                TextBox cardNUMBox = new TextBox()
                {
                    Enabled = false,
                    Text = card.Number,
                    Size = new Size(140, 20),
                    Location = new Point(20, 40)

                };

                Label cardExpire = new Label()
                {
                    Text = "Expire Date",
                    Size = new Size(175, 20),
                    Location = new Point(20, 70)
                };

                TextBox cardExpireBox = new TextBox()
                {
                    Enabled = false,
                    Text = card.ExpireDate,
                    Size = new Size(140, 20),
                    Location = new Point(20, 90)

                };

                Label cardType = new Label()
                {
                    Text = "Card Type",
                    Size = new Size(175, 20),
                    Location = new Point(20, 120)
                };

                if(card is CreditCard)
                {
                    type = "Credit Card";
                }
                else
                {
                    type = "Debit Card";
                }

                TextBox cardTypeBOX = new TextBox()
                {
                    Enabled = false,
                    Text = type,
                    Size = new Size(140, 20),
                    Location = new Point(20, 140)

                };

                Controls.Add(cardNUM);
                Controls.Add(cardNUMBox);
                Controls.Add(cardExpire);
                Controls.Add(cardExpireBox);
                Controls.Add(cardType);
                Controls.Add(cardTypeBOX);

                total_cards++;


            }
            public void showInfo(object sender, EventArgs e)
            {
                if(card is DebitCard)
                {
                    MessageBox.Show(this,"Account Number: " + card.Account.number + Environment.NewLine + "CVC: " + card.Cvc + Environment.NewLine + "Card Expiration: " + card.ExpireDate + Environment.NewLine + "Balance: " + ((DebitCard)card).Balance.ToString(),"Card Info");
                    
                }
                else
                {


                    MessageBox.Show(this, "CVC: " + card.Cvc + Environment.NewLine + "Card Expiration: " + card.ExpireDate + "Limit:" + ((CreditCard)card).Limit.ToString() + Environment.NewLine + "Debt: " + ((CreditCard)card).Debt.ToString(), "Card Info");

                }
            }
        }


       

        private void Homepage_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);

        }

        private void tabPage4_Enter(object sender, EventArgs e)
        {

            for (int i = comboBox1.Items.Count-1; 0 <= i; i--)
            {
                comboBox1.Items.RemoveAt(i);

            }

            foreach (var acc in client.accounts)
            {
                comboBox1.Items.Add("Account Number: " + acc.number + " Balance: " + acc.balance);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string IBAN = ibanBox.Text;
            double amount = Convert.ToDouble(amountBox.Text);
            int accID = comboBox1.SelectedIndex;

            if (client.accounts[accID].balance < amount)
            {
                MessageBox.Show("Insufficent Funds");
            }
            else
            {
                if (radioButton1.Checked) { 

                if(IBAN != client.accounts[accID].iban) {

                string number = client.accounts[accID].number;

                using (var connection = new MySqlConnection("Server=sql11.freemysqlhosting.net;User ID=sql11463638;Password=v3GB9RXB5P;Database=sql11463638"))
                {
                    connection.Open();

                    using (var command = new MySqlCommand("SELECT * FROM accounts WHERE iban = @iban;", connection))
                    {
                        command.Parameters.Add(new MySqlParameter("@iban", IBAN));


                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();

                            if (reader.HasRows)
                            {
                                double transfer_balance = reader.GetDouble("balance");
                                connection.Close();
                                connection.Open();

                                using (var command1 = new MySqlCommand("UPDATE accounts SET balance = @balance WHERE iban = @iban;", connection))
                                {
                                    command1.Parameters.Add(new MySqlParameter("@iban", IBAN));
                                    command1.Parameters.Add(new MySqlParameter("@balance", amount + transfer_balance));

                                    command1.ExecuteReader();



                                }

                                connection.Close();
                                connection.Open();

                                using (var command1 = new MySqlCommand("UPDATE accounts SET balance = @balance WHERE accountID = @number;", connection))
                                {
                                   

                                    command1.Parameters.Add(new MySqlParameter("@number", number));
                                    command1.Parameters.Add(new MySqlParameter("@balance", client.accounts[accID].balance - amount));

                                    command1.ExecuteReader();



                                }


                                MessageBox.Show("Transfer Completed!");




                            }
                            else
                            {
                                MessageBox.Show("Iban is not correct!");
                            }

}
                            connection.Close();

                        }

                        comboBox1.Text = "";
                        amountBox.Text = "";
                        ibanBox.Text = "";

                        tabControl1.SelectedTab = tabPage1;
                    }


                

                }
                else
                {
                    MessageBox.Show("You can't send money to the same account!");
                }
                }
                else
                {
                    using (var connection = new MySqlConnection("Server=sql11.freemysqlhosting.net;User ID=sql11463638;Password=v3GB9RXB5P;Database=sql11463638"))
                    {
                        connection.Open();
                        using (var command1 = new MySqlCommand("UPDATE accounts SET balance = @balance WHERE accountID = @number;", connection))
                        {


                            command1.Parameters.Add(new MySqlParameter("@number", client.accounts[accID].number));
                            command1.Parameters.Add(new MySqlParameter("@balance", client.accounts[accID].balance - amount));

                            command1.ExecuteReader();



                        }

                        MessageBox.Show("Transfer Completed!");
                        comboBox1.Text = "";
                        amountBox.Text = "";
                        ibanBox.Text = "";

                        tabControl1.SelectedTab = tabPage1;
                    }
                }

            }
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {

            client.accounts.RemoveRange(0,client.accounts.Count);
                        using (var connection = new MySqlConnection("Server=sql11.freemysqlhosting.net;User ID=sql11463638;Password=v3GB9RXB5P;Database=sql11463638"))
            {
                connection.Open();

                using (var cmd = new MySqlCommand("SELECT * FROM accounts WHERE owner = @id AND accountType != 2;", connection))
                {
                    cmd.Parameters.Add(new MySqlParameter("@id", client.ID()));
                    using (var rd = cmd.ExecuteReader())
                    {

                        while (rd.Read())
                        {
                            double balance = rd.GetDouble("balance");
                            string iban = rd.GetString("iban");
                            string acc_num = rd.GetString("accountID");

                  

                            client.accounts.Add(new Account(acc_num, client, balance, iban)); ;

                        }
                    }

                }

            }




            int cnt = 0;

            foreach (var acc in accountBlocks)
            {
                if (client.accounts.Count != 0) { 
                acc.balanceBox.Text = client.accounts[cnt].balance.ToString() + " TL";
                cnt++;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,"You will be contacted in 24 hours if you get approved!","Your application has been submitted");
        }

     
        private void button3_Click(object sender, EventArgs e)
        {
            int index = -1;
            int cnt = 0;

            foreach (var cr in client.cards)
            {
                if (cr.Number.Equals(comboBox3.Text))
                {
                    index = cnt;
                    break;
                }
                cnt++;
            }

            
            double debt = Convert.ToDouble(debtBox.Text);
            double amount = Convert.ToDouble(payBox.Text);

            if (amount > debt)
            {
                MessageBox.Show("You can't pay more than your debt");
            }
            else
            {
                ((CreditCard)client.cards[index]).Debt -= amount;
                MessageBox.Show("Payment Sucessful");
                comboBox3.Text = "";
                debtBox.Text = "";
                payBox.Text = "";
                tabControl1.SelectedTab = tabPage1;
            }



        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {

            comboBox3.Text = "";
            debtBox.Text = "";
            payBox.Text = "";

            for (int i = comboBox3.Items.Count - 1; 0 <= i; i--)
            {
                comboBox3.Items.RemoveAt(i);

            }

            foreach (var card in client.cards)
            {

                if(card is CreditCard)
                {
                    comboBox3.Items.Add(((CreditCard)card).Number);
                }

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            int index = -1;
            int cnt = 0;

            foreach (var cr in client.cards)
            {
                if (cr.Number.Equals(comboBox3.Text))
                {
                    index = cnt;
                    break;
                }
                cnt++;
            }

            Card card = client.cards[index];

            debtBox.Text = ((CreditCard)card).Debt.ToString();

        }

        CurrencyBlock currBlock;


        private void tabPage3_Enter(object sender, EventArgs e)
        {


            if (accountList.Items.Count != 0) { 
                for(int i = accountList.Items.Count-1; 0 <= i; i--)
                {
                    
                accountList.Items.RemoveAt(i);
                accountList1.Items.RemoveAt(i);
                }
            }


           


            if (radioButton3.Checked)
            {
                curName.Text = "EUR";
            }
            else if(radioButton4.Checked)
            {
                curName.Text = "USD";
            }


            if (radioButton6.Checked)
            {
                label17.Text = "EUR";
            }
            else if(radioButton5.Checked)
            {
                label17.Text = "USD";

            }

            if(client.currency_account.Count != 0) { 
            client.currency_account.RemoveAt(0);
            }

            using (var connection = new MySqlConnection("Server=sql11.freemysqlhosting.net;User ID=sql11463638;Password=v3GB9RXB5P;Database=sql11463638"))
            {
                connection.Open();

                using (var cmd = new MySqlCommand("SELECT * FROM accounts WHERE owner = @id AND accountType = 2;", connection))
                {
                    cmd.Parameters.Add(new MySqlParameter("@id", client.ID()));
                    
                    using (var rd = cmd.ExecuteReader())
                    {
                        rd.Read();
                        if (rd.HasRows)
                        {
                           
                            string id = rd.GetString("accountID");
                            double eur = rd.GetDouble("eurBalance");
                            double usd = rd.GetDouble("usdBalance");
                            string iban = rd.GetString("iban");

                            client.currency_account.Add(new CurrencyAccount(id, client, 0, iban, eur, usd));

                        }
                    }



                }
                connection.Close();

            }


            foreach (var account in client.accounts)
            {
              
                    accountList.Items.Add("Account Number: " + account.number + " Account Balance: " + account.balance);
                    accountList1.Items.Add("Account Number: " + account.number + " Account Balance: " + account.balance);
                
            }


            if (client.currency_account.Count != 0)
            {
                currBlock = new CurrencyBlock(client.currency_account[0]);
                
                tabPage3.Controls.Add(currBlock);               


            }




            }


        public class CurrencyBlock : Panel
        {
            public TextBox usdBlock;
            public TextBox eurBlock;
            public CurrencyBlock(CurrencyAccount cur)
            {


                Location = new Point(20, 20);

                Size = new Size(180, 240); //700 60 tam
                BackColor = Color.SkyBlue;


                Label accountID = new Label()
                {
                    Text = "Account Number:",
                    Size = new Size(175, 20),
                    Location = new Point(20, 20)
                };

                TextBox accountIDBox = new TextBox()
                {
                    Enabled = false,
                    Text = cur.number,
                    Size = new Size(140, 20),
                    Location = new Point(20, 40)

                };

                Label accIban = new Label()
                {
                    Text = "IBAN",
                    Size = new Size(175, 20),
                    Location = new Point(20, 70)
                };

                TextBox accIbanBox = new TextBox()
                {
                    Enabled = false,
                    Text = cur.iban,
                    Size = new Size(140, 20),
                    Location = new Point(20, 90)

                };

                Label accEur = new Label()
                {
                    Text = "Euro Balance",
                    Size = new Size(175, 20),
                    Location = new Point(20, 120)
                };


                eurBlock = new TextBox()
                {
                    Enabled = false,
                    Text = cur.Euro_balance.ToString(),
                    Size = new Size(140, 20),
                    Location = new Point(20, 140)

                };

                Label accUSD = new Label()
                {
                    Text = "USD Balance",
                    Size = new Size(175, 20),
                    Location = new Point(20, 170)
                };


                usdBlock = new TextBox()
                {
                    Enabled = false,
                    Text = cur.Usd_balance.ToString(),
                    Size = new Size(140, 20),
                    Location = new Point(20, 190)

                };

                Controls.Add(accUSD);
                Controls.Add(accEur);
                Controls.Add(usdBlock);
                Controls.Add(eurBlock);
                Controls.Add(accIban);
                Controls.Add(accIbanBox);
                Controls.Add(accountID);
                Controls.Add(accountIDBox);

                total_cards++;


            }


            }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            double value;

            if (radioButton3.Checked)
            {
                value = 15.25;
            }
            else
            {
                value = 13.46;
            }

            try
            {
                buyAmount.Text = String.Format("{0:0.00}", Convert.ToDouble(textBox2.Text) / value);

            }
            catch (Exception)
            {
               
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            double value = 0;

            if (radioButton3.Checked)
            {
                value = 15.25;
                curName.Text = "EUR";
            }
            else if(radioButton4.Checked)
            {
                value = 13.46;
                curName.Text = "USD";

            }
            

            try
            {
                buyAmount.Text = String.Format("{0:0.00}", Convert.ToDouble(textBox2.Text) / value);

            }
            catch (Exception)
            {

            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            double value = 0;

            if (radioButton5.Checked)
            {
                value = 15.25;
                label17.Text = "EUR";
            }
            else if(radioButton6.Checked)
            {
                value = 13.46;
                label17.Text = "USD";

            }

            try
            {
                sellAmount.Text = String.Format("{0:0.00}", Convert.ToDouble(textBox5.Text) * value);

            }
            catch (Exception)
            {

            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            double value = 0;

            if (radioButton6.Checked)
            {
                value = 15.25;
                label17.Text = "EUR";
            }
            else if (radioButton5.Checked)
            {
                value = 13.46;
                label17.Text = "USD";

            }

            try
            {
                sellAmount.Text = String.Format("{0:0.00}", Convert.ToDouble(textBox5.Text) * value);

            }
            catch (Exception)
            {

            }



        }

        private void buyButton_Click(object sender, EventArgs e)
        {

            if(client.accounts[accountList.SelectedIndex].balance - Convert.ToDouble(textBox2.Text) >= 0) { 
            client.accounts[accountList.SelectedIndex].balance -= Convert.ToDouble(textBox2.Text);


            if (radioButton3.Checked)
            {
                client.currency_account[0].Euro_balance += Convert.ToDouble(buyAmount.Text);
                currBlock.eurBlock.Text = client.currency_account[0].Euro_balance.ToString();


            }
            else if (radioButton4.Checked)
            {
                client.currency_account[0].Usd_balance += Convert.ToDouble(buyAmount.Text);
                currBlock.usdBlock.Text = client.currency_account[0].Usd_balance.ToString();
            }



                        using (var connection = new MySqlConnection("Server=sql11.freemysqlhosting.net;User ID=sql11463638;Password=v3GB9RXB5P;Database=sql11463638"))
            {
                connection.Open();

                using (var cmd = new MySqlCommand("UPDATE accounts SET eurBalance = @euro, usdBalance = @usd WHERE accountID = @number AND accountType = 2;", connection))
                {
                    cmd.Parameters.Add(new MySqlParameter("@euro", client.currency_account[0].Euro_balance));
                    cmd.Parameters.Add(new MySqlParameter("@usd", client.currency_account[0].Usd_balance));
                    cmd.Parameters.Add(new MySqlParameter("@number", client.currency_account[0].number));
                    cmd.ExecuteReader();
                   

                }

                connection.Close();
                connection.Open();

                using (var cmd = new MySqlCommand("UPDATE accounts SET balance = @bal WHERE accountID = @number;", connection))
                {
                    cmd.Parameters.Add(new MySqlParameter("@bal", client.accounts[accountList.SelectedIndex].balance));
                    cmd.Parameters.Add(new MySqlParameter("@number", client.accounts[accountList.SelectedIndex].number));
                    cmd.ExecuteReader();


                }


            }

            accountList.SelectedIndex = 0;
            accountList.Text = "";
            textBox2.Text = "0";

            MessageBox.Show("Buying completed!");
            tabControl1.SelectedTab = tabPage1;
            }
            else
            {
                MessageBox.Show("Insufficent Fund!");
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {




            if (radioButton6.Checked)
            {
                if(client.currency_account[0].Euro_balance >= Convert.ToDouble(textBox5.Text)){ 
                client.currency_account[0].Euro_balance -= Convert.ToDouble(textBox5.Text);
                currBlock.eurBlock.Text = client.currency_account[0].Euro_balance.ToString();
                }
                else
                {
                    MessageBox.Show("Insufficent Fund");
                    return;
                }


            }
            else if (radioButton5.Checked)
            {
                if (client.currency_account[0].Usd_balance >= Convert.ToDouble(textBox5.Text)){

                    client.currency_account[0].Usd_balance -= Convert.ToDouble(textBox5.Text);
                    currBlock.usdBlock.Text = client.currency_account[0].Usd_balance.ToString();
                }
                else
                {
                    MessageBox.Show("Insufficent Fund");

                    return;
                }
            }

            client.accounts[accountList1.SelectedIndex].balance += Convert.ToDouble(sellAmount.Text);


                        using (var connection = new MySqlConnection("Server=sql11.freemysqlhosting.net;User ID=sql11463638;Password=v3GB9RXB5P;Database=sql11463638"))
            {
                connection.Open();

                using (var cmd = new MySqlCommand("UPDATE accounts SET eurBalance = @euro, usdBalance = @usd WHERE accountID = @number AND accountType = 2;", connection))
                {
                    cmd.Parameters.Add(new MySqlParameter("@euro", client.currency_account[0].Euro_balance));
                    cmd.Parameters.Add(new MySqlParameter("@usd", client.currency_account[0].Usd_balance));
                    cmd.Parameters.Add(new MySqlParameter("@number", client.currency_account[0].number));
                    cmd.ExecuteReader();


                }

                connection.Close();
                connection.Open();

                using (var cmd = new MySqlCommand("UPDATE accounts SET balance = @bal WHERE accountID = @number;", connection))
                {
                    cmd.Parameters.Add(new MySqlParameter("@bal", client.accounts[accountList1.SelectedIndex].balance));
                    cmd.Parameters.Add(new MySqlParameter("@number", client.accounts[accountList1.SelectedIndex].number));
                    cmd.ExecuteReader();


                }


            }

            accountList1.SelectedIndex = 0;
            accountList1.Text = "";
            sellAmount.Text = "0";

            MessageBox.Show("Selling completed!");
            tabControl1.SelectedTab = tabPage1;





        }

        private void logoutButton_Click(object sender, EventArgs e)
        {

            LoginForm login = new LoginForm();
            total_blocks = 0;
            total_cards = 0;
            client = null;
            currBlock = null;
            login.Show();
            this.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(comboBox5.Text != "") { 
            MessageBox.Show("Payment is successfull!");
            }
            else
            {
                MessageBox.Show("There is no tax to pay!");
            }
        }
    }
}
