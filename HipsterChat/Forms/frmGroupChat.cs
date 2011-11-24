using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using agsXMPP;
using agsXMPP.Collections;
using agsXMPP.protocol;
using agsXMPP.protocol.client;
using agsXMPP.protocol.x.muc;

namespace HipsterClient
{
    public partial class frmGroupChat : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
        int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        #region << Constructors >>        
        public frmGroupChat(XmppClientConnection xmppCon, Jid roomJid, string Nickname)
        {
            InitializeComponent();
            this.rtfSend.Select();
            m_RoomJid = roomJid;
            m_XmppCon = xmppCon;
            m_Nickname = Nickname;
            this.groupChatServerLabel.Text = "Room Name: " + m_RoomJid.User;

            Util.GroupChatForms.Add(roomJid.Bare.ToLower(), this);
            
            // Setup new Message Callback
            m_XmppCon.MessageGrabber.Add(roomJid, new BareJidComparer(), new MessageCB(MessageCallback), null);
            
            // Setup new Presence Callback
            m_XmppCon.PresenceGrabber.Add(roomJid, new BareJidComparer(), new PresenceCB(PresenceCallback), null);
        }
        #endregion

        private Jid                     m_RoomJid;
        private XmppClientConnection    m_XmppCon;
        private string                  m_Nickname;

        private void frmGroupChat_Load(object sender, EventArgs e)
        {
            if (m_RoomJid != null)
            {
                Presence pres = new Presence();

                Jid to = new Jid(m_RoomJid.ToString());
                to.Resource = m_Nickname;
                pres.To = to;
                m_XmppCon.Send(pres);
            }
        }

        private void frmGroupChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_RoomJid != null)
            {
                Presence pres = new Presence();
                pres.To = m_RoomJid;
                pres.Type = PresenceType.unavailable;
                m_XmppCon.Send(pres);
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        private void MessageCallback(object sender, agsXMPP.protocol.client.Message msg, object data)
        {
            if (InvokeRequired)
            {				
                BeginInvoke(new MessageCB(MessageCallback), new object[] { sender, msg, data });
                return;
            }
            
            if (msg.Type == MessageType.groupchat)
                IncomingMessage(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pres"></param>
        /// <param name="data"></param>
        private void PresenceCallback(object sender, agsXMPP.protocol.client.Presence pres, object data)
        {
            if (InvokeRequired)
            {				
                BeginInvoke(new PresenceCB(PresenceCallback), new object[] { sender, pres, data });
                return;
            }

            ListViewItem lvi = FindListViewItem(pres.From);
            if (lvi != null)
            {
                if (pres.Type == PresenceType.unavailable)
                {
                    lvi.Remove();
                }
                else
                {
                    int imageIdx = Util.GetRosterImageIndex(pres);
                    lvi.ImageIndex = imageIdx;
                    lvi.SubItems[1].Text = (pres.Status == null ? "" : pres.Status);
                    User u = pres.SelectSingleElement(typeof(User)) as User;
                    if (u != null)
                    {
                        lvi.SubItems[2].Text = u.Item.Affiliation.ToString();
                        lvi.SubItems[3].Text = u.Item.Role.ToString();
                    }
                }
            }
            else
            {
                int imageIdx = Util.GetRosterImageIndex(pres);
                
                ListViewItem lv = new ListViewItem(pres.From.Resource);               

                lv.Tag = pres.From.ToString();
                lv.SubItems.Add(pres.Status == null ? "" : pres.Status);
                User u = pres.SelectSingleElement(typeof(User)) as User;
                if (u != null)
                {
                    lv.SubItems.Add(u.Item.Affiliation.ToString());
                    lv.SubItems.Add(u.Item.Role.ToString());
                }
                lv.ImageIndex = imageIdx;
                lvwRoster.Items.Add(lv);
            }
        }

        private ListViewItem FindListViewItem(Jid jid)
        {
            foreach (ListViewItem lvi in lvwRoster.Items)
            {
                if (jid.ToString().ToLower() == lvi.Tag.ToString().ToLower())
                    return lvi;
            }
            return null;
        }

        private void IncomingMessage(agsXMPP.protocol.client.Message msg)
        {
            if (msg.Type == MessageType.error)
            {
                MessageBox.Show("Error: " + msg.Body, 
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                return;
            }

            if (msg.Subject != null)
            {
                txtSubject.Text = msg.Subject;

                rtfChat.SelectionColor = Color.DarkGreen;
                rtfChat.AppendText(msg.From.Resource + " changed subject: ");
                rtfChat.SelectionColor = Color.DarkTurquoise;                
                rtfChat.AppendText(msg.Subject);
                rtfChat.AppendText("\r\n");
            }
            else
            {
                if (msg.Body == null)
                    return;

                rtfChat.SelectionColor = Color.Red;
                rtfChat.AppendText(msg.From.Resource + " said: ");
                rtfChat.SelectionColor = Color.DarkTurquoise;
                rtfChat.AppendText(msg.Body);
                rtfChat.AppendText("\r\n");
            }
            rtfChat.ScrollToCaret();
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            if (rtfSend.Text.Length > 0)
            {
                sendText();
            }
        }

        /// <summary>
        /// Changing the subject in a chatroom
        /// in MUC rooms this could return an error when you are a normal user and not allowed
        /// to change the subject.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdChangeSubject_Click(object sender, EventArgs e)
        {
            agsXMPP.protocol.client.Message msg = new agsXMPP.protocol.client.Message();

            msg.Type = MessageType.groupchat;
            msg.To = m_RoomJid;
            msg.Subject = txtSubject.Text;

            m_XmppCon.Send(msg);
        }

        private void rtfSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers != Keys.Shift)
            {
                sendText();
            }
            else if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Shift)
            {
                rtfSend.AppendText("\r\n");
            }
            else
            {
                return;
            }
            e.SuppressKeyPress = true;
        }

        private void sendText()
        {
            agsXMPP.protocol.client.Message msg = new agsXMPP.protocol.client.Message();

            msg.Type = MessageType.groupchat;
            msg.To = m_RoomJid;
            msg.Body = rtfSend.Text;

            m_XmppCon.Send(msg);

            rtfSend.Text = "";
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeButton_MouseEnter(object sender, EventArgs e)
        {
            this.closeButton.Image = ((System.Drawing.Image)(Properties.Resources.close2));
        }

        private void closeButton_MouseLeave(object sender, EventArgs e)
        {
            this.closeButton.Image = ((System.Drawing.Image)(Properties.Resources.close));
        }

        private void frmGroupChat_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}