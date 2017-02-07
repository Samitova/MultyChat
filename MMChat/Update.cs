using System;
using System.Windows.Forms;
using MMChatEngine;

namespace MMChatClient
{
    public partial class Update : Form
    {
        private readonly Client _client;
        public Update(Client client)
        {
            InitializeComponent();
            pcBirthdate.MaxDate = DateTime.Now;
            pcBirthdate.Format = DateTimePickerFormat.Custom;
            pcBirthdate.CustomFormat = "dd MMMM yyyy";
            _client = client;
            cbSex.Items.AddRange(Enum.GetNames(typeof(Sex)));

            tbLogin.Text = _client.Login;
            tbNick.Text = _client.Users[_client.Login].Nick;
            cbSex.Text = _client.Users[_client.Login].Sex.ToString();
            pcBirthdate.Text = _client.Users[_client.Login].Birthdate.ToString(pcBirthdate.CustomFormat);
        }

        public UserInfoWithPrivateInfo UserInfoWithPrivateInfo { get; private set; }

        private async void btUpdate_Click(object sender, EventArgs e)
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

            Sex sex;
            Enum.TryParse(cbSex.Text, out sex);
            UserInfoWithPrivateInfo = new UserInfoWithPrivateInfo(tbPassword.Text, tbNick.Text, sex, DateTime.ParseExact(pcBirthdate.Text, "dd MMMM yyyy", null));


            RequestToServerResult result = await _client.UpdateUser(_client.Login, UserInfoWithPrivateInfo);

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
