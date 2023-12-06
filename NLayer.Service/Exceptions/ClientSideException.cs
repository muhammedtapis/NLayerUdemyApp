using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Exceptions
{
    public class ClientSideException : Exception
    {
        //constructorda burdaki exception mesajını base sınıfta exceptiona gönderiyoruz
        public ClientSideException(string exceptionMessage) : base(exceptionMessage)
        {
        }
    }
}