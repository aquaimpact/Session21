using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session21
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            using(Session2Entities db = new Session2Entities())
            {
                var username = usernameTxt.Text.Trim();
                var password = PasswordTxt.Text.Trim();
                var query = db.Employees.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
                if(query != null)
                {
                    if(query.isAdmin == true)
                    {
                        EMManagement management = new EMManagement();
                        this.Hide();
                        management.ShowDialog();
                        
                    }
                    else
                    {
                        AccountableParty accountable = new AccountableParty();
                        this.Hide();
                        accountable.ShowDialog();
                        
                    }
                }
                else
                {
                    MessageBox.Show("Invalid User!");
                }
            }
        }
    }
}
