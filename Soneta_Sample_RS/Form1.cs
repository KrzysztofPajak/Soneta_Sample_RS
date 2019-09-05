using Soneta.Business;
using Soneta.Business.App;
using Soneta.Business.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Soneta_Sample_RS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Soneta.Start.Loader loader = new Soneta.Start.Loader();
            loader.WithExtensions = false;
            loader.Load();

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Login login = CreateLogin();
            using (Session session = login.CreateSession(false, false))
            {
                var context = Context.Empty.Clone(session);
                var storage = context.Login != null ? context.Login.StorageProvider : null;
                IReportService rs;
                context.Session.GetService(out rs);
            }
        }

        public Login CreateLogin()
        {
            Login login;
            MsSqlDatabase msdb = new MsSqlDatabase("enova", @"DESKTOP-LDPQC0R\SQL2017", "enova", "sa", "sa", false);
            msdb.Active = true;
            try
            {
                login = msdb.Login(false, "Administrator", "123456");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return login;
        }
    }
}
