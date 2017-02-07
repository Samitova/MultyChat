using System;
using System.Windows.Forms;
using MMChatEngine;

namespace MMChatServer
{
    public partial class ServerMainForm : Form
    {
        private Server _server;

        public ServerMainForm()
        {
            InitializeComponent();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            _server = new Server(1234);
            rtbLogs.Text += $"{DateTime.Now.ToString()}: Server started{Environment.NewLine}";
            _server.ClientConnected += ServerOnClientConnected;
            _server.UserLogin += ServerOnUserLogin;
            _server.NewUserRegistred += ServerOnNewUserRegistred;
            _server.ClientDisconnected += ServerOnClientDisconnected;
            _server.ErrorOccurred += ServerOnErrorOccurred;
            _server.Start();
        }


        private void ServerOnErrorOccurred(object sender, ErrorOccurredEventHandlerArgs args)
        {
            rtbLogs.Text += $"{DateTime.Now.ToString()}: Error has occured: {args.ErrorMessage}";
        }

        private void ServerOnNewUserRegistred(object sender, NewUserRegisteredEventHandlerArgs args)
        {
            rtbLogs.Text += $"{DateTime.Now.ToString()}: Registred a new user login: {args.Login}, nick: {args.UserInfo.Nick}, sex: {args.UserInfo.Sex}, birthdate: {args.UserInfo.Birthdate}{Environment.NewLine}";
        }

        private void ServerOnUserLogin(object sender, UserLoginEventHandlerArgs args)
        {
            rtbLogs.Text += $"{DateTime.Now.ToString()}: User login: {args.Login}, password: {args.Password}{Environment.NewLine}";
        }

        private void ServerOnClientConnected(object sender, ConnectedEventHandlerArgs args)
        {
            rtbLogs.Text += $"{DateTime.Now.ToString()}: Connected client IP: {args.Ip}{Environment.NewLine}";
        }

        private void ServerOnClientDisconnected(object sender, DisconnectedEventHandlerArgs args)
        {
            rtbLogs.Text += $"{DateTime.Now.ToString()}: Disconnected client {args.UserLogin} IP: {args.Ip}{Environment.NewLine}";

        }
    }
}
