using System;

using agsXMPP;
using agsXMPP.protocol.component;

namespace Component
{
	/// <summary>
	/// Component example
	/// </summary>
	class Component
	{
		private static XmppComponentConnection comp = null;

        /*
         * change the following constats according to your correct server configuration
         * this our our testing values.
         */
        private const int       PORT        = 5275;
        private const string    SECRET      = "12345";        
        private const string    HOST        = "vm-2k";
        private const string    JID         = "weather.vm-2k";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            // create a new component connection to the given serverdomain, port and password
            // a password is needed for the most servers for security reasons, if your server runs behind
            // a firewall or a closed company network then you could allow component connections without password.
            comp = new XmppComponentConnection(HOST, PORT, SECRET);

            // The Jid which this component will use
			comp.ComponentJid = new Jid(JID);

            // Setup event handlers
			comp.OnLogin	+= new ObjectHandler(comp_OnLogin);
			comp.OnClose	+= new ObjectHandler(comp_OnClose);
			comp.OnRoute	+= new agsXMPP.XmppComponentConnection.RouteHandler(comp_OnRoute);			
			comp.OnReadXml	+= new XmlHandler(comp_OnReadXml);
			comp.OnWriteXml	+= new XmlHandler(comp_OnWriteXml);

            comp.OnMessage  += new MessageHandler(comp_OnMessage);
            comp.OnIq       += new IqHandler(comp_OnIq);
            comp.OnPresence += new PresenceHandler(comp_OnPresence);

            comp.OnAuthError += new XmppElementHandler(comp_OnAuthError);

			comp.Open();

			Console.ReadLine();
			comp.Close();         
			
		}

        static void comp_OnPresence(object sender, Presence pres)
        {
            Console.WriteLine("OnPresence\r\n" + pres.ToString());
        }

        static void comp_OnIq(object sender, IQ iq)
        {
            Console.WriteLine("OnIq\r\n" + iq.ToString());
        }

        static void comp_OnMessage(object sender, agsXMPP.protocol.component.Message msg)
        {
            Console.WriteLine("OnMessage\r\n" + msg.ToString());
        }

        static void comp_OnAuthError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            Console.WriteLine("Authentication error ==> wrong Password\r\n");
        }

		private static void comp_OnClose(object sender)
		{
			Console.WriteLine("OnClose\r\n");
		}

		private static void comp_OnLogin(object sender)
		{
			Console.WriteLine("OnLogin\r\n");      
   
            RequestServerVersion();
            DiscoverClient();

        }

        #region << Example of using the IqGrabber in components >>


        private static void DiscoverClient()
        {
            // change the Jid when running the example to a online user, otherwise this will not work
            Jid jid = new Jid("admin@vm-2k/ALEX-LAPTOP");
            
            IQ iq = new IQ(agsXMPP.protocol.client.IqType.get);           
            iq.To = jid;
            iq.From = new Jid(JID);
            iq.GenerateId();

            
            iq.AddChild(new agsXMPP.protocol.iq.disco.DiscoInfo());
            
            /*
             * we pass an additional object (jid) here with the IqGrabber
             * Passing the jid makes no sense because you can also read it from the iq response, but 
             * this is only an example to show that you can pass any object you need in your business logic
             */
            comp.IqGrabber.SendIq(iq, new IqCB(OnDiscoResult), jid);
        }

        private static void OnDiscoResult(object sender, agsXMPP.protocol.client.IQ iq, object data)
        {
            Console.WriteLine("Disco result:");
            Console.WriteLine("passed object: " + ((Jid) data).Bare + "\r\n");            
        }

        private static void RequestServerVersion()
        {
            IQ iq = new IQ(agsXMPP.protocol.client.IqType.get);
            // change the Jid when running the example to a online user, otherwise this will not work
            iq.To = new Jid("admin@vm-2k/ALEX-LAPTOP");
            iq.From = new Jid(JID);
            iq.GenerateId();

            iq.AddChild(new agsXMPP.protocol.iq.version.Version());
            //iq.AddChild(new agsXMPP.protocol.iq.disco.DiscoInfo());
            comp.IqGrabber.SendIq(iq, new IqCB(OnVersionResult));
        }

        /// <summary>
        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="iq"></param>
        /// <param name="data"></param>
        private static void OnVersionResult(object sender, agsXMPP.protocol.client.IQ iq, object data)
        {
            if (iq.Query is agsXMPP.protocol.iq.version.Version)
            {
                agsXMPP.protocol.iq.version.Version ver = iq.Query as agsXMPP.protocol.iq.version.Version;
                Console.WriteLine("Version result:");
                Console.WriteLine("Name: " + ver.Name);
                Console.WriteLine("Version: " + ver.Ver);
                Console.WriteLine("Os: " + ver.Os + "\r\n");
            }
        }
        #endregion

        private static void comp_OnRoute(object sender, agsXMPP.protocol.component.Route r)
		{
            Console.WriteLine("OnRoute: " + r.ToString() + "\r\n");
		}

		private static void comp_OnReadXml(object sender, string xml)
		{
			Console.WriteLine("OnReadXml: " + xml + "\r\n");
		}

		private static void comp_OnWriteXml(object sender, string xml)
		{
			Console.WriteLine("OnWriteXml: " + xml + "\r\n");
		}
	}
}