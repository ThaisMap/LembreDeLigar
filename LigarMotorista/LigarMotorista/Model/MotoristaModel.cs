using Ganss.Excel;
using System;

namespace LigarMotorista
{
    internal class Motorista
    {
        public Motorista()
        { }

        public Motorista(string id, string nome, string obs)
        {
            NomeMotorista = nome;
            Observacao = obs;
            Id = id;
            UltimaLigacao = DateTime.Today;
            Atrasado = false;
        }

        public string Acao { get; set; }
        public bool Atrasado { get; set; }
        public string Cliente { get; set; }
        [Column("DATA")]
        public DateTime DataManifesto { get; set; }
        public string Fornecedor { get; set; }
        public string Id { get; set; }
        public int Intervalo { get; set; }
        public string NF { get; set; }
        [Column("MOTORISTA")]
        public string NomeMotorista { get; set; }
        [Column("OBSERVAÇÃO")]
        public string Observacao { get; set; }
        public string QuemLigou { get; set; }
        public DateTime UltimaLigacao { get; set; }

        public void CalculaProxima()
        {
            DateTime Proxima = DateTime.Today;

            if (Intervalo > 0 && UltimaLigacao > DateTime.Today)
                Proxima = UltimaLigacao.AddMinutes(Intervalo);

            if (Proxima != DateTime.Today && Proxima <= DateTime.Now)
                Atrasado = true;
        }
    }
}