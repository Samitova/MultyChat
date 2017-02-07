using System;
using System.Windows.Forms;
using MMChatEngine;

namespace MMChatClient
{
    public partial class Regiter : Form
    {
        private readonly Client _client;
        public Regiter(Client client)
        {
            InitializeComponent();
            pcBirthdate.MaxDate = DateTime.Now;
            pcBirthdate.Format = DateTimePickerFormat.Custom;
            pcBirthdate.CustomFormat = @"dd MMMM yyyy";
            _client = client;
            
            cbSex.Items.AddRange(Enum.GetNames(typeof(Sex)));
        }

        public string UserLogin
        {
            get { return tbLogin.Text; }
        }

        public UserInfoWithPrivateInfo UserInfoWithPrivateInfo { get; private set; }

        private async void btRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbLogin.Text))
            {
                MessageBox.Show("Login can't be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                MessageBox.Show("Password can't be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tbPassword.Text != tbPasswordConfirm.Text)
            {
                MessageBox.Show("Password and Confirm password must be equal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(tbNick.Text))
            {
                MessageBox.Show("Nick can't be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(cbSex.Text))
            {
                MessageBox.Show("Please, select your sex.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(pcBirthdate.Text))
            {
                MessageBox.Show("Please, select your birthdate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _client.Connect();

            Sex sex;
            Enum.TryParse(cbSex.Text, out sex);
            UserInfoWithPrivateInfo = new UserInfoWithPrivateInfo(tbPassword.Text, tbNick.Text, sex, DateTime.ParseExact(pcBirthdate.Text, "dd MMMM yyyy", null));


            RequestToServerResult result = await _client.RegisterNewUser(UserLogin, UserInfoWithPrivateInfo);

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
