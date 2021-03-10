using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Service.Cartao
{
    public class CartaoServiceTeste : ICartaoService
    {
        public string ObterCartaoDeCreditoDeDestaque()
         => "ByteBank Gold Platinum Black Extra Special"; 

        public string ObterCartaoDeDebitoDeDestaque()
           => "ByteBank Estudante sem taxas de manutenção";

    }
}
