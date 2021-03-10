using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura.Filter
{
    public abstract class FiltroAttribute: Attribute
    {
        public abstract bool PodeContinuar();
    }
}
