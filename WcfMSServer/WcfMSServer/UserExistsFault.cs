using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfMSServer
{
    [DataContract]
    public class UserExistsFault
    {
        [DataMember]
        public string Message { get; set; }
    }
}
