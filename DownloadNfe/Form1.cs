using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

using System.Text;
using System.Windows.Forms;
using System.Xml;


namespace DownloadNfe
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void btnManifesta_Click(object sender, EventArgs e)
        {
            //
            //Ciencia da Operacao
            //
            var NFe_Rec = new NfeRecepcao.RecepcaoEventoSoapClient();
            NFe_Rec.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "PENINSULA NORTE FERTILIZANTES S A:14267717000109");

            var notas = new[]
            {
                "21161101633840001398550010000030021000030025"
                //, "Ponha a chave de 44 dígitos da NFe recebida pela sua empresa"
            }; // este array não deve passar de 20 elementos, máximo permitido por lote de manifestação

            var sbXml = new StringBuilder();
            sbXml.Append(
                          @"<?xml version=""1.0"" encoding=""UTF-8""?>
                          <envEvento xmlns=""http://www.portalfiscal.inf.br/nfe"" versao=""1.00"">
                          <idLote>1</idLote>
                          "
                        );
            foreach (var nota in notas)
            {
                sbXml.Append(@"
                    <evento xmlns=""http://www.portalfiscal.inf.br/nfe"" versao=""1.00"">
                    <infEvento Id=""ID210210" + nota + @"01"">
                    <cOrgao>91</cOrgao>
                    <tpAmb>1</tpAmb>
                    <CNPJ>14267717000109</CNPJ>
                    <chNFe>" + nota + @"</chNFe>
                    <dhEvento>" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss") + @"-03:00</dhEvento>
                    <tpEvento>210210</tpEvento>
                    <nSeqEvento>1</nSeqEvento>
                    <verEvento>1.00</verEvento>
                    <detEvento versao=""1.00"">
                    <descEvento>Ciencia da Operacao</descEvento>
                    </detEvento>
                    </infEvento>
                    </evento>
                ");
            }
            sbXml.Append("</envEvento>");

            var xml = new XmlDocument();
            xml.LoadXml(sbXml.ToString());

            var i = 0;
            foreach (var nota in notas)
            {
                var docXML = new SignedXml(xml);
                docXML.SigningKey = NFe_Rec.ClientCredentials.ClientCertificate.Certificate.PrivateKey;
                var refer = new Reference();
                refer.Uri = "#ID210210" + nota + "01";
                refer.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                refer.AddTransform(new XmlDsigC14NTransform());
                docXML.AddReference(refer);

                var ki = new KeyInfo();
                ki.AddClause(new KeyInfoX509Data(NFe_Rec.ClientCredentials.ClientCertificate.Certificate));
                docXML.KeyInfo = ki;

                docXML.ComputeSignature();
                i++;
                xml.ChildNodes[1].ChildNodes[i].AppendChild(xml.ImportNode(docXML.GetXml(), true));
            }

            var NFe_Cab = new NfeRecepcao.nfeCabecMsg();
            NFe_Cab.cUF = "21"; //RJ => De acordo com a Tabela de Código de UF do IBGE
            NFe_Cab.versaoDados = "1.00";
            var resp = NFe_Rec.nfeRecepcaoEvento(NFe_Cab, xml);

            var fileResp = "c:\\p\\testexml" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "-tempResp.xml";
            var fileReq = "c:\\p\\testexml" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "-tempRequ.xml";
            File.WriteAllText(fileReq, xml.OuterXml);
            File.WriteAllText(fileResp, resp.OuterXml);
            Process.Start(fileReq);
            Process.Start(fileResp);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            //
            //Download da NFe
            //
            var notas = new string[]
            {
                "21161101633840001398550010000030021000030025"
                //,"Ponha a chave de 44 dígitos da NFe recebida pela sua empresa"
            }; // este array não deve passar de 10 elementos, máximo permitido por lote de download da NFe

            var sbXml = new StringBuilder();
            sbXml.Append (
                            @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <downloadNFe xmlns=""http://www.portalfiscal.inf.br/nfe"" versao=""1.00"">
                            <tpAmb>1</tpAmb>
                            <xServ>DOWNLOAD NFE</xServ>
                            <CNPJ>14267717000109</CNPJ>
                            "
                         );

            foreach (var nota in notas)
            {
                sbXml.Append("<chNFe>" + nota + "</chNFe>");
            }
            sbXml.Append("</downloadNFe>");

            var xml = new XmlDocument();
            xml.LoadXml(sbXml.ToString());

            var NFe_Cab = new NfeDownloadNF.nfeCabecMsg();
            NFe_Cab.cUF = "21"; //MA => De acordo com a Tabela de Código de UF do IBGE
            NFe_Cab.versaoDados = "1.00";

            var NFe_Sc = new NfeDownloadNF.NfeDownloadNFSoapClient();
            NFe_Sc.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "PENINSULA NORTE FERTILIZANTES S A:14267717000109");
            var resp = NFe_Sc.nfeDownloadNF(NFe_Cab, xml);

            var fileResp = "c:\\p\\testexml" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "-tempResp.xml";
            var fileReq = "c:\\p\\testexml" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "-tempRequ.xml";
            File.WriteAllText(fileReq, xml.OuterXml);
            File.WriteAllText(fileResp, resp.OuterXml);
            System.Diagnostics.Process.Start(fileReq);
            System.Diagnostics.Process.Start(fileResp);
        }
    }
}
