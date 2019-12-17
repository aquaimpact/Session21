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
    public partial class EMRequest : Form
    {
        private long IDs;
        public EMRequest(long ID)
        {
            IDs = ID;
            InitializeComponent();
        }

        private void EMRequest_Load(object sender, EventArgs e)
        {
            using (Session2Entities db = new Session2Entities())
            {
                var query = db.Assets.Where(x => x.ID == IDs).FirstOrDefault();
                AssetSNTxt.Text = query.AssetSN;
                AssetNameTxt.Text = query.AssetName;
                DeptTxt.Text = query.DepartmentLocation.Department.Name;

                var query2 = db.Priorities.ToList();
                foreach(var item in query2)
                {
                    comboBox1.Items.Add(item.Name);
                }
                comboBox1.SelectedIndex = 0;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(Session2Entities db = new Session2Entities())
            {
                var desc = DescTxt.Text.Trim();
                var others = OtherTxt.Text.Trim();
                var date = DateTime.Now;

                EmergencyMaintenance maintenance = new EmergencyMaintenance();
                maintenance.AssetID = IDs;
                var query = db.Priorities.Where(x => x.Name == comboBox1.Text).FirstOrDefault();
                maintenance.PriorityID = query.ID;
                if(desc == "" || others == "")
                {
                    MessageBox.Show("Invalid Description or Other Considerations!");
                }
                else
                {
                    maintenance.DescriptionEmergency = desc;
                    maintenance.OtherConsiderations = others;
                    maintenance.EMReportDate = date;
                    maintenance.EMStartDate = date;
                    maintenance.EMEndDate = null;
                    maintenance.EMTechnicianNote = null;

                    db.EmergencyMaintenances.Add(maintenance);
                    try
                    {
                        db.SaveChanges();
                        MessageBox.Show("Submitted Successfully!");
                        this.Hide();
                    }
                    catch (Exception es)
                    {
                        MessageBox.Show(es.ToString());
                    }
                }
                
            }
        }
    }
}
