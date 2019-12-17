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
    public partial class AccountableParty : Form
    {
        public AccountableParty()
        {
            InitializeComponent();
        }
        
        DataTable DT(List<Asset> assets)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Asset SN");
            table.Columns.Add("Asset Name");
            table.Columns.Add("Last Closed EM");
            table.Columns.Add("Number of EMs");
            table.Columns.Add("ID");

            foreach(var item in assets)
            {
                using (Session2Entities db = new Session2Entities())
                {
                    DataRow dataRow = table.NewRow();
                    dataRow["Asset SN"] = item.AssetSN;
                    dataRow["Asset Name"] = item.AssetName;
                    var query = db.EmergencyMaintenances.Where(x => x.AssetID == item.ID);
                    var query2 = query.OrderByDescending(x => x.EMEndDate).FirstOrDefault();
                    if(query2 != null){
                        if (query2.EMEndDate == null)
                        {
                            dataRow["Last Closed EM"] = "--";
                        }
                        else
                        {
                            dataRow["Last Closed EM"] = query2.EMEndDate;
                        }
                    }
                    else
                    {
                        dataRow["Last Closed EM"] = "Nil";
                    }
                   
                    dataRow["Number of EMs"] = query.Count();
                    dataRow["ID"] = item.ID;
                    table.Rows.Add(dataRow);
                }
                    
            }

            return table;
        }

        private void AccountableParty_Load(object sender, EventArgs e)
        {
            using (Session2Entities db = new Session2Entities())
            {
                var query = db.Assets.ToList();
                dataGridView1.DataSource = DT(query);
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.Columns["ID"].Visible = false;
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    if(row.Cells[2].Value != null)
                    {
                        if (row.Cells[2].Value.ToString() == "--")
                        {
                            row.DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                }
            }
                
        }

        private void SendEMRBtn_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 1)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (row.Cells["Last Closed EM"].Value.ToString() != "--")
                    {
                        EMRequest request = new EMRequest(long.Parse(row.Cells["ID"].Value.ToString()));
                        request.ShowDialog();
                        using (Session2Entities db = new Session2Entities())
                        {
                            var query = db.Assets.ToList();
                            dataGridView1.DataSource = DT(query);
                            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                            dataGridView1.Columns["ID"].Visible = false;
                            foreach (DataGridViewRow rows in dataGridView1.Rows)
                            {
                                if (rows.Cells[2].Value != null)
                                {
                                    if (rows.Cells[2].Value.ToString() == "--")
                                    {
                                        rows.DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("There is still an open request associated with this Asset! You cannot make any other requests!");
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("Invalid Selection!");
            }
        }
    }
}
