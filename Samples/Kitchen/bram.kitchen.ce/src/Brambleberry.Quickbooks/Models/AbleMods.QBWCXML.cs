using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Brambleberry.Quickbooks.Models
{

    public class QBWCXML
    {
        public string AppName;
        public string AppID;
        public string AppURL;
        public string AppDescription;
        public string AppSupport;
        public string UserName;
        public string OwnerID;
        public string FileID;
        public string QBType;
        public virtual string ToXml()
        {

            StringWriter Output = new StringWriter(new StringBuilder());
            string Ret = "";

            try
            {
                XmlSerializer s = new XmlSerializer(this.GetType());
                s.Serialize(Output, this);

                // cut out the header stuff since we set it later with the QB XML schema info

                Ret = Output.ToString().Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                Ret = Ret.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                Ret = Ret.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Trim();

                // clean out any extra tags not compatible with QBSDK
                Ret = Ret.Replace("<OrderItems>", "");
                Ret = Ret.Replace("</OrderItems>", "");

                // strip empty ref tags - QBSDK doesn't like them
                Ret = Ret.Replace("<Quantity>0</Quantity>", "");

                // add the std XML header info required by QBSDK
                Ret = "<?xml version=\"1.0\" ?>" + Ret;
            }
            catch (Exception)
            {
                throw;
            }

            return Ret;
        }

    }
}