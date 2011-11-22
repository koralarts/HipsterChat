using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using agsXMPP;

namespace MiniClient
{
	/// <summary>
	/// Summary description for femAddRoster.
	/// </summary>
	public class frmAddRoster : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TextBox txtJid;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cmdAdd;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private Button cancelButton;

		private XmppClientConnection _connection;
        
		public frmAddRoster(XmppClientConnection con)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
			_connection = con;
		}

        public frmAddRoster(Jid jid, XmppClientConnection con)
            : this(con)
        {
            txtJid.Text = jid.Bare;
            this.DialogResult = DialogResult.Cancel;
        }

		/// <summary>
		/// Clean up any resources being used.
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.txtJid = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtJid
            // 
            this.txtJid.Location = new System.Drawing.Point(111, 42);
            this.txtJid.Name = "txtJid";
            this.txtJid.Size = new System.Drawing.Size(137, 20);
            this.txtJid.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label2.Location = new System.Drawing.Point(111, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "user@server.org";
            // 
            // cmdAdd
            // 
            this.cmdAdd.BackColor = System.Drawing.Color.Transparent;
            this.cmdAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.cmdAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.cmdAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdAdd.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.cmdAdd.Location = new System.Drawing.Point(56, 108);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(80, 24);
            this.cmdAdd.TabIndex = 12;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.UseVisualStyleBackColor = false;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.cancelButton.Location = new System.Drawing.Point(141, 109);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Canel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // frmAddRoster
            //
            this.AcceptButton = this.cmdAdd;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = Color.Black;
            this.BackgroundImage = global::MiniClient.Properties.Resources.addBg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(272, 161);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.txtJid);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
          //  this.MinimizeBox = false;
            this.Name = "frmAddRoster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Contact";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.FormBorderStyle = FormBorderStyle.None;
		}
		#endregion

		private void cmdAdd_Click(object sender, System.EventArgs e)
		{
			Jid jid = new Jid(txtJid.Text);		
			if (txtJid.Text.Length > 0)
				_connection.RosterManager.AddRosterItem(jid);
			_connection.PresenceManager.Subscribe(jid);
						
			this.Close();
		}

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
	}
}
