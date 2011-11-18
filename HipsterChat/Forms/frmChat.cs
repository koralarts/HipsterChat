using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using agsXMPP;
using agsXMPP.protocol;
using agsXMPP.protocol.client;
using agsXMPP.Collections;

namespace MiniClient
{
	/// <summary>
	/// 
	/// </summary>
	public class frmChat : System.Windows.Forms.Form
	{
				
		private System.ComponentModel.Container components = null;

		private XmppClientConnection	_connection;
        private Jid m_Jid;
        private System.Windows.Forms.RichTextBox rtfChat;
        private Button cmdSend1;
        private TextBox rtfSend;
        private Panel panel1;
        private Button closeButton;
		private string					_nickname;

		
		public frmChat(Jid jid, XmppClientConnection con, string nickname)
		{
			m_Jid		= jid;
			_connection = con;
			_nickname	= nickname;

			InitializeComponent();
			
			this.Text = "Chat with " + nickname;
            Util.ChatForms.Add(m_Jid.Bare.ToLower(), this);

			// Setup new Message Callback
            con.MessageGrabber.Add(jid, new BareJidComparer(), new MessageCB(MessageCallback), null);
		}

        public frmChat(Jid jid, XmppClientConnection con, string nickname, bool privateChat)
        {
            m_Jid = jid;
            _connection = con;
            _nickname = nickname;

            InitializeComponent();

            this.Text = "Chat with " + nickname;
            Util.ChatForms.Add(m_Jid.Bare.ToLower(), this);

            // Setup new Message Callback
            if (privateChat)
                con.MessageGrabber.Add(jid, new BareJidComparer(), new MessageCB(MessageCallback), null);
            else
                con.MessageGrabber.Add(jid, new FullJidComparer(), new MessageCB(MessageCallback), null);
        }

		public Jid Jid
		{
			get { return m_Jid; }
			set { m_Jid = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
			
			Util.ChatForms.Remove(m_Jid.Bare.ToLower());
            _connection.MessageGrabber.Remove(m_Jid);
			_connection = null;
		}

		#region Form-Designer Code
		
		private void InitializeComponent()
		{
            this.rtfChat = new System.Windows.Forms.RichTextBox();
            this.cmdSend1 = new System.Windows.Forms.Button();
            this.rtfSend = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtfSend
            // 
            this.rtfSend.BackColor = System.Drawing.Color.Black;
            this.rtfSend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtfSend.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.rtfSend.Location = new System.Drawing.Point(2, 2);
            this.rtfSend.Multiline = true;
            this.rtfSend.Name = "rtfSend";
            this.rtfSend.Size = new System.Drawing.Size(223, 52);
            this.rtfSend.TabIndex = 12;
            // 
            // rtfChat
            // 
            this.rtfChat.BackColor = System.Drawing.Color.Black;
            this.rtfChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtfChat.Location = new System.Drawing.Point(21, 22);
            this.rtfChat.Name = "rtfChat";
            this.rtfChat.ReadOnly = true;
            this.rtfChat.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtfChat.Size = new System.Drawing.Size(458, 225);
            this.rtfChat.TabIndex = 10;
            this.rtfChat.Text = "";
            // 
            // cmdSend1
            // 
            this.cmdSend1.BackColor = System.Drawing.Color.Transparent;
            this.cmdSend1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdSend1.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.cmdSend1.Location = new System.Drawing.Point(102, 274);
            this.cmdSend1.Name = "cmdSend1";
            this.cmdSend1.Size = new System.Drawing.Size(75, 23);
            this.cmdSend1.TabIndex = 11;
            this.cmdSend1.Text = "Send";
            this.cmdSend1.UseVisualStyleBackColor = false;
            this.cmdSend1.Click += new System.EventHandler(this.cmdSend_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkTurquoise;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rtfSend);
            this.panel1.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.panel1.Location = new System.Drawing.Point(253, 271);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(229, 58);
            this.panel1.TabIndex = 13;
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.closeButton.Location = new System.Drawing.Point(21, 274);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 14;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // frmChat
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = global::MiniClient.Properties.Resources.chatbg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cmdSend1);
            this.Controls.Add(this.rtfChat);
            this.DoubleBuffered = true;
            this.Name = "frmChat";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmChat_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.Width = BackgroundImage.Width;
            this.Height = BackgroundImage.Height;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(0, 255, 0);
		}
		#endregion

		private void OutgoingMessage(agsXMPP.protocol.client.Message msg)
		{
			rtfChat.SelectionColor = Color.White;
			rtfChat.AppendText("Me said: ");
			rtfChat.SelectionColor = Color.DarkTurquoise;
			rtfChat.AppendText(msg.Body);
			rtfChat.AppendText("\r\n");
		}

		public void IncomingMessage(agsXMPP.protocol.client.Message msg)
		{
			rtfChat.SelectionColor = Color.Red;
			rtfChat.AppendText(_nickname + " said: ");
			rtfChat.SelectionColor = Color.DarkTurquoise;
			rtfChat.AppendText(msg.Body);
			rtfChat.AppendText("\r\n");
		}

		private void cmdSend_Click(object sender, System.EventArgs e)
		{
			agsXMPP.protocol.client.Message msg = new agsXMPP.protocol.client.Message();

			msg.Type	= MessageType.chat;
			msg.To		= m_Jid;
			msg.Body	= rtfSend.Text;

            if (msg.Body != null)
            {
                _connection.Send(msg);
                OutgoingMessage(msg);
            }

			rtfSend.Text = "";
		}

		private void MessageCallback(object sender, agsXMPP.protocol.client.Message msg, object data)
		{
            if (InvokeRequired)
            {
                // Windows Forms are not Thread Safe, we need to invoke this :(
                // We're not in the UI thread, so we need to call BeginInvoke				
                BeginInvoke(new MessageCB(MessageCallback), new object[] { sender, msg, data });
                return;
            }
            
            if (msg.Body != null)
			    IncomingMessage(msg);
		}

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute ("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, 
        int Msg, int wParam, int lParam);
          
        [DllImportAttribute ("user32.dll")]
        public static extern bool ReleaseCapture();

        private void frmChat_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
	}
}
