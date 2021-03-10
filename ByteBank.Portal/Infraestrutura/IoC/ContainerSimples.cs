using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura.IoC
{
    public class ContainerSimples : IContainer
    {
        private readonly Dictionary<Type, Type> _mapaDeTipos = new Dictionary<Type, Type>();
        // Regristar ( typeof(ICambioService), typeof(CambioServiceTeste));
        // Recuperar ( typeof(ICambioService))
        // deve retornar para nós uma instancia do tipo CambioServiceTeste
        public void Registrar(Type tipoOrigem, Type tipoDestino)
        {
            if (_mapaDeTipos.ContainsKey(tipoOrigem))
                throw new InvalidOperationException("Tipo já mapeado");

            VerificarHierarquiaOuLancarExcecao(tipoOrigem, tipoDestino);

            _mapaDeTipos.Add(tipoOrigem, tipoDestino);
        }
        public void Registrar<TOrigem, TDestino>() where TDestino : TOrigem
        {
            if (_mapaDeTipos.ContainsKey(typeof(TOrigem)))
                throw new InvalidOperationException("Tipo já mapeado");

            _mapaDeTipos.Add(typeof(TOrigem), typeof(TDestino));
        }
        private void VerificarHierarquiaOuLancarExcecao(Type tipoOrigem, Type tipoDestino)
        {
            //Verificar se o tipoDestino herda ou implementa tipoOrigem
            if (tipoOrigem.IsInterface)
            {
                var tipoDestinoImplementaInterface =
                    tipoDestino.GetInterfaces()
                    .Any(t => t == tipoOrigem);

                if (!tipoDestinoImplementaInterface)
                    throw new InvalidOperationException("O tipo destino não implementa a interface");
            }
            else
            {
                var tipoDestinoHerdaTipoOrigem = tipoDestino.IsSubclassOf(tipoOrigem);
                if (!tipoDestinoHerdaTipoOrigem)
                    throw new InvalidOperationException("O tipo destino não herda o tipo de origem");
            }
        }
        public object Recuperar(Type tipoOrigem)
        {
            var tipoOrigemFoiMapeado = _mapaDeTipos.ContainsKey(tipoOrigem);

            if (tipoOrigemFoiMapeado)
            {
                var tipoDestino = _mapaDeTipos[tipoOrigem];
                return Recuperar(tipoDestino);
            }

            var construtores = tipoOrigem.GetConstructors();
            var construtorSemParametros = construtores.FirstOrDefault(c=> c.GetParameters().Any() == false);

            if(construtorSemParametros != null)
            {
                var instanciaDeConstrutorSemParametros = construtorSemParametros.Invoke(new object[0]);
                return instanciaDeConstrutorSemParametros;
            }

            var construtor = construtores[0];
            var parametrosDoConstrutor = construtor.GetParameters();
            var valoresDeParamentros = new object[parametrosDoConstrutor.Count()];

            for (int i = 0; i < parametrosDoConstrutor.Count(); i++)
            {
                var parametro = parametrosDoConstrutor[i];
                var tipoParamentro = parametro.ParameterType;

                valoresDeParamentros[i] = Recuperar(tipoParamentro);
            }

            var instancia = construtor.Invoke(valoresDeParamentros);
            return instancia;
        }

    }
}
