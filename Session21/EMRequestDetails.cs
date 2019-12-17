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
    public partial class EMRequestDetails : Form
    {
        private long IDs, IDss;
        private List<ChangedPart> partsList = new List<ChangedPart>();

        public EMRequestDetails(long ID1, long ID2)
        {
            IDs = ID1;

            //EMID
            IDss = ID2;
            InitializeComponent();
        }

        private void EMRequestDetails_Load(object sender, EventArgs e)
        {
            using (Session2Entities db = new Session2Entities())
            {
                var query = db.Assets.Where(x => x.ID == IDs).FirstOrDefault();
                AssetSNTxt.Text = query.AssetSN;
                AssetNameTxt.Text = query.AssetName;
                DeptTxt.Text = query.DepartmentLocation.Department.Name;
                StartDate.Value = DateTime.Now;
                var query2 = db.Parts.ToList();
                foreach (var item in query2)
                {
                    PartBox.Items.Add(item.Name);
                }
                //var query3 = db.ChangedParts.Where(x => x.EmergencyMaintenanceID == IDss).ToList();
                dataGridView1.DataSource = DT(partsList);
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void AddtoListBtn_Click(object sender, EventArgs e)
        {
            using (Session2Entities db = new Session2Entities())
            {
                ChangedPart part = new ChangedPart();
                part.Amount = amt.Value;
                var name = PartBox.Text.ToString();
                var query = db.Parts.Where(x => x.Name == name).FirstOrDefault();
                part.PartID = query.ID;
                part.EmergencyMaintenanceID = IDss;
                partsList.Add(part);
                dataGridView1.DataSource = DT(partsList);
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            using (Session2Entities db = new Session2Entities())
            {
                if (partsList != null)
                {
                    foreach (var item in partsList)
                    {
                        db.ChangedParts.Add(item);
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception es)
                    {
                        MessageBox.Show(es.ToString());
                    }
                }

                EmergencyMaintenance em = db.EmergencyMaintenances.Where(x => x.ID == IDss).FirstOrDefault();
                em.EMStartDate = StartDate.Value;
                em.EMEndDate = EndDate.Value;
                em.EMTechnicianNote = NoteTxt.Text.Trim();
                try
                {
                    db.SaveChanges();
                    this.Hide();
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.ToString());
                }
            }
        }

        private void EndDate_ValueChanged(object sender, EventArgs e)
        {
            if (NoteTxt.Text.Trim() == "" || NoteTxt.Text.Trim() == null)
            {
                MessageBox.Show("Cannot be changed untill technician note has been edited!");
            }
        }

        DataTable DT(List<ChangedPart> parts)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Part Name");
            dt.Columns.Add("Amount");
            foreach (var item in parts)
            {
                using (Session2Entities db = new Session2Entities())
                {
                    DataRow dr = dt.NewRow();
                    var id = item.PartID;
                    var query = db.Parts.Where(x => x.ID == id).First();
                    dr["Part Name"] = query.Name;
                    dr["Amount"] = item.Amount;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}
