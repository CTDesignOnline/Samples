using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Brambleberry.Quickbooks.Models.Base
{
    public partial class QBSerialization
    {
        /// <summary>
        /// Condenses current object contents into Intuit API compatible XML document
        /// </summary>
        /// <returns>XMLDocument of entire object tree contents</returns>
        public virtual XmlDocument ToXml()
        {

            StringWriter Output = new StringWriter(new StringBuilder());
            string Ret = "";
            XmlDocument _RetDoc = new XmlDocument();

            // assign the request ID values
            //SetRequestIds();

            try
            {
                XmlSerializer s = new XmlSerializer(this.GetType());
                s.Serialize(Output, this);

                // cut out the header stuff since we set it later with the QB XML schema info
                Ret = Output.ToString().Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                Ret = Ret.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                Ret = Ret.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Trim();
                Ret = "<?xml version=\"1.0\" ?><?qbxml version=\"3.0\"?>" + Ret;
                _RetDoc.LoadXml(Ret);
            }
            catch (Exception)
            {
                throw;
            }

            return _RetDoc;
        }
    }
}
