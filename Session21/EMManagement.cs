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
    public partial class EMManagement : Form
    {
        public EMManagement()
        {
            InitializeComponent();
        }

        private void EMManagement_Load(object sender, EventArgs e)
        {
            using (Session2Entities db = new Session2Entities())
            {
                var query = db.EmergencyMaintenances.OrderByDescending(x => x.PriorityID).OrderByDescending(x => x.EMReportDate).ToList();
                dataGridView1.DataSource = DT(query);
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.Columns["ID1"].Visible = false;
            }

        }

        DataTable DT(List<EmergencyMaintenance> assets)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Asset SN");
            table.Columns.Add("Asset Name");
            table.Columns.Add("Request Date");
            table.Columns.Add("Employee Full Name");
            table.Columns.Add("Department");
            table.Columns.Add("ID1");
            table.Columns.Add("ID2");

            foreach (var item in assets)
            {
                using (Session2Entities db = new Session2Entities())
                {
                    if (item.EMEndDate == null)
                    {
                        DataRow dataRow = table.NewRow();
                        dataRow["Asset SN"] = item.Asset.AssetSN;
                        dataRow["Asset Name"] = item.Asset.AssetName;
                        dataRow["Request Date"] = item.EMReportDate;
                        dataRow["Employee Full Name"] = item.Asset.Employee.FirstName + " " + item.Asset.Employee.LastName;
                        dataRow["Department"] = item.Asset.DepartmentLocation.Department.Name;
                        dataRow["ID1"] = item.AssetID;
                        dataRow["ID2"] = item.ID;
                        table.Rows.Add(dataRow);
                    }
                }

            }
            return table;
        }

        private void ManageReqBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    EMRequestDetails details = new EMRequestDetails(long.Parse(row.Cells["ID1"].Value.ToString()), long.Parse(row.Cells["ID2"].Value.ToString()));
                    details.ShowDialog();
                    using (Session2Entities db = new Session2Entities())
                    {
                        var query = db.EmergencyMaintenances.OrderByDescending(x => x.PriorityID).OrderByDescending(x => x.EMReportDate).ToList();
                        dataGridView1.DataSource = DT(query);
                        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridView1.Columns["ID1"].Visible = false;
                        dataGridView1.Columns["ID2"].Visible = false;
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
