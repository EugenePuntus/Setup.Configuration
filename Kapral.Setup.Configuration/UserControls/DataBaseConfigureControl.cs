using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kapral.Setup.Configuration.UserControls
{
    public partial class DataBaseConfigureControl : UserControl
    {
        private string ConnectionStringTemp
            => $"Server={txtServer.Text};Database={txtDataBase.Text};User ID={txtLogin.Text};Password={txtPassword.Text};";

        public string ConnectionString
        {
            get { return new SqlConnectionStringBuilder(ConnectionStringTemp).ToString(); }
        }

        public DataBaseConfigureControl()
        {
            InitializeComponent();
        }

        public DataBaseConfigureControl(string connectionString) : this()
        {
            ReadConnectingString(connectionString);
        }

        public void ReadConnectingString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return;

            var sqlConnection = new SqlConnectionStringBuilder(connectionString);

            txtServer.Text = sqlConnection.DataSource;
            txtDataBase.Text = sqlConnection.InitialCatalog;
            txtLogin.Text = sqlConnection.UserID;
            txtPassword.Text = sqlConnection.Password;
        }

        private void checkConnection_Click(object sender, EventArgs e)
        {
            var result = CheckConnection();

            if (result)
                MessageBox.Show("Verification of the connection was successful.");
        }

        public bool CheckConnection()
        {
            try
            {
                using (var sqlConnection = new SqlConnection(ConnectionStringTemp))
                {
                    sqlConnection.Open();
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Connection failed. Error:\n\n" + e.Message);
                return false;
            }
        }
    }
}
