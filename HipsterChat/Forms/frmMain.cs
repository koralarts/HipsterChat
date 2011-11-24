using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;

using agsXMPP;
using agsXMPP.protocol;
using agsXMPP.protocol.iq;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.version;
using agsXMPP.protocol.iq.oob;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.shim;
using agsXMPP.protocol.extensions.si;
using agsXMPP.protocol.extensions.bytestreams;

using agsXMPP.protocol.x;
using agsXMPP.protocol.x.data;

using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

using agsXMPP.sasl;

using agsXMPP.ui;
using agsXMPP.ui.roster;

using System.Security.Cryptography;
using System.Text;

using agsXMPP.protocol.stream.feature.compression;

namespace HipsterClient
{
    /// <summary>
    /// MainForm
    /// </summary>
    public class frmMain : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components;


        private ContextMenuStrip contextMenuGC;
        private ContextMenuStrip contextMenuStripRoster;
        private ToolStripMenuItem chatToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem disconnectToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem joinToolStripMenuItem;

        private ToolStripMenuItem sendFileToolStripMenuItem;

        delegate void OnMessageDelegate(object sender, agsXMPP.protocol.client.Message msg);
        delegate void OnPresenceDelegate(object sender, Presence pres);

        const int IMAGE_PARTICIPANT = 3;
        const int IMAGE_CHATROOM = 4;
        const int IMAGE_SERVER = 5;

        private XmppClientConnection XmppCon;
        private Label statusBar1;
        private Button closeButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem addContactToolStripMenuItem;
        private ToolStripMenuItem searchContactToolStripMenuItem;
        private Button contactsButton;
        private Button groupChatButton;
        private ComboBox cboStatus;
        private RosterControl rosterControl;
        private Panel contactListPanel;
        private ToolStrip miniToolStrip;
        private ToolStripButton toolStripButtonFindRooms;
        private ToolStripButton toolStripButtonFindPart;
        private Panel groupChatPanel;
        private TreeView treeGC;
        private Button serverRefreshButton;
        private Button findParticipantsButton;
        //private DiscoHelper discoHelper;
        DiscoManager discoManager;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
        int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public frmMain()
        {
            InitializeComponent();
            treeGC.ContextMenuStrip = contextMenuGC;
            contactListPanel.BringToFront();

            // initialize Combo Status
            cboStatus.Items.AddRange(new object[] {"offline",
													ShowType.away.ToString(),
													ShowType.xa.ToString(),
													ShowType.chat.ToString(),
													ShowType.dnd.ToString(),
													"online" });
            cboStatus.SelectedIndex = 0;

            // initilaize XmppConnection
            XmppCon = new XmppClientConnection();

            XmppCon.SocketConnectionType = agsXMPP.net.SocketConnectionType.Direct;

            XmppCon.OnReadXml += new XmlHandler(XmppCon_OnReadXml);
            XmppCon.OnWriteXml += new XmlHandler(XmppCon_OnWriteXml);

            XmppCon.OnRosterStart += new ObjectHandler(XmppCon_OnRosterStart);
            XmppCon.OnRosterEnd += new ObjectHandler(XmppCon_OnRosterEnd);
            XmppCon.OnRosterItem += new agsXMPP.XmppClientConnection.RosterHandler(XmppCon_OnRosterItem);

            XmppCon.OnAgentStart += new ObjectHandler(XmppCon_OnAgentStart);
            XmppCon.OnAgentEnd += new ObjectHandler(XmppCon_OnAgentEnd);
            XmppCon.OnAgentItem += new agsXMPP.XmppClientConnection.AgentHandler(XmppCon_OnAgentItem);

            XmppCon.OnLogin += new ObjectHandler(XmppCon_OnLogin);
            XmppCon.OnClose += new ObjectHandler(XmppCon_OnClose);
            XmppCon.OnError += new ErrorHandler(XmppCon_OnError);
            XmppCon.OnPresence += new PresenceHandler(XmppCon_OnPresence);
            XmppCon.OnMessage += new MessageHandler(XmppCon_OnMessage);
            XmppCon.OnIq += new IqHandler(XmppCon_OnIq);
            XmppCon.OnAuthError += new XmppElementHandler(XmppCon_OnAuthError);
            XmppCon.OnSocketError += new ErrorHandler(XmppCon_OnSocketError);
            XmppCon.OnStreamError += new XmppElementHandler(XmppCon_OnStreamError);


            XmppCon.OnReadSocketData += new agsXMPP.net.BaseSocket.OnSocketDataHandler(ClientSocket_OnReceive);
            XmppCon.OnWriteSocketData += new agsXMPP.net.BaseSocket.OnSocketDataHandler(ClientSocket_OnSend);

            XmppCon.ClientSocket.OnValidateCertificate += new System.Net.Security.RemoteCertificateValidationCallback(ClientSocket_OnValidateCertificate);


            XmppCon.OnXmppConnectionStateChanged += new XmppConnectionStateHandler(XmppCon_OnXmppConnectionStateChanged);
            XmppCon.OnSaslStart += new SaslEventHandler(XmppCon_OnSaslStart);

            discoManager = new DiscoManager(XmppCon);

            agsXMPP.Factory.ElementFactory.AddElementType("Login", null, typeof(Settings.Login));
            LoadChatServers();

            frmLogin f = new frmLogin(XmppCon);

            if (f.ShowDialog() == DialogResult.OK)
            {
                XmppCon.Open();
            }
        }

        void XmppCon_OnStreamError(object sender, Element e)
        {
            // Stream errors <stream:error/>
        }

        private void XmppCon_OnSocketError(object sender, Exception ex)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ErrorHandler(XmppCon_OnSocketError), new object[] { sender, ex });
                return;
            }

            MessageBox.Show("Socket Error\r\n" + ex.Message + "\r\n" + ex.InnerException);
        }

        private void XmppCon_OnSaslStart(object sender, SaslEventArgs args)
        {
            // You can define the SASL mechanism here when needed, or implement your own SASL mechanisms
            // for authentication

            //args.Auto = false;
            //args.Mechanism = agsXMPP.protocol.sasl.Mechanism.GetMechanismName(agsXMPP.protocol.sasl.MechanismType.PLAIN);            


            //args.Auto = false;
            //args.Mechanism = agsXMPP.protocol.sasl.Mechanism.GetMechanismName(agsXMPP.protocol.sasl.MechanismType.ANONYMOUS);
        }

        private void LoadChatServers()
        {
            treeGC.TreeViewNodeSorter = new TreeNodeSorter();

            string fileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            fileName += @"\chatservers.xml";

            Document doc = new Document();
            doc.LoadFile(fileName);

            // Get Servers
            ElementList servers = doc.RootElement.SelectElements("Server");
            foreach (Element server in servers)
            {
                TreeNode n = new TreeNode(server.Value);
                n.Tag = "server";
                n.ImageIndex = n.SelectedImageIndex = IMAGE_SERVER;

                this.treeGC.Nodes.Add(n);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.contextMenuGC = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.joinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripRoster = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.chatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addContactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchContactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar1 = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.contactsButton = new System.Windows.Forms.Button();
            this.groupChatButton = new System.Windows.Forms.Button();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.contactListPanel = new System.Windows.Forms.Panel();
            this.rosterControl = new agsXMPP.ui.roster.RosterControl();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonFindRooms = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFindPart = new System.Windows.Forms.ToolStripButton();
            this.groupChatPanel = new System.Windows.Forms.Panel();
            this.findParticipantsButton = new System.Windows.Forms.Button();
            this.serverRefreshButton = new System.Windows.Forms.Button();
            this.treeGC = new System.Windows.Forms.TreeView();
            this.contextMenuGC.SuspendLayout();
            this.contextMenuStripRoster.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contactListPanel.SuspendLayout();
            this.groupChatPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuGC
            // 
            this.contextMenuGC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.joinToolStripMenuItem});
            this.contextMenuGC.Name = "contextMenuGC";
            this.contextMenuGC.Size = new System.Drawing.Size(96, 26);
            // 
            // joinToolStripMenuItem
            // 
            this.joinToolStripMenuItem.BackColor = System.Drawing.Color.Azure;
            this.joinToolStripMenuItem.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.joinToolStripMenuItem.Name = "joinToolStripMenuItem";
            this.joinToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.joinToolStripMenuItem.Text = "Join";
            this.joinToolStripMenuItem.ToolTipText = "Join Server";
            this.joinToolStripMenuItem.Click += new System.EventHandler(this.joinToolStripMenuItem_Click);
            // 
            // contextMenuStripRoster
            // 
            this.contextMenuStripRoster.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chatToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.sendFileToolStripMenuItem});
            this.contextMenuStripRoster.Name = "contextMenuStripRoster";
            this.contextMenuStripRoster.Size = new System.Drawing.Size(169, 92);
            // 
            // chatToolStripMenuItem
            // 

            this.chatToolStripMenuItem.BackColor = System.Drawing.Color.Azure;
            this.chatToolStripMenuItem.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.chatToolStripMenuItem.Image = global::HipsterClient.Properties.Resources.comment;
            this.chatToolStripMenuItem.Name = "chatToolStripMenuItem";
            this.chatToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.chatToolStripMenuItem.Text = "Chat";
            this.chatToolStripMenuItem.ToolTipText = "Begin Chat with Contact";
            this.chatToolStripMenuItem.Click += new System.EventHandler(this.chatToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.BackColor = System.Drawing.Color.Azure;
            this.deleteToolStripMenuItem.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.deleteToolStripMenuItem.Image = global::HipsterClient.Properties.Resources.user_delete;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.ToolTipText = "Delete Contact";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // sendFileToolStripMenuItem
            // 
            this.sendFileToolStripMenuItem.BackColor = System.Drawing.Color.Azure;
            this.sendFileToolStripMenuItem.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.sendFileToolStripMenuItem.Image = global::HipsterClient.Properties.Resources.package;
            this.sendFileToolStripMenuItem.Name = "sendFileToolStripMenuItem";
            this.sendFileToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.sendFileToolStripMenuItem.Text = "Send File";
            this.sendFileToolStripMenuItem.ToolTipText = "Send File to Contact";
            this.sendFileToolStripMenuItem.Click += new System.EventHandler(this.sendFileToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.Black;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(26, 13);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(45, 26);
            this.menuStrip1.TabIndex = 5;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.toolStripSeparator1,
            this.addContactToolStripMenuItem,
            this.searchContactToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.BackColor = System.Drawing.Color.Azure;
            this.connectToolStripMenuItem.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.connectToolStripMenuItem.Image = global::HipsterClient.Properties.Resources.connect;
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.BackColor = System.Drawing.Color.Azure;
            this.disconnectToolStripMenuItem.Enabled = false;
            this.disconnectToolStripMenuItem.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.disconnectToolStripMenuItem.Image = global::HipsterClient.Properties.Resources.disconnect;
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
            // 
            // addContactToolStripMenuItem
            // 
            this.addContactToolStripMenuItem.BackColor = System.Drawing.Color.Azure;
            this.addContactToolStripMenuItem.Enabled = false;
            this.addContactToolStripMenuItem.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.addContactToolStripMenuItem.Image = global::HipsterClient.Properties.Resources.user_add;
            this.addContactToolStripMenuItem.Name = "addContactToolStripMenuItem";
            this.addContactToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.addContactToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.addContactToolStripMenuItem.Text = "Add Contact";
            this.addContactToolStripMenuItem.Click += new System.EventHandler(this.addContactToolStripMenuItem_Click);
            // 
            // searchContactToolStripMenuItem
            // 
            this.searchContactToolStripMenuItem.BackColor = System.Drawing.Color.Azure;
            this.searchContactToolStripMenuItem.Enabled = false;
            this.searchContactToolStripMenuItem.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.searchContactToolStripMenuItem.Image = global::HipsterClient.Properties.Resources.zoom;
            this.searchContactToolStripMenuItem.Name = "searchContactToolStripMenuItem";
            this.searchContactToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.searchContactToolStripMenuItem.Text = "Search Contact";
            this.searchContactToolStripMenuItem.Click += new System.EventHandler(this.searchContactToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.BackColor = System.Drawing.Color.Azure;
            this.toolStripSeparator2.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(176, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.BackColor = System.Drawing.Color.Azure;
            this.exitToolStripMenuItem.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.exitToolStripMenuItem.Image = global::HipsterClient.Properties.Resources.door_in;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusBar1
            // 
            this.statusBar1.AutoSize = true;
            this.statusBar1.BackColor = System.Drawing.Color.Transparent;
            this.statusBar1.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.statusBar1.Location = new System.Drawing.Point(27, 453);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(37, 13);
            this.statusBar1.TabIndex = 10;
            this.statusBar1.Text = "Offline";
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.BackgroundImage = global::HipsterClient.Properties.Resources.close;
            this.closeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.closeButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.ForeColor = System.Drawing.Color.Red;
            this.closeButton.Location = new System.Drawing.Point(346, 14);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(23, 23);
            this.closeButton.TabIndex = 11;
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.button1_Click);
            this.closeButton.MouseEnter += new System.EventHandler(this.closeButton_mouseEnter);
            this.closeButton.MouseLeave += new System.EventHandler(this.closeButton_mouseLeave);
            // 
            // contactsButton
            // 
            this.contactsButton.BackColor = System.Drawing.Color.Transparent;
            this.contactsButton.Enabled = false;
            this.contactsButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkTurquoise;
            this.contactsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.contactsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.contactsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.contactsButton.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.contactsButton.Location = new System.Drawing.Point(14, 47);
            this.contactsButton.Name = "contactsButton";
            this.contactsButton.Size = new System.Drawing.Size(75, 23);
            this.contactsButton.TabIndex = 12;
            this.contactsButton.Text = "Contacts";
            this.contactsButton.UseVisualStyleBackColor = false;
            this.contactsButton.Click += new System.EventHandler(this.contactsButton_Click);
            // 
            // groupChatButton
            // 
            this.groupChatButton.BackColor = System.Drawing.Color.Transparent;
            this.groupChatButton.Enabled = false;
            this.groupChatButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkTurquoise;
            this.groupChatButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.groupChatButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.groupChatButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupChatButton.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.groupChatButton.Location = new System.Drawing.Point(91, 47);
            this.groupChatButton.Name = "groupChatButton";
            this.groupChatButton.Size = new System.Drawing.Size(75, 23);
            this.groupChatButton.TabIndex = 13;
            this.groupChatButton.Text = "Group Chat";
            this.groupChatButton.UseVisualStyleBackColor = false;
            this.groupChatButton.Click += new System.EventHandler(this.groupChatButton_Click);
            // 
            // cboStatus
            // 
            this.cboStatus.BackColor = System.Drawing.Color.Black;
            this.cboStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboStatus.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.cboStatus.Location = new System.Drawing.Point(0, 0);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(358, 21);
            this.cboStatus.TabIndex = 14;
            this.cboStatus.SelectedValueChanged += new System.EventHandler(this.cboStatus_SelectedValueChanged);
            // 
            // contactListPanel
            // 
            this.contactListPanel.BackColor = System.Drawing.Color.Transparent;
            this.contactListPanel.Controls.Add(this.rosterControl);
            this.contactListPanel.Controls.Add(this.cboStatus);
            this.contactListPanel.Enabled = false;
            this.contactListPanel.Location = new System.Drawing.Point(13, 75);
            this.contactListPanel.Name = "contactListPanel";
            this.contactListPanel.Size = new System.Drawing.Size(358, 368);
            this.contactListPanel.TabIndex = 15;
            // 
            // rosterControl
            // 
            this.rosterControl.BackColor = System.Drawing.Color.White;
            this.rosterControl.ColorGroup = System.Drawing.Color.DarkTurquoise;
            this.rosterControl.ColorResource = System.Drawing.Color.Black;
            this.rosterControl.ColorRoot = System.Drawing.Color.CadetBlue;
            this.rosterControl.ColorRoster = System.Drawing.Color.Black;
            this.rosterControl.DefaultGroupName = "Ungrouped";
            this.rosterControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rosterControl.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.rosterControl.HideEmptyGroups = true;
            this.rosterControl.Location = new System.Drawing.Point(0, 21);
            this.rosterControl.Name = "rosterControl";
            this.rosterControl.Size = new System.Drawing.Size(358, 347);
            this.rosterControl.TabIndex = 15;
            this.rosterControl.SelectionChanged += new System.EventHandler(this.rosterControl_SelectionChanged);
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.Location = new System.Drawing.Point(214, 3);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(333, 25);
            this.miniToolStrip.TabIndex = 19;
            this.miniToolStrip.Visible = false;
            // 
            // toolStripButtonFindRooms
            // 
            this.toolStripButtonFindRooms.Image = global::HipsterClient.Properties.Resources.comments;
            this.toolStripButtonFindRooms.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFindRooms.Name = "toolStripButtonFindRooms";
            this.toolStripButtonFindRooms.Size = new System.Drawing.Size(90, 22);
            this.toolStripButtonFindRooms.Text = "Find Rooms";
            // 
            // toolStripButtonFindPart
            // 
            this.toolStripButtonFindPart.Image = global::HipsterClient.Properties.Resources.group;
            this.toolStripButtonFindPart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFindPart.Name = "toolStripButtonFindPart";
            this.toolStripButtonFindPart.Size = new System.Drawing.Size(115, 22);
            this.toolStripButtonFindPart.Text = "Find Participants";
            // 
            // groupChatPanel
            // 
            this.groupChatPanel.BackColor = System.Drawing.Color.Transparent;
            this.groupChatPanel.Controls.Add(this.findParticipantsButton);
            this.groupChatPanel.Controls.Add(this.serverRefreshButton);
            this.groupChatPanel.Controls.Add(this.treeGC);
            this.groupChatPanel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.groupChatPanel.Location = new System.Drawing.Point(13, 75);
            this.groupChatPanel.Name = "groupChatPanel";
            this.groupChatPanel.Size = new System.Drawing.Size(358, 368);
            this.groupChatPanel.TabIndex = 16;
            // 
            // findParticipantsButton
            // 
            this.findParticipantsButton.BackgroundImage = global::HipsterClient.Properties.Resources.zoom;
            this.findParticipantsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.findParticipantsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.findParticipantsButton.FlatAppearance.BorderSize = 0;
            this.findParticipantsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.findParticipantsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.findParticipantsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.findParticipantsButton.Location = new System.Drawing.Point(308, 5);
            this.findParticipantsButton.Name = "findParticipantsButton";
            this.findParticipantsButton.Size = new System.Drawing.Size(23, 23);
            this.findParticipantsButton.TabIndex = 2;
            this.findParticipantsButton.UseVisualStyleBackColor = true;
            this.findParticipantsButton.Click += new System.EventHandler(this.findParticipantsButton_Click);
            // 
            // serverRefreshButton
            // 
            this.serverRefreshButton.BackgroundImage = global::HipsterClient.Properties.Resources.refresh;
            this.serverRefreshButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.serverRefreshButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.serverRefreshButton.FlatAppearance.BorderSize = 0;
            this.serverRefreshButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.serverRefreshButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.serverRefreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.serverRefreshButton.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.serverRefreshButton.Location = new System.Drawing.Point(333, 5);
            this.serverRefreshButton.Name = "serverRefreshButton";
            this.serverRefreshButton.Size = new System.Drawing.Size(23, 23);
            this.serverRefreshButton.TabIndex = 1;
            this.serverRefreshButton.UseVisualStyleBackColor = true;
            this.serverRefreshButton.Click += new System.EventHandler(this.serverRefreshButton_Click);
            // 
            // treeGC
            // 
            this.treeGC.BackColor = System.Drawing.Color.Black;
            this.treeGC.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeGC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeGC.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.treeGC.Location = new System.Drawing.Point(0, 0);
            this.treeGC.Name = "treeGC";
            this.treeGC.Size = new System.Drawing.Size(358, 368);
            this.treeGC.TabIndex = 0;
            this.treeGC.DoubleClick += new System.EventHandler(this.treeGC_DoubleClick);
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::HipsterClient.Properties.Resources.mainFrmBg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(385, 485);
            this.Controls.Add(this.groupChatPanel);
            this.Controls.Add(this.contactListPanel);
            this.Controls.Add(this.groupChatButton);
            this.Controls.Add(this.contactsButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(9999999, 9999999);
            this.MinimumSize = new System.Drawing.Size(363, 464);
            this.Name = "frmMain";
            this.Text = "Hipster Chat";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.contextMenuGC.ResumeLayout(false);
            this.contextMenuStripRoster.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contactListPanel.ResumeLayout(false);
            this.groupChatPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new frmMain());
        }

        #region << XmppConnection events >>
        private void XmppCon_OnReadXml(object sender, string xml)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new XmlHandler(XmppCon_OnReadXml), new object[] { sender, xml });
                return;
            }
        }

        private void XmppCon_OnWriteXml(object sender, string xml)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new XmlHandler(XmppCon_OnWriteXml), new object[] { sender, xml });
                return;
            }
        }

        private void XmppCon_OnRosterStart(object sender)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ObjectHandler(XmppCon_OnRosterStart), new object[] { this });
                return;
            }
            // Disable redraw for faster updating
            rosterControl.BeginUpdate();
        }

        private void XmppCon_OnRosterEnd(object sender)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ObjectHandler(XmppCon_OnRosterEnd), new object[] { this });
                return;
            }
            // enable redraw again
            rosterControl.EndUpdate();
            rosterControl.ExpandAll();

            cboStatus.Text = "online";
        }

        private void XmppCon_OnRosterItem(object sender, agsXMPP.protocol.iq.roster.RosterItem item)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new agsXMPP.XmppClientConnection.RosterHandler(XmppCon_OnRosterItem), new object[] { this, item });
                return;
            }

            if (item.Subscription != SubscriptionType.remove)
            {
                rosterControl.AddRosterItem(item);
            }
            else
            {
                rosterControl.RemoveRosterItem(item);
            }

        }

        private void XmppCon_OnAgentStart(object sender)
        {

        }

        private void XmppCon_OnAgentEnd(object sender)
        {

        }

        private void XmppCon_OnAgentItem(object sender, agsXMPP.protocol.iq.agent.Agent agent)
        {

        }

        private void XmppCon_OnLogin(object sender)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ObjectHandler(XmppCon_OnLogin), new object[] { sender });
                return;
            }
            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = true;
            addContactToolStripMenuItem.Enabled = true;
            searchContactToolStripMenuItem.Enabled = true;
            contactListPanel.Enabled = true;
            contactsButton.Enabled = true;
            groupChatButton.Enabled = true;
            statusBar1.Text = "Online";
            this.Text = "HipsterChat - Online";

            DiscoServer();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XmppCon_OnAuthError(object sender, Element e)
        {
            if (InvokeRequired)
            {
                // Windows Forms are not Thread Safe, we need to invoke this :(
                // We're not in the UI thread, so we need to call BeginInvoke				
                BeginInvoke(new XmppElementHandler(XmppCon_OnAuthError), new object[] { sender, e });
                return;
            }

            if (XmppCon.XmppConnectionState != XmppConnectionState.Disconnected)
                XmppCon.Close();

            MessageBox.Show("Authentication Error!\r\nWrong password or username.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pres"></param>
        private void XmppCon_OnPresence(object sender, Presence pres)
        {
            if (InvokeRequired)
            {
                // Windows Forms are not Thread Safe, we need to invoke this :(
                // We're not in the UI thread, so we need to call BeginInvoke				
                BeginInvoke(new OnPresenceDelegate(XmppCon_OnPresence), new object[] { sender, pres });
                return;
            }

            if (pres.Type == PresenceType.subscribe)
            {
                frmSubscribe f = new frmSubscribe(XmppCon, pres.From);
                f.Show();
            }
            else if (pres.Type == PresenceType.subscribed)
            {

            }
            else if (pres.Type == PresenceType.unsubscribe)
            {

            }
            else if (pres.Type == PresenceType.unsubscribed)
            {

            }
            else
            {
                try
                {
                    rosterControl.SetPresence(pres);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        private void XmppCon_OnIq(object sender, agsXMPP.protocol.client.IQ iq)
        {
            if (InvokeRequired)
            {
                // Windows Forms are not Thread Safe, we need to invoke this :(
                // We're not in the UI thread, so we need to call BeginInvoke				
                BeginInvoke(new IqHandler(XmppCon_OnIq), new object[] { sender, iq });
                return;
            }


            if (iq != null)
            {
                // No Iq with query
                if (iq.HasTag(typeof(agsXMPP.protocol.extensions.si.SI)))
                {
                    if (iq.Type == IqType.set)
                    {
                        agsXMPP.protocol.extensions.si.SI si = iq.SelectSingleElement(typeof(agsXMPP.protocol.extensions.si.SI)) as agsXMPP.protocol.extensions.si.SI;

                        agsXMPP.protocol.extensions.filetransfer.File file = si.File;
                        if (file != null)
                        {
                            // somebody wants to send a file to us
                            Console.WriteLine(file.Size.ToString());
                            Console.WriteLine(file.Name);
                            frmFileTransfer frmFile = new frmFileTransfer(XmppCon, iq);
                            frmFile.Show();
                        }
                    }
                }
                else
                {
                    Element query = iq.Query;

                    if (query != null)
                    {
                        if (query.GetType() == typeof(agsXMPP.protocol.iq.version.Version))
                        {
                            // its a version IQ VersionIQ
                            agsXMPP.protocol.iq.version.Version version = query as agsXMPP.protocol.iq.version.Version;
                            if (iq.Type == IqType.get)
                            {
                                // Somebody wants to know our client version, so send it back
                                iq.SwitchDirection();
                                iq.Type = IqType.result;

                                version.Name = "HipsterChat";
                                version.Ver = "0.5";
                                version.Os = Environment.OSVersion.ToString();

                                XmppCon.Send(iq);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// We received a message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        private void XmppCon_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            if (InvokeRequired)
            {
                // Windows Forms are not Thread Safe, we need to invoke this :(
                // We're not in the UI thread, so we need to call BeginInvoke				
                BeginInvoke(new OnMessageDelegate(XmppCon_OnMessage), new object[] { sender, msg });
                return;
            }

            // Dont handle GroupChat Messages here, they have their own callbacks in the
            // GroupChat Form
            if (msg.Type == MessageType.groupchat)
                return;

            if (msg.Type == MessageType.error)
            {
                //Handle errors here
                // we dont handle them in this example
                return;
            }

            // check for xData Message

            if (msg.HasTag(typeof(Data)))
            {
                Element e = msg.SelectSingleElement(typeof(Data));
                Data xdata = e as Data;
                if (xdata.Type == XDataFormType.form)
                {
                    frmXData fXData = new frmXData(xdata);
                    fXData.Text = "xData Form from " + msg.From.ToString();
                    fXData.Show();
                }
            }
            else if (msg.HasTag(typeof(agsXMPP.protocol.extensions.ibb.Data)))
            {
                // ignore IBB messages
                return;
            }
            else
            {
                if (msg.Body != null)
                {
                    if (!Util.ChatForms.ContainsKey(msg.From.Bare))
                    {
                        RosterNode rn = rosterControl.GetRosterItem(msg.From);
                        string nick = msg.From.Bare;
                        if (rn != null)
                            nick = rn.Text;

                        frmChat f = new frmChat(msg.From, XmppCon, nick);
                        f.Show();
                        f.IncomingMessage(msg);
                    }
                }
            }
        }

        private void XmppCon_OnClose(object sender)
        {
            if (InvokeRequired)
            {
                // Windows Forms are not Thread Safe, we need to invoke this :(
                // We're not in the UI thread, so we need to call BeginInvoke				
                BeginInvoke(new ObjectHandler(XmppCon_OnClose), new object[] { sender });
                return;
            }

            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = false;
            addContactToolStripMenuItem.Enabled = false;
            searchContactToolStripMenuItem.Enabled = false;
            contactListPanel.Enabled = false;
            contactsButton.Enabled = false;
            groupChatButton.Enabled = false;
            contactListPanel.BringToFront();
            cboStatus.SelectedValueChanged -= new System.EventHandler(this.cboStatus_SelectedValueChanged);

            cboStatus.Text = "offline";
            statusBar1.Text = "OffLine";
            this.Text = "HipsterChat - Offline";
            rosterControl.Clear();

        }

        private void XmppCon_OnError(object sender, Exception ex)
        {

        }
        #endregion

        private bool ClientSocket_OnValidateCertificate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void ClientSocket_OnReceive(object sender, byte[] data, int count)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new agsXMPP.net.ClientSocket.OnSocketDataHandler(ClientSocket_OnReceive), new object[] { sender, data, count });
                return;
            }
        }

        private void ClientSocket_OnSend(object sender, byte[] data, int count)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new agsXMPP.net.ClientSocket.OnSocketDataHandler(ClientSocket_OnSend), new object[] { sender, data, count });
                return;
            }
        }

        private void XmppCon_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            Console.WriteLine("OnXmppConnectionStateChanged: " + state.ToString());
        }

        private void OnBrowseIQ(object sender, IQ iq, object data)
        {
            Element s = iq.SelectSingleElement(typeof(agsXMPP.protocol.iq.browse.Service));
            if (s != null)
            {
                agsXMPP.protocol.iq.browse.Service service = s as agsXMPP.protocol.iq.browse.Service;
                string[] ns = service.GetNamespaces();
            }
        }

        #region << RequestDiscover >>
        public void RequestDiscovery()
        {
            //DiscoItemsIq discoIq = new DiscoItemsIq(IqType.get);
            ////TreeNode node = treeGC.SelectedNode;
            ////discoIq.To = new Jid(this.XmppCon.Server);
            //discoIq.To = new Jid("amessage.info");
            //this.XmppCon.IqGrabber.SendIq(discoIq, new IqCB(OnGetDiscovery), null);
        }

        /// <summary>
        /// Callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="iq"></param>
        /// <param name="data"></param>
        private void OnGetDiscovery(object sender, IQ iq, object data)
        {
            DiscoItems items = iq.Query as DiscoItems;
            if (items == null)
                return;

            DiscoItem[] rooms = items.GetDiscoItems();
            foreach (DiscoItem item in rooms)
            {
                Console.WriteLine(item.Name);
            }
        }
        #endregion

        #region << lookup chatrooms on a chatserver using service discovery >>
        /// <summary>
        /// Discover chatromms of a chat server using disco (service discovery)
        /// </summary>
        private void FindChatRooms()
        {
            TreeNode node = treeGC.SelectedNode;
            if (node == null || node.Level != 0)
                return;

            DiscoItemsIq discoIq = new DiscoItemsIq(IqType.get);
            discoIq.To = new Jid(node.Text);
            this.XmppCon.IqGrabber.SendIq(discoIq, new IqCB(OnGetChatRooms), node);
        }

        /// <summary>
        /// Callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="iq"></param>
        /// <param name="data"></param>
        private void OnGetChatRooms(object sender, IQ iq, object data)
        {
            if (InvokeRequired)
            {
                // Windows Forms are not Thread Safe, we need to invoke this :(
                // We're not in the UI thread, so we need to call BeginInvoke				
                BeginInvoke(new IqCB(OnGetChatRooms), new object[] { sender, iq, data });
                return;
            }

            TreeNode node = data as TreeNode;
            node.Nodes.Clear();

            DiscoItems items = iq.Query as DiscoItems;
            if (items == null)
                return;

            DiscoItem[] rooms = items.GetDiscoItems();
            foreach (DiscoItem item in rooms)
            {
                TreeNode n = new TreeNode(item.Name);
                n.Tag = item.Jid.ToString();
                n.ImageIndex = n.SelectedImageIndex = IMAGE_CHATROOM;
                node.Nodes.Add(n);
            }
        }

        private void FindParticipants()
        {
            TreeNode node = treeGC.SelectedNode;
            if (node == null && node.Level != 1)
                return;

            DiscoItemsIq discoIq = new DiscoItemsIq(IqType.get);
            discoIq.To = new Jid((string)node.Tag);
            this.XmppCon.IqGrabber.SendIq(discoIq, new IqCB(OnGetParticipants), node);
        }

        private void OnGetParticipants(object sender, IQ iq, object data)
        {
            if (InvokeRequired)
            {
                // Windows Forms are not Thread Safe, we need to invoke this :(
                // We're not in the UI thread, so we need to call BeginInvoke				
                BeginInvoke(new IqCB(OnGetParticipants), new object[] { sender, iq, data });
                return;
            }

            TreeNode node = data as TreeNode;
            node.Nodes.Clear();

            DiscoItems items = iq.Query as DiscoItems;
            if (items == null)
                return;

            DiscoItem[] rooms = items.GetDiscoItems();
            foreach (DiscoItem item in rooms)
            {
                TreeNode n = new TreeNode(item.Jid.Resource);
                n.Tag = item.Jid.ToString();
                n.ImageIndex = n.SelectedImageIndex = IMAGE_PARTICIPANT;
                node.Nodes.Add(n);
            }
        }
        #endregion

        private void chatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterNode node = rosterControl.SelectedItem();
            if (node != null)
            {
                if (!Util.ChatForms.ContainsKey(node.RosterItem.Jid.ToString()))
                {
                    frmChat f = new frmChat(node.RosterItem.Jid, XmppCon, node.Text);
                    f.Show();
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterNode node = rosterControl.SelectedItem();
            if (node != null)
            {
                RosterIq riq = new RosterIq();
                riq.Type = IqType.set;

                XmppCon.RosterManager.RemoveRosterItem(node.RosterItem.Jid);
            }
        }

        private void sendFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterNode node = rosterControl.SelectedItem();

            if (node != null)
            {
                if (node.Nodes.Count > 0)
                {
                    Jid jid = node.RosterItem.Jid;
                    jid.Resource = node.FirstNode.Text;
                    frmFileTransfer ft = new frmFileTransfer(XmppCon, jid);
                    ft.Show();
                }
            }
        }

        private void rosterControl_SelectionChanged(object sender, EventArgs e)
        {
            RosterNode node = rosterControl.SelectedItem();
            if (node != null)
            {
                if (node.NodeType == RosterNodeType.RosterNode)
                {
                    rosterControl.ContextMenuStrip = contextMenuStripRoster;
                    node.TreeView.DoubleClick += new System.EventHandler(this.rosterStartChat_mouseDoubleClick);
                }
                else if (node.NodeType == RosterNodeType.GroupNode)
                    rosterControl.ContextMenuStrip = null;    // Add Group context menu here
                else if (node.NodeType == RosterNodeType.RootNode)
                    rosterControl.ContextMenuStrip = null;    // Add RootNode context menu here
                else if (node.NodeType == RosterNodeType.ResourceNode)
                    rosterControl.ContextMenuStrip = null;    // Add Resource Context Menu here
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin f = new frmLogin(XmppCon);

            if (f.ShowDialog() == DialogResult.OK)
            {
                XmppCon.Open();
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmppCon.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboStatus_SelectedValueChanged(object sender, EventArgs e)
        {
            if (XmppCon != null && XmppCon.Authenticated)
            {
                if (cboStatus.Text == "online")
                {
                    XmppCon.Show = ShowType.NONE;
                }
                else if (cboStatus.Text == "offline")
                {
                    XmppCon.Close();
                }
                else
                {
                    XmppCon.Show = (ShowType)Enum.Parse(typeof(ShowType), cboStatus.Text);
                }
                XmppCon.SendMyPresence();
            }
        }

        private void joinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Join a ChatRoom
            if (XmppCon.XmppConnectionState == XmppConnectionState.Disconnected)
                return;

            TreeNode node = this.treeGC.SelectedNode;
            if (node != null && node.Level == 1)
            {
                joinChatRoom(node);
            }
        }

        private void toolStripButtonFindRooms_Click(object sender, EventArgs e)
        {
            if (XmppCon.XmppConnectionState == XmppConnectionState.Disconnected)
                return;

            FindChatRooms();
        }

        private void toolStripButtonFindPart_Click(object sender, EventArgs e)
        {
            if (XmppCon.XmppConnectionState == XmppConnectionState.Disconnected)
                return;

            FindParticipants();
        }

        #region << Disco Server >>
        private void DiscoServer()
        {
            discoManager.DiscoverItems(new Jid(XmppCon.Server), new IqCB(OnDiscoServerResult), null);
        }

        /**
         *
         */
        private void OnDiscoServerResult(object sender, IQ iq, object data)
        {
            if (iq.Type == IqType.result)
            {
                Element query = iq.Query;
                if (query != null && query.GetType() == typeof(DiscoItems))
                {
                    DiscoItems items = query as DiscoItems;
                    DiscoItem[] itms = items.GetDiscoItems();

                    foreach (DiscoItem itm in itms)
                    {
                        if (itm.Jid != null)
                            discoManager.DiscoverInformation(itm.Jid, new IqCB(OnDiscoInfoResult), itm);
                    }
                }
            }
        }

        private void OnDiscoInfoResult(object sender, IQ iq, object data)
        {
            // <iq from='proxy.cachet.myjabber.net' to='gnauck@jabber.org/Exodus' type='result' id='jcl_19'>
            //  <query xmlns='http://jabber.org/protocol/disco#info'>
            //      <identity category='proxy' name='SOCKS5 Bytestreams Service' type='bytestreams'/>
            //      <feature var='http://jabber.org/protocol/bytestreams'/>
            //      <feature var='http://jabber.org/protocol/disco#info'/>
            //  </query>
            // </iq>
            if (iq.Type == IqType.result)
            {
                if (iq.Query is DiscoInfo)
                {
                    DiscoInfo di = iq.Query as DiscoInfo;
                    if (di.HasFeature(agsXMPP.Uri.IQ_SEARCH))
                    {
                        Jid jid = iq.From;
                        if (!Util.Services.Search.Contains(jid))
                            Util.Services.Search.Add(jid);
                    }
                    else if (di.HasFeature(agsXMPP.Uri.BYTESTREAMS))
                    {
                        Jid jid = iq.From;
                        if (!Util.Services.Proxy.Contains(jid))
                            Util.Services.Proxy.Add(jid);
                    }
                }
            }
        }
        #endregion

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            frmSearch fSearch = new frmSearch(this.XmppCon);
            fSearch.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addContactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddContact f = new frmAddContact(XmppCon);
            f.ShowDialog();
        }

        private void searchContactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearch fSearch = new frmSearch(this.XmppCon);
            fSearch.Show();
        }

        private void contactsButton_Click(object sender, EventArgs e)
        {
            this.contactListPanel.BringToFront();
        }

        private void groupChatButton_Click(object sender, EventArgs e)
        {
            this.groupChatPanel.BringToFront();
        }

        private void serverRefreshButton_Click(object sender, EventArgs e)
        {
            if (XmppCon.XmppConnectionState == XmppConnectionState.Disconnected)
                return;

            FindChatRooms();
        }

        private void findParticipantsButton_Click(object sender, EventArgs e)
        {
            if (XmppCon.XmppConnectionState == XmppConnectionState.Disconnected)
                return;

            FindParticipants();
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void closeButton_mouseEnter(object sender, EventArgs e)
        {
            this.closeButton.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.close2));
        }

        private void closeButton_mouseLeave(object sender, EventArgs e)
        {
            this.closeButton.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.close));
        }

        private void rosterStartChat_mouseDoubleClick(object sender, EventArgs e)
        {
            RosterNode node = rosterControl.SelectedItem();
            if (node.NodeType == RosterNodeType.RosterNode && node != null)
            {
                if (!Util.ChatForms.ContainsKey(node.RosterItem.Jid.ToString()))
                {
                    frmChat f = new frmChat(node.RosterItem.Jid, XmppCon, node.Text);
                    f.Show();
                }
            }
        }

        private void treeGC_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = treeGC.SelectedNode;
            if (node == null)
                return;
            else if (node.Level == 0)
                FindChatRooms();
            else if (node.Level != 0)
                joinChatRoom(node);
        }

        private void joinChatRoom(TreeNode node)
        {
            // Ask for the Nickname for this GroupChat
            frmInputBox input = new frmInputBox("Nickname", "Nickname");
            if (input.ShowDialog() == DialogResult.OK)
            {
                Jid jid = new Jid((string)node.Tag);
                string nickname = input.Result;
                frmGroupChat gc = new frmGroupChat(this.XmppCon, jid, nickname);
                gc.Show();
            }
        }

    }
}