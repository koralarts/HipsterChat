using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using agsXMPP;
using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

using agsXMPP.protocol.iq.disco;


namespace HipsterClient
{
	/// <summary>
	/// Summary for frmLogin.
	/// </summary>
	public class frmLogin : System.Windows.Forms.Form
    {
		private System.Windows.Forms.TextBox txtJid;
		private System.Windows.Forms.Button cmdLogin;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.TextBox txtPassword;
        private System.ComponentModel.Container components = null;
        private CheckBox chkRegister;

		private XmppClientConnection _connection;

		public frmLogin(XmppClientConnection con)
		{			
			InitializeComponent();

		
			this.DialogResult = DialogResult.Cancel;
			_connection = con;
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
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.txtJid = new System.Windows.Forms.TextBox();
            this.cmdLogin = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkRegister = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtJid
            // 
            this.txtJid.BackColor = System.Drawing.Color.Black;
            this.txtJid.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.txtJid.Location = new System.Drawing.Point(112, 46);
            this.txtJid.Name = "txtJid";
            this.txtJid.Size = new System.Drawing.Size(142, 20);
            this.txtJid.TabIndex = 0;
            // 
            // cmdLogin
            // 
            this.cmdLogin.BackColor = System.Drawing.Color.Transparent;
            this.cmdLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.cmdLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.cmdLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdLogin.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.cmdLogin.Location = new System.Drawing.Point(138, 120);
            this.cmdLogin.Name = "cmdLogin";
            this.cmdLogin.Size = new System.Drawing.Size(88, 24);
            this.cmdLogin.TabIndex = 6;
            this.cmdLogin.Text = "Login";
            this.cmdLogin.UseVisualStyleBackColor = false;
            this.cmdLogin.Click += new System.EventHandler(this.cmdLogin_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Transparent;
            this.cmdCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.cmdCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.cmdCancel.Location = new System.Drawing.Point(44, 120);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(88, 24);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.Black;
            this.txtPassword.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.txtPassword.Location = new System.Drawing.Point(112, 72);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = ' ';
            this.txtPassword.Size = new System.Drawing.Size(142, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // chkRegister
            // 
            this.chkRegister.BackColor = System.Drawing.Color.Transparent;
            this.chkRegister.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chkRegister.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.chkRegister.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.chkRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRegister.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.chkRegister.Location = new System.Drawing.Point(86, 98);
            this.chkRegister.Name = "chkRegister";
            this.chkRegister.Size = new System.Drawing.Size(160, 16);
            this.chkRegister.TabIndex = 11;
            this.chkRegister.Text = "Register A New Account";
            this.chkRegister.UseVisualStyleBackColor = false;
            // 
            // frmLogin
            // 
            this.AcceptButton = this.cmdLogin;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = global::HipsterClient.Properties.Resources.loginBg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(271, 161);
            this.ControlBox = false;
            this.Controls.Add(this.chkRegister);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtJid);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdLogin);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
		}
		#endregion

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void cmdLogin_Click(object sender, System.EventArgs e)
		{
            _connection.Server                      = getServerName();
            _connection.Username                    = txtJid.Text;
			_connection.Password		            = txtPassword.Text;
            _connection.Resource                    = "HipsterChat";
            _connection.Priority                    = 10;
            _connection.Port                        = 5222;
            _connection.UseSSL                      = false;
            _connection.AutoResolveConnectServer    = true;
            _connection.UseCompression              = false;   

            if (chkRegister.Checked)                
                _connection.RegisterAccount = true;            
            else
                _connection.RegisterAccount = false;
            
            SetDiscoInfo();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

        private void SetDiscoInfo()
        {
            _connection.DiscoInfo.AddIdentity(new DiscoIdentity("pc", "HipsterChat", "client"));
            _connection.DiscoInfo.AddFeature(new DiscoFeature(agsXMPP.Uri.DISCO_INFO));
            _connection.DiscoInfo.AddFeature(new DiscoFeature(agsXMPP.Uri.DISCO_ITEMS));
            _connection.DiscoInfo.AddFeature(new DiscoFeature(agsXMPP.Uri.MUC));
        }

		private string SettingsFilename
		{
			get
			{
				string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				return path + @"\Settings.xml";
			}
		}

        public String getServerName() {
            string fileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            fileName += @"\chatservers.xml";
            Document doc = new Document();
            doc.LoadFile(fileName);
            return doc.RootElement.SelectSingleElement("Login").Value;
        }

	}
}