using Soneta.Business;
using Soneta.Business.App;
using Soneta.Business.UI;
using Soneta.Handel;
using System;
using System.Windows.Forms;

namespace Soneta_Sample_RS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Soneta.Start.Loader loader = new Soneta.Start.Loader();
            //loader.WithExtensions = true;
            loader.WithExtra = true;
            loader.Load();

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Login login = CreateLogin();
            if (login != null)
                using (Session session = login.CreateSession(false, false))
                {
                    using (var t = session.Logout(true))
                    {
                        var context = Context.Empty.Clone(session);
                        var storage = context.Login != null ? context.Login.StorageProvider : null;
                        IReportService rs;
                        
                        HandelModule hm = HandelModule.GetInstance(session);
                        DokumentHandlowy dok = hm.DokHandlowe.NumerWgNumeruDokumentu["FV/000004/19"];
                        context.Set(dok);
                        context.Session.GetService(out rs);

                        var reportResult = new ReportResult //rezultat służący do uruchamiania raportów
                        {

                            Caption = "Generowanie faktury", //nazwa zadania
                            Context = context, //kontekst raportu
                            Preview = false, //nie pokazuj okna z podglądem raportu
                            //DataType = dok.GetType(), //typ dla którego będzie generowany wydruk
                            Format = ReportResultFormat.PDF, //format zapisu
                            TemplateFileName = "XtraReports/Wzorce użytkownika/dokument_sprzedazy.repx",
                            TemplateFileSource = AspxSource.Local, //wskazanie na aspx wydruku
                        };
                        var stream = rs.GenerateReport(reportResult);
                        t.Commit();
                    }
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
