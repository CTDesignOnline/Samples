namespace Brambleberry.Quickbooks
{
    public class QBResponseStatus
    {

        public string ObjectName;
        /// <summary>
        /// QUERY or ADD
        /// </summary>
        public string ObjectMode;
        /// <summary>
        /// REQUEST or RESPONSE
        /// </summary>
        public string ObjectCmd;
        public int RequestId;
        public string StatusCode;
        public string StatusMessage;
    }

}
