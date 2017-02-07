using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MMChatEngine;

namespace MMChatClient
{
    public partial class MultiChatName : Form
    {
        private readonly Client _client;
        private readonly IReadOnlyList<string> _users;
        public MultiChatName(Client client, List<string> users)
        {
            InitializeComponent();
            _client = client;
            _users = users;
        }

        private async void btCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Chat name can't be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RequestToServerResult result = await _client.CreateMultiChat(tbName.Text, _users);

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
