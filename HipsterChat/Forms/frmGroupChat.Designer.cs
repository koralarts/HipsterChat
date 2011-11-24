namespace HipsterClient
{
    partial class frmGroupChat
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

            Util.GroupChatForms.Remove(m_RoomJid.Bare.ToLower());
            
            // Remove the Message Callback in the MessageGrabber
            m_XmppCon.MessageGrabber.Remove(m_RoomJid);

            // Remove the Presence Callback in the MessageGrabber
            m_XmppCon.PresenceGrabber.Remove(m_RoomJid);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroupChat));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvwRoster = new System.Windows.Forms.ListView();
            this.headerNickname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerRole = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerAffiliation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ilsRoster = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rtfChat = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdChangeSubject = new System.Windows.Forms.Button();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rtfSend = new System.Windows.Forms.RichTextBox();
            this.cmdSend = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.groupChatServerLabel = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(0, 26);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.BackgroundImage = global::HipsterClient.Properties.Resources.groupChatBg;
            this.splitContainer1.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainer1.Panel1.Controls.Add(this.lvwRoster);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(6, 15, 6, 19);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(623, 328);
            this.splitContainer1.SplitterDistance = 188;
            this.splitContainer1.TabIndex = 7;
            // 
            // lvwRoster
            // 
            this.lvwRoster.BackColor = System.Drawing.Color.Black;
            this.lvwRoster.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwRoster.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerNickname,
            this.headerStatus,
            this.headerRole,
            this.headerAffiliation});
            this.lvwRoster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwRoster.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.lvwRoster.Location = new System.Drawing.Point(6, 15);
            this.lvwRoster.Name = "lvwRoster";
            this.lvwRoster.Size = new System.Drawing.Size(176, 294);
            this.lvwRoster.SmallImageList = this.ilsRoster;
            this.lvwRoster.TabIndex = 0;
            this.lvwRoster.UseCompatibleStateImageBehavior = false;
            this.lvwRoster.View = System.Windows.Forms.View.Details;
            // 
            // headerNickname
            // 
            this.headerNickname.Text = "Nickname";
            this.headerNickname.Width = 82;
            // 
            // headerStatus
            // 
            this.headerStatus.Text = "Status";
            this.headerStatus.Width = 73;
            // 
            // headerRole
            // 
            this.headerRole.Text = "Role";
            // 
            // headerAffiliation
            // 
            this.headerAffiliation.Text = "Affiliation";
            // 
            // ilsRoster
            // 
            this.ilsRoster.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilsRoster.ImageStream")));
            this.ilsRoster.TransparentColor = System.Drawing.Color.Transparent;
            this.ilsRoster.Images.SetKeyName(0, "");
            this.ilsRoster.Images.SetKeyName(1, "");
            this.ilsRoster.Images.SetKeyName(2, "");
            this.ilsRoster.Images.SetKeyName(3, "");
            this.ilsRoster.Images.SetKeyName(4, "");
            this.ilsRoster.Images.SetKeyName(5, "");
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.Black;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Panel1.BackgroundImage = global::HipsterClient.Properties.Resources.groupChatBg;
            this.splitContainer2.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer2.Panel1.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(15);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Size = new System.Drawing.Size(431, 328);
            this.splitContainer2.SplitterDistance = 214;
            this.splitContainer2.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 277F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel2.Controls.Add(this.rtfChat, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmdChangeSubject, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtSubject, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(15, 15);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(401, 184);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // rtfChat
            // 
            this.rtfChat.BackColor = System.Drawing.Color.Black;
            this.rtfChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel2.SetColumnSpan(this.rtfChat, 3);
            this.rtfChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfChat.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.rtfChat.Location = new System.Drawing.Point(3, 33);
            this.rtfChat.Name = "rtfChat";
            this.rtfChat.ReadOnly = true;
            this.rtfChat.Size = new System.Drawing.Size(395, 148);
            this.rtfChat.TabIndex = 3;
            this.rtfChat.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 30);
            this.label1.TabIndex = 4;
            this.label1.Text = "Subject:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdChangeSubject
            // 
            this.cmdChangeSubject.BackColor = System.Drawing.Color.Transparent;
            this.cmdChangeSubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdChangeSubject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.cmdChangeSubject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.cmdChangeSubject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdChangeSubject.Location = new System.Drawing.Point(333, 3);
            this.cmdChangeSubject.Name = "cmdChangeSubject";
            this.cmdChangeSubject.Size = new System.Drawing.Size(65, 24);
            this.cmdChangeSubject.TabIndex = 6;
            this.cmdChangeSubject.Text = "Change";
            this.cmdChangeSubject.UseVisualStyleBackColor = false;
            this.cmdChangeSubject.Click += new System.EventHandler(this.cmdChangeSubject_Click);
            // 
            // txtSubject
            // 
            this.txtSubject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubject.BackColor = System.Drawing.Color.Black;
            this.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSubject.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.txtSubject.Location = new System.Drawing.Point(56, 3);
            this.txtSubject.Multiline = true;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(271, 24);
            this.txtSubject.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackgroundImage = global::HipsterClient.Properties.Resources.groupChatBg;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.rtfSend, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmdSend, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(15, 9, 15, 9);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(431, 110);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // rtfSend
            // 
            this.rtfSend.BackColor = System.Drawing.Color.Black;
            this.rtfSend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtfSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfSend.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.rtfSend.Location = new System.Drawing.Point(18, 12);
            this.rtfSend.Name = "rtfSend";
            this.rtfSend.Size = new System.Drawing.Size(395, 56);
            this.rtfSend.TabIndex = 0;
            this.rtfSend.Text = "";
            this.rtfSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtfSend_KeyDown);
            // 
            // cmdSend
            // 
            this.cmdSend.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmdSend.BackColor = System.Drawing.Color.Transparent;
            this.cmdSend.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.cmdSend.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.cmdSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdSend.Location = new System.Drawing.Point(336, 74);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(77, 24);
            this.cmdSend.TabIndex = 1;
            this.cmdSend.Text = "&Send";
            this.cmdSend.UseVisualStyleBackColor = false;
            this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Image = global::HipsterClient.Properties.Resources.close;
            this.closeButton.Location = new System.Drawing.Point(595, 1);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(25, 23);
            this.closeButton.TabIndex = 8;
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            this.closeButton.MouseEnter += new System.EventHandler(this.closeButton_MouseEnter);
            this.closeButton.MouseLeave += new System.EventHandler(this.closeButton_MouseLeave);
            // 
            // groupChatServerLabel
            // 
            this.groupChatServerLabel.AutoSize = true;
            this.groupChatServerLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.groupChatServerLabel.Location = new System.Drawing.Point(6, 7);
            this.groupChatServerLabel.Name = "groupChatServerLabel";
            this.groupChatServerLabel.Size = new System.Drawing.Size(101, 13);
            this.groupChatServerLabel.TabIndex = 9;
            this.groupChatServerLabel.Text = " Group Chat Server:";
            // 
            // frmGroupChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(623, 354);
            this.Controls.Add(this.groupChatServerLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGroupChat";
            this.Text = "Group Chat";
            this.TransparencyKey = System.Drawing.Color.Lime;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGroupChat_FormClosed);
            this.Load += new System.EventHandler(this.frmGroupChat_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGroupChat_MouseDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox rtfSend;
        private System.Windows.Forms.ListView lvwRoster;
        private System.Windows.Forms.ColumnHeader headerNickname;
        private System.Windows.Forms.ColumnHeader headerStatus;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.ImageList ilsRoster;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.RichTextBox rtfChat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdChangeSubject;
        private System.Windows.Forms.ColumnHeader headerRole;
        private System.Windows.Forms.ColumnHeader headerAffiliation;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label groupChatServerLabel;

    }
}