using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using agsXMPP;
using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

using agsXMPP.protocol.iq.disco;


namespace MiniClient
{
	/// <summary>
	/// Summary for frmLogin.
	/// </summary>
	public class frmLogin : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtJid;
		private System.Windows.Forms.Button cmdLogin;
        private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtJid = new System.Windows.Forms.TextBox();
            this.cmdLogin = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkRegister = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtJid
            // 
            this.txtJid.Location = new System.Drawing.Point(86, 13);
            this.txtJid.Name = "txtJid";
            this.txtJid.Size = new System.Drawing.Size(168, 20);
            this.txtJid.TabIndex = 0;
            // 
            // cmdLogin
            // 
            this.cmdLogin.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdLogin.Location = new System.Drawing.Point(138, 97);
            this.cmdLogin.Name = "cmdLogin";
            this.cmdLogin.Size = new System.Drawing.Size(88, 24);
            this.cmdLogin.TabIndex = 6;
            this.cmdLogin.Text = "Login";
            this.cmdLogin.Click += new System.EventHandler(this.cmdLogin_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdCancel.Location = new System.Drawing.Point(44, 97);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(88, 24);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(86, 40);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(168, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Password:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkRegister
            // 
            this.chkRegister.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkRegister.Location = new System.Drawing.Point(86, 66);
            this.chkRegister.Name = "chkRegister";
            this.chkRegister.Size = new System.Drawing.Size(160, 16);
            this.chkRegister.TabIndex = 11;
            this.chkRegister.Text = "Register A New Account";
            // 
            // frmLogin
            // 
            this.AcceptButton = this.cmdLogin;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(266, 133);
            this.Controls.Add(this.chkRegister);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtJid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdLogin);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void cmdLogin_Click(object sender, System.EventArgs e)
		{
			Jid jid = new Jid(txtJid.Text);

            _connection.Server                      = getServerName();
            _connection.Username                    =txtJid.Text;
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