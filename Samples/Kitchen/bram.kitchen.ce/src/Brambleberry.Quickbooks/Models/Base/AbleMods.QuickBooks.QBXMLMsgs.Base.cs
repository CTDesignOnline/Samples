using System;
using System.Xml.Serialization;

namespace Brambleberry.Quickbooks.Models.Base
{

    [XmlRoot(ElementName = "QBXMLMsgsRq")]
    public class BaseXMLMsgs : QBSerialization
    {
        [XmlAttribute(AttributeName = "onError")]
        public string onError = "continueOnError";
    }

    /// <summary>
    /// base QUERY request
    /// </summary>
    public class BaseQryRq : BaseRequest
    {
        [XmlIgnore]
        public string ObjectName
        {
            get
            {
                string _RetVal = this.GetType().Name;
                return _RetVal.Substring(0, _RetVal.IndexOf("QryRq"));
            }
        }
        /// <summary>
        /// REQUEST or RESPONSE
        /// </summary>
        [XmlIgnore]
        public string ObjectCmd { get { return "Request"; } }
        /// <summary>
        /// QUERY or ADD
        /// </summary>
        [XmlIgnore]
        public string ObjectMode { get { return "Query"; } }
    }

    /// <summary>
    /// base ADD request
    /// </summary>
    public class BaseAddRq : BaseRequest
    {
        [XmlIgnore]
        public string ObjectName
        {
            get
            {
                string _RetVal = this.GetType().Name;
                return _RetVal.Substring(0, _RetVal.IndexOf("AddRq"));
            }
        }
        /// <summary>
        /// REQUEST or RESPONSE
        /// </summary>
        [XmlIgnore]
        public string ObjectCmd { get { return "Request"; } }
        /// <summary>
        /// QUERY or ADD
        /// </summary>
        [XmlIgnore]
        public string ObjectMode { get { return "Add"; } }
    }

    /// <summary>
    /// base ADD response
    /// </summary>
    public class BaseAddRs : BaseResponse
    {
        [XmlIgnore]
        public string ObjectName
        {
            get
            {
                string _RetVal = this.GetType().Name;
                return _RetVal.Substring(0, _RetVal.IndexOf("AddRs"));
            }
        }
        /// <summary>
        /// REQUEST or RESPONSE
        /// </summary>
        [XmlIgnore]
        public string ObjectCmd { get { return "Response"; } }
        /// <summary>
        /// QUERY or ADD
        /// </summary>
        [XmlIgnore]
        public string ObjectMode { get { return "Add"; } }

        [XmlIgnore]
        public QBResponseStatus StatusMsg
        {
            get
            {
                QBResponseStatus _RetVal = new QBResponseStatus();
                _RetVal.ObjectName = this.ObjectName;
                _RetVal.ObjectCmd = this.ObjectCmd;
                _RetVal.ObjectMode = this.ObjectMode;
                _RetVal.StatusMessage = this.statusMessage;
                _RetVal.StatusCode = this.statusCode;
                _RetVal.RequestId = this.requestID;
                return _RetVal;
            }
        }
    }

    /// <summary>
    /// base QUERY response
    /// </summary>
    public class BaseQryRs : BaseResponse
    {
        [XmlIgnore]
        public string ObjectName
        {
            get
            {
                string _RetVal = this.GetType().Name;
                return _RetVal.Substring(0, _RetVal.IndexOf("QueryRs"));
            }
        }
        /// <summary>
        /// REQUEST or RESPONSE
        /// </summary>
        [XmlIgnore]
        public string ObjectCmd { get { return "Response"; } }
        /// <summary>
        /// QUERY or ADD
        /// </summary>
        [XmlIgnore]
        public string ObjectMode { get { return "Query"; } }

        [XmlIgnore]
        public QBResponseStatus StatusMsg
        {
            get
            {
                QBResponseStatus _RetVal = new QBResponseStatus();
                _RetVal.ObjectName = this.ObjectName;
                _RetVal.ObjectCmd = this.ObjectCmd;
                _RetVal.ObjectMode = this.ObjectMode;
                _RetVal.StatusMessage = this.statusMessage;
                _RetVal.StatusCode = this.statusCode;
                _RetVal.RequestId = this.requestID;
                return _RetVal;
            }
        }

    }

    /// <summary>
    /// base MOD response
    /// </summary>
    public class BaseModRs : BaseResponse
    {
        [XmlIgnore]
        public string ObjectName
        {
            get
            {
                string _RetVal = this.GetType().Name;
                return _RetVal.Substring(0, _RetVal.IndexOf("ModRs"));
            }
        }
        /// <summary>
        /// REQUEST or RESPONSE
        /// </summary>
        [XmlIgnore]
        public string ObjectCmd { get { return "Response"; } }
        
        /// <summary>
        /// QUERY or ADD or MOD
        /// </summary>
        [XmlIgnore]
        public string ObjectMode { get { return "Mod"; } }

        [XmlIgnore]
        public QBResponseStatus StatusMsg
        {
            get
            {
                QBResponseStatus _RetVal = new QBResponseStatus();
                _RetVal.ObjectName = this.ObjectName;
                _RetVal.ObjectCmd = this.ObjectCmd;
                _RetVal.ObjectMode = this.ObjectMode;
                _RetVal.StatusMessage = this.statusMessage;
                _RetVal.StatusCode = this.statusCode;
                _RetVal.RequestId = this.requestID;
                return _RetVal;
            }
        }

    }


    /// <summary>
    /// base elements for REQUEST to QuickBooks
    /// </summary>
    public class BaseRequest
    {
        [XmlAttribute(AttributeName = "requestID")]
        public int requestID;
    }

    /// <summary>
    /// base class elements for a RESPONSE from QuickBooks
    /// </summary>
    public class BaseResponse
    {
        [XmlAttribute(AttributeName = "requestID")]
        public int requestID;

        [XmlAttribute(AttributeName = "statusCode")]
        public string statusCode;

        [XmlAttribute(AttributeName = "statusSeverity")]
        public string statusSeverity;

        [XmlAttribute(AttributeName = "statusMessage")]
        public string statusMessage;


        [XmlIgnore]
        public bool HasResponse
        { get { if (this.statusCode != "" & this.statusCode != null) return true; return false; } }


    }

    public class BaseRet
    {
        public string ListID;
        public DateTime TimeCreated;
        public DateTime TimeModified;
        public string EditSequence;
    }


}
