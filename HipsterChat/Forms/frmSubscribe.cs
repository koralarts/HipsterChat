using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using agsXMPP;
using agsXMPP.protocol.client;

namespace HipsterChat
{
	/// <summary>
	/// Zusammenfassung für frmSubscribe.
	/// </summary>
	public class frmSubscribe : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button cmdApprove;
        private System.Windows.Forms.Button cmdRefuse;
		private System.Windows.Forms.Label lblFrom;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private XmppClientConnection	_connection;
		private Jid						_from;

		public frmSubscribe(XmppClientConnection con, Jid jid)
		{
			InitializeComponent();
            
			_connection = con;
			_from		= jid;

			lblFrom.Text	= jid.ToString();
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
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
            this.cmdApprove = new System.Windows.Forms.Button();
            this.cmdRefuse = new System.Windows.Forms.Button();
            this.lblFrom = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdApprove
            // 
            this.cmdApprove.BackColor = System.Drawing.Color.Transparent;
            this.cmdApprove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.cmdApprove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.cmdApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdApprove.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.cmdApprove.Location = new System.Drawing.Point(55, 97);
            this.cmdApprove.Name = "cmdApprove";
            this.cmdApprove.Size = new System.Drawing.Size(72, 24);
            this.cmdApprove.TabIndex = 0;
            this.cmdApprove.Text = "Accept";
            this.cmdApprove.UseVisualStyleBackColor = false;
            this.cmdApprove.Click += new System.EventHandler(this.cmdApprove_Click);
            // 
            // cmdRefuse
            // 
            this.cmdRefuse.BackColor = System.Drawing.Color.Transparent;
            this.cmdRefuse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.cmdRefuse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.cmdRefuse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdRefuse.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.cmdRefuse.Location = new System.Drawing.Point(133, 97);
            this.cmdRefuse.Name = "cmdRefuse";
            this.cmdRefuse.Size = new System.Drawing.Size(72, 24);
            this.cmdRefuse.TabIndex = 1;
            this.cmdRefuse.Text = "Decline";
            this.cmdRefuse.UseVisualStyleBackColor = false;
            this.cmdRefuse.Click += new System.EventHandler(this.cmdRefuse_Click);
            // 
            // lblFrom
            // 
            this.lblFrom.BackColor = System.Drawing.Color.Transparent;
            this.lblFrom.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.lblFrom.Location = new System.Drawing.Point(83, 62);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.lblFrom.Size = new System.Drawing.Size(166, 32);
            this.lblFrom.TabIndex = 3;
            this.lblFrom.Text = "jid";
            // 
            // frmSubscribe
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = global::HipsterChat.Properties.Resources.addReqBg;
            this.ClientSize = new System.Drawing.Size(261, 150);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.cmdRefuse);
            this.Controls.Add(this.cmdApprove);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSubscribe";
            this.Text = "Add Request";
            this.ResumeLayout(false);

		}
		#endregion

		private void cmdRefuse_Click(object sender, System.EventArgs e)
		{			
			PresenceManager pm = new PresenceManager(_connection);
			pm.RefuseSubscriptionRequest(_from);
	
			this.Close();
		}

		private void cmdApprove_Click(object sender, System.EventArgs e)
		{
			PresenceManager pm = new PresenceManager(_connection);
			pm.ApproveSubscriptionRequest(_from);
            _connection.PresenceManager.Subscribe(_from);

			this.Close();
		}
	}
}
