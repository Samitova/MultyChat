namespace MMChatClient
{
    partial class UserInfoForm
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
            this.lbSexCapt = new System.Windows.Forms.Label();
            this.lbNicknameCapt = new System.Windows.Forms.Label();
            this.lbBirthdateCapt = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbNichName = new System.Windows.Forms.Label();
            this.lbSex = new System.Windows.Forms.Label();
            this.lbBirthdate = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbSexCapt
            // 
            this.lbSexCapt.AutoSize = true;
            this.lbSexCapt.Location = new System.Drawing.Point(51, 94);
            this.lbSexCapt.Name = "lbSexCapt";
            this.lbSexCapt.Size = new System.Drawing.Size(40, 20);
            this.lbSexCapt.TabIndex = 0;
            this.lbSexCapt.Text = "Sex:";
            // 
            // lbNicknameCapt
            // 
            this.lbNicknameCapt.AutoSize = true;
            this.lbNicknameCapt.Location = new System.Drawing.Point(51, 38);
            this.lbNicknameCapt.Name = "lbNicknameCapt";
            this.lbNicknameCapt.Size = new System.Drawing.Size(83, 20);
            this.lbNicknameCapt.TabIndex = 1;
            this.lbNicknameCapt.Text = "Nickname:";
            // 
            // lbBirthdateCapt
            // 
            this.lbBirthdateCapt.AutoSize = true;
            this.lbBirthdateCapt.Location = new System.Drawing.Point(51, 145);
            this.lbBirthdateCapt.Name = "lbBirthdateCapt";
            this.lbBirthdateCapt.Size = new System.Drawing.Size(78, 20);
            this.lbBirthdateCapt.TabIndex = 2;
            this.lbBirthdateCapt.Text = "Birthdate:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbBirthdate);
            this.groupBox1.Controls.Add(this.lbSex);
            this.groupBox1.Controls.Add(this.lbNichName);
            this.groupBox1.Controls.Add(this.lbNicknameCapt);
            this.groupBox1.Controls.Add(this.lbBirthdateCapt);
            this.groupBox1.Controls.Add(this.lbSexCapt);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(323, 191);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // lbNichName
            // 
            this.lbNichName.AutoSize = true;
            this.lbNichName.Location = new System.Drawing.Point(171, 38);
            this.lbNichName.Name = "lbNichName";
            this.lbNichName.Size = new System.Drawing.Size(0, 20);
            this.lbNichName.TabIndex = 3;
            // 
            // lbSex
            // 
            this.lbSex.AutoSize = true;
            this.lbSex.Location = new System.Drawing.Point(171, 94);
            this.lbSex.Name = "lbSex";
            this.lbSex.Size = new System.Drawing.Size(0, 20);
            this.lbSex.TabIndex = 4;
            // 
            // lbBirthdate
            // 
            this.lbBirthdate.AutoSize = true;
            this.lbBirthdate.Location = new System.Drawing.Point(171, 145);
            this.lbBirthdate.Name = "lbBirthdate";
            this.lbBirthdate.Size = new System.Drawing.Size(0, 20);
            this.lbBirthdate.TabIndex = 5;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(124, 225);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // UserInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 272);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.Name = "UserInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UserInfoForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbSexCapt;
        private System.Windows.Forms.Label lbNicknameCapt;
        private System.Windows.Forms.Label lbBirthdateCapt;
        private System.Windows.Forms.GroupBox groupBox1;
        protected internal System.Windows.Forms.Label lbBirthdate;
        protected internal System.Windows.Forms.Label lbSex;
        protected internal System.Windows.Forms.Label lbNichName;
        private System.Windows.Forms.Button btnOk;
    }
}