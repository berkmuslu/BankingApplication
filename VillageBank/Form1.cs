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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Contact The System Admin!");
        }


        private void button1_Click(object sender, EventArgs e)
        {

            if (radioButton3.Checked)
            {
                if(userInp.Text == "admin" && passInp.Text == "admin"){ 
                Officer officer = new Officer("Officer");
                OfficerForm officerForm = new OfficerForm(officer);
                officerForm.Show();
                this.Hide();
                    return;
                
                }else
                {
                    MessageBox.Show("Error!");
                }

            }


            Client client;
            string name = "";
            string ID = "";
            int userType = 0;


            if (radioButton1.Checked)
            {
                userType = 1;
            }
            else if (radioButton2.Checked)
            {
                userType = 0;
            }

            using (var connection = new MySqlConnection("Server=sql11.freemysqlhosting.net;User ID=sql11463638;Password=v3GB9RXB5P;Database=sql11463638"))
            {
                connection.Open();

                using (var command = new MySqlCommand("SELECT * FROM users WHERE TCKimlik = @tckimlik AND password = @password AND userType = @type;", connection)) { 
                command.Parameters.Add(new MySqlParameter("@tckimlik", userInp.Text));
                command.Parameters.Add(new MySqlParameter("@password", passInp.Text));
                command.Parameters.Add(new MySqlParameter("@type", userType.ToString()));


                    using (var reader = command.ExecuteReader()) {
                        reader.Read();

                        if (reader.HasRows)
                        {
                            name = reader.GetString("Name");
                            name += " " + reader.GetString("Surname");
                            ID = reader.GetInt16("id").ToString();

                            userType = reader.GetInt16("userType");

                            if(userType == 0)
                            {
                                client = new IndividualClient(name, ID);                             

                            }
                            else
                            {
                                 client = new CorporateClient(name, ID);

                            }

                            connection.Close();
                            connection.Open();

                            using (var cmd = new MySqlCommand("SELECT * FROM accounts WHERE owner = @id;", connection))
                            {
                                cmd.Parameters.Add(new MySqlParameter("@id", ID));
                                using (var rd = cmd.ExecuteReader())
                                {
                                    
                                    while (rd.Read())
                                    {
                                        double balance = rd.GetDouble("balance");
                                        string iban = rd.GetString("iban");
                                        string acc_num = rd.GetString("accountID");
                                        int account_type = rd.GetInt32("accountType");
                                        if(account_type != 2) { 
                                        client.accounts.Add(new Account(acc_num, client, balance, iban)); ;
                                        }
                                    }
                                }

                            }

                            connection.Close();
                            connection.Open();

                            using (var cmd = new MySqlCommand("SELECT * FROM cards WHERE ownerID = @id AND accountID = @acc_id;", connection))
                            {
                                cmd.Parameters.Add(new MySqlParameter("@id", ID));

                                foreach (var acc in client.accounts)
                                {

                                    cmd.Parameters.Add(new MySqlParameter("@acc_id", acc.number));

                                    using (var rd = cmd.ExecuteReader())
                                    {

                                        while (rd.Read())
                                        {
                                            string cardNum = rd.GetString("cardNumber");
                                            int cardCVC = rd.GetInt32("cardCVC");
                                            string cardExp = rd.GetString("cardExp");
                                            int cardType = rd.GetInt32("cardType");
                                            Card card;
                                            if (cardType == 0)
                                            {
                                                 card = new DebitCard(client, acc, cardNum, 1234, cardExp, cardCVC,acc.balance);
                                            }
                                            else
                                            {
                                                card = new CreditCard(client, null , cardNum, 1234, cardExp, cardCVC, 400, 2000);
                                            }

                                            client.cards.Add(card);

                                        }
                                    }
                                    cmd.Parameters.RemoveAt(1);

                                }

                                

                            }

                            Homepage form = new Homepage(client);
                                this.Hide();

                                form.Show();
                            
                    }
                    else
                    {
                            MessageBox.Show("No user found!");
                    }

                    }

                }

            }

         

            


        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label1.Text = "Corporate ID";
                userInp.Text = "12312312312";
                passInp.Text = "test";
            } 
            else if(radioButton2.Checked)
            {
                label1.Text = "Identity Number";
                userInp.Text = "11111111111";
                passInp.Text = "berk";
            }
            else
            {
                label1.Text = "Username";
                userInp.Text = "admin";
                passInp.Text = "admin";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label1.Text = "Corporate ID";
                userInp.Text = "12312312312";
                passInp.Text = "test";
            }
            else if (radioButton2.Checked)
            {
                label1.Text = "Identity Number";
                userInp.Text = "11111111111";
                passInp.Text = "berk";
            }
            else
            {
                label1.Text = "Username";
                userInp.Text = "admin";
                passInp.Text = "admin";
            }
        }
    }
}
