using MMChatEngine;
using System.Windows.Forms;

namespace MMChatClient
{
    public partial class Login : Form
    {
        private Client _client;

        public Login(Client client)
        {
            InitializeComponent();
            _client = client;
        }

        public string UserLogin
        {
            get { return tbLogin.Text; }
        }

        public string UserPassword
        {
            get { return tbPassword.Text; }
        }

        private async void btOK_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserLogin))
            {
                MessageBox.Show("Login can't be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(UserPassword))
            {
                MessageBox.Show("Password can't be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _client.Connect();

            RequestToServerResult result = await _client.SendNegotiation(UserLogin, UserPassword);

            if (result == RequestToServerResult.OK)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else if (result == RequestToServerResult.Disconnect)
            {
                MessageBox.Show("Couldn`t connect to server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
