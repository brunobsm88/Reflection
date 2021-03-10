using ByteBank.Portal.Filtros;
using ByteBank.Portal.Infraestrutura;
using ByteBank.Service;
using ByteBank.Service.Cambio;
using ByteBank.Service.Cartao;
using System;

namespace ByteBank.Portal.Controller
{
    public class CambioController : ControllerBase
    {
        private ICambioService _cambioService;
        private ICartaoService _cartaoService;
        public CambioController(ICambioService cambioService, ICartaoService cartaoService)
        {
            _cambioService = cambioService;
            _cartaoService = cartaoService;
        }
        [ApenasHorarioComercialFiltro]
        public string MXN()
        {
            var valorFinal = _cambioService.Calcular("MXN", "BRL", 1);
            return View(new
            {
                Valor = valorFinal
            });
        }
        [ApenasHorarioComercialFiltro]
        public string USD()
        {
            var valorFinal = _cambioService.Calcular("USD", "BRL", 1);
            return View(new
            {
                Valor = valorFinal
            });
        }

        [ApenasHorarioComercialFiltro]
        public string Calculo(string moedaDestino) =>
            Calculo("BRL", moedaDestino, 1);
        [ApenasHorarioComercialFiltro]
        public string Calculo(string moedaDestino, decimal valor) =>
            Calculo("BRL", moedaDestino, valor);
        [ApenasHorarioComercialFiltro]
        public string Calculo(string moedaOrigem, string moedaDestino, decimal valor)
        {
            var valorFinal = _cambioService.Calcular(moedaOrigem, moedaDestino, valor);
            var cartaoPromocao = _cartaoService.ObterCartaoDeCreditoDeDestaque();
            var modelo = new
            {
                MoedaDestino = moedaDestino,
                MoedaOrigem = moedaOrigem,
                ValorDestino = valorFinal,
                ValorOrigem = valor,
                CartaoPromocao = cartaoPromocao
            };
            return View(modelo);

            //var textoResultado =
            //    textoPagina
            //        .Replace("VALOR_MOEDA_ORIGEM", valor.ToString())
            //        .Replace("VALOR_MOEDA_DESTINO", valorFinal.ToString())
            //        .Replace("MOEDA_ORIGEM", moedaOrigem)
            //        .Replace("MOEDA_DESTINO", moedaDestino);

            //return textoResultado;
        }
    }
}
