using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MMChatEngine;

namespace MMChatClient
{
    public partial class MainForm : Form
    {
        private readonly Client _client;

        public MainForm()
        {
            InitializeComponent();

            _client = new Client();
            _client.UserConnected += ClientOnUserConnected;
            _client.SimpleMessageReceived += ClientOnSimpleMessageReceived;
            _client.UserDisconnected += ClientOnUserDisconnected;
            _client.ErrorOccurred += ClientOnErrorOccurred;
            _client.NewRoomCreated += ClientOnNewRoomCreated;
            _client.NegotiationCompleted += ClientOnNegotiationCompleted;
            _client.RoomsUpdated += ClientOnRoomsUpdated;
            _client.RoomClosed += ClientOnRoomClosed;
            _client.UserUpdated += ClientOnUserUpdated;
        }

        private void ClientOnUserUpdated(object sender, EventArgs args)
        {
            UpdateUserList();
        }

        private void ClientOnRoomClosed(object sender, RoomClosedEventHandlerArgs args)
        {
            CloseRoom(args.RoomId);
        }

        private void ClientOnRoomsUpdated(object sender, RoomsUpdatedEventHandlerArgs args)
        {
            UpdateUserList();
        }

        private void ClientOnNegotiationCompleted(object sender, EventArgs args)
        {
            Room room = new Room(Room.MainRoomId, Room.MainRoomName, new HashSet<string>());
            CreateNewRoom(room);
            Text = $"Chat. User: {_client.Login}";
        }

        private void ClientOnNewRoomCreated(object sender, NewRoomCreatedEventHandlerArgs args)
        {
            CreateNewRoom(args.Room);
        }

        private void ClientOnErrorOccurred(object sender, ErrorOccurredEventHandlerArgs args)
        {
            MessageBox.Show(args.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ClientOnUserDisconnected(object sender, UserDisconnectedEventHandlerArgs args)
        {
            UpdateUserList();
        }

        private void ClientOnSimpleMessageReceived(object sender, SimpleMessageRecivedEventHandlerArgs args)
        {
            ((RichTextBox)tcChatWindow.TabPages[args.Room.ToString()]?.Controls[$"rtb{args.Room}"])?.AppendText($"{args.DateTime} {args.UserLogin} : {args.Message}{Environment.NewLine}");
        }

        private void ClientOnUserConnected(object sender, UserConnectedEventHandlerArgs args)
        {
            UpdateUserList();
        }

        private void UpdateUserList()
        {
            Guid roomId;
            lstUsers.DataSource = null;
            if (Guid.TryParse(tcChatWindow.SelectedTab.Name, out roomId) && _client.Rooms.ContainsKey(roomId))
            {
                lstUsers.DataSource = _client.GetRoomUsers(roomId);
                lstUsers.DisplayMember = "Value";
                lstUsers.ValueMember = "Key";
            }
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            if (tbMessage.Text.Length > 0)
            {
                SendSimpleMessage();
            }
        }

        private void SendSimpleMessage()
        {
            var activeRoomId = GetActiveRoomId();
            if (activeRoomId != Guid.Empty)
            {
                _client.SendSimpleMessage(activeRoomId, tbMessage.Text);
                tbMessage.Clear();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_client.IsConnected)
            {
                _client.UserDisconnect();
            }
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && tbMessage.Text.Length > 0)
            {
                SendSimpleMessage();
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login login = new Login(_client);
            if (login.ShowDialog() == DialogResult.OK)
            {
                miLogin.Enabled = false;
                miRegister.Enabled = false;
                miShowUserInfo.Enabled = true;
                miPrivateChat.Enabled = true;
                btSend.Enabled = true;
                tbMessage.Enabled = true;
                miMultiChat.Enabled = true;
            }
        }

        private void miShowUserInfo_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItem != null)
            {
                UserInfoForm infoForm = new UserInfoForm();
                infoForm.Owner = this;
                UserInfo userInfo = _client.GetUserInfo(lstUsers.SelectedValue.ToString());

                infoForm.lbNichName.Text = userInfo.Nick;
                infoForm.lbSex.Text = userInfo.Sex.ToString();
                infoForm.lbBirthdate.Text = userInfo.Birthdate.ToString("dd.MM.yyyy");
                infoForm.Visible = true;

            }
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regiter regiter = new Regiter(_client);
            if (regiter.ShowDialog() == DialogResult.OK)
            {  
                miLogin.Enabled = false;
                miRegister.Enabled = false;
                miShowUserInfo.Enabled = true;
                miPrivateChat.Enabled = true;
                btSend.Enabled = true;
                tbMessage.Enabled = true;
                miMultiChat.Enabled = true;
            }
        }

        private void CreateNewRoom(Room room)
        {
            tcChatWindow.TabPages.Add(room.Id.ToString(), room.Name);
            RichTextBox richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.White,
                Name = $"rtb{room.Id}"
            };
            TabPage tabPage = tcChatWindow.TabPages[room.Id.ToString()];
            tabPage.Controls.Add(richTextBox);
        }

        private void CloseRoom(Guid roomId)
        {
            tcChatWindow.SelectTab(Room.MainRoomId.ToString());
            tcChatWindow.TabPages.RemoveByKey(roomId.ToString());
        }

        private Guid GetActiveRoomId()
        {
            var selectedTabName = tcChatWindow.SelectedTab?.Name;
            Guid guid;
            if (selectedTabName != null && Guid.TryParse(selectedTabName, out guid))
            {
                return guid;
            }
            return Guid.Empty;
        }

        private void tcChatWindow_Selecting(object sender, TabControlCancelEventArgs e)
        {
            UpdateUserList();
        }

        private void tcChatWindow_DoubleClick(object sender, EventArgs e)
        {
            string selectedTabName = tcChatWindow.SelectedTab.Name;
            if (selectedTabName == Room.MainRoomId.ToString())
            {
                return;
            }
            if (MessageBox.Show($"Are you sure to close chat: {tcChatWindow.SelectedTab.Text}", "Chat close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _client.CloseRoom(Guid.Parse(selectedTabName));
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update update = new Update(_client);
            if (update.ShowDialog() == DialogResult.OK)
            {
                
            }
        }

        private void miPrivateChat_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count == 1)
            {
                _client.CreatePrivateChat(lstUsers.SelectedValue.ToString());
            }
        }

        private void miMultiChat_Click(object sender, EventArgs e)
        {
            List<string> users = new List<string>();
            foreach (KeyValuePair<string,string> lstUsersSelectedItem in lstUsers.SelectedItems)
            {
                users.Add(lstUsersSelectedItem.Key);
            }
            MultiChatName multiChatName = new MultiChatName(_client, users);
            multiChatName.ShowDialog();
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
