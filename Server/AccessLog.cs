using System;

namespace Server
{
    public struct AccessLog
    {
        public int Id { get; set; }
        public string IpOrigin { get; set; }
        public string Url { get; set; }
        public int HttpResult { get; set; }
        public DateTime Date { get; set; }
        public int SizeRequest { get; set; }
        public string User { get; set; }
        public string IpDestination { get; set; }
    }
}
