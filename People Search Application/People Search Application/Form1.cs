using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace People_Search_Application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    this.profilesTableAdapter.Fill(this.appData.Profiles);
                    profilesBindingSource.DataSource = this.appData.Profiles;
                    
                }
                else
                {
                    var query = from o in this.appData.Profiles
                                where o.Profile.Contains(txtSearch.Text) || o.Age == txtSearch.Text || o.Email == txtSearch.Text || o.Interests == txtSearch.Text || o.Address.Contains(txtSearch.Text) 
                                select o;
                    profilesBindingSource.DataSource = query.ToList();
                    
                }
            }
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete this profile ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    profilesBindingSource.RemoveCurrent();
            }
        }

        private void btnProfilePicture_Click(object sender, EventArgs e)
        {
            try
            {
                using(OpenFileDialog ofd=new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true, Multiselect = false })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                        pictureBox.Image = Image.FromFile(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                panel.Enabled = true;
                txtProfile.Focus();
                this.appData.Profiles.AddProfilesRow(this.appData.Profiles.NewProfilesRow());
                profilesBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                profilesBindingSource.ResetBindings(false);
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            panel.Enabled = true;
            txtProfile.Focus();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            panel.Enabled = false;
            profilesBindingSource.ResetBindings(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                profilesBindingSource.EndEdit();
                profilesTableAdapter.Update(this.appData.Profiles);
                panel.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                profilesBindingSource.ResetBindings(false);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'appData.Profiles' table. You can move, or remove it, as needed.
            this.profilesTableAdapter.Fill(this.appData.Profiles);
            profilesBindingSource.DataSource = this.appData.Profiles;
        }
    }
}
