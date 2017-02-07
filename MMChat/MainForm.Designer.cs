namespace MMChatClient
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btSend = new System.Windows.Forms.Button();
            this.tcChatWindow = new System.Windows.Forms.TabControl();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.miRegister = new System.Windows.Forms.ToolStripMenuItem();
            this.miUpdateUserInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.miShowUserInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miChat = new System.Windows.Forms.ToolStripMenuItem();
            this.miPrivateChat = new System.Windows.Forms.ToolStripMenuItem();
            this.miMultiChat = new System.Windows.Forms.ToolStripMenuItem();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btSend
            // 
            this.btSend.Enabled = false;
            this.btSend.Location = new System.Drawing.Point(530, 435);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(75, 23);
            this.btSend.TabIndex = 1;
            this.btSend.Text = "Send";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // tcChatWindow
            // 
            this.tcChatWindow.Location = new System.Drawing.Point(4, 31);
            this.tcChatWindow.Name = "tcChatWindow";
            this.tcChatWindow.SelectedIndex = 0;
            this.tcChatWindow.Size = new System.Drawing.Size(528, 397);
            this.tcChatWindow.TabIndex = 2;
            this.tcChatWindow.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tcChatWindow_Selecting);
            this.tcChatWindow.DoubleClick += new System.EventHandler(this.tcChatWindow_DoubleClick);
            // 
            // tbMessage
            // 
            this.tbMessage.Enabled = false;
            this.tbMessage.Location = new System.Drawing.Point(11, 437);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(514, 20);
            this.tbMessage.TabIndex = 3;
            this.tbMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbMessage_KeyDown);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miChat});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(774, 24);
            this.menuStrip.TabIndex = 8;
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLogin,
            this.miRegister,
            this.miUpdateUserInfo,
            this.miShowUserInfo,
            this.toolStripSeparator1,
            this.miExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "File";
            // 
            // miLogin
            // 
            this.miLogin.Name = "miLogin";
            this.miLogin.Size = new System.Drawing.Size(162, 22);
            this.miLogin.Text = "Login";
            this.miLogin.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // miRegister
            // 
            this.miRegister.Name = "miRegister";
            this.miRegister.Size = new System.Drawing.Size(162, 22);
            this.miRegister.Text = "Register";
            this.miRegister.Click += new System.EventHandler(this.registerToolStripMenuItem_Click);
            // 
            // miUpdateUserInfo
            // 
            this.miUpdateUserInfo.Enabled = false;
            this.miUpdateUserInfo.Name = "miUpdateUserInfo";
            this.miUpdateUserInfo.Size = new System.Drawing.Size(162, 22);
            this.miUpdateUserInfo.Text = "Update User Info";
            this.miUpdateUserInfo.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // miShowUserInfo
            // 
            this.miShowUserInfo.Enabled = false;
            this.miShowUserInfo.Name = "miShowUserInfo";
            this.miShowUserInfo.Size = new System.Drawing.Size(162, 22);
            this.miShowUserInfo.Text = "Show User Info";
            this.miShowUserInfo.Click += new System.EventHandler(this.miShowUserInfo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(162, 22);
            this.miExit.Text = "Exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miChat
            // 
            this.miChat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miPrivateChat,
            this.miMultiChat});
            this.miChat.Name = "miChat";
            this.miChat.Size = new System.Drawing.Size(44, 20);
            this.miChat.Text = "Chat";
            // 
            // miPrivateChat
            // 
            this.miPrivateChat.Enabled = false;
            this.miPrivateChat.Name = "miPrivateChat";
            this.miPrivateChat.Size = new System.Drawing.Size(138, 22);
            this.miPrivateChat.Text = "Private Chat";
            this.miPrivateChat.Click += new System.EventHandler(this.miPrivateChat_Click);
            // 
            // miMultiChat
            // 
            this.miMultiChat.Enabled = false;
            this.miMultiChat.Name = "miMultiChat";
            this.miMultiChat.Size = new System.Drawing.Size(138, 22);
            this.miMultiChat.Text = "Multi Chat";
            this.miMultiChat.Click += new System.EventHandler(this.miMultiChat_Click);
            // 
            // lstUsers
            // 
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(538, 33);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstUsers.Size = new System.Drawing.Size(233, 394);
            this.lstUsers.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 464);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.tcChatWindow);
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.TabControl tcChatWindow;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miLogin;
        private System.Windows.Forms.ToolStripMenuItem miShowUserInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miRegister;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem miUpdateUserInfo;
        private System.Windows.Forms.ToolStripMenuItem miChat;
        private System.Windows.Forms.ToolStripMenuItem miPrivateChat;
        private System.Windows.Forms.ToolStripMenuItem miMultiChat;
    }
}

