using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LigarMotorista
{
    internal class LogModel
    {
        public LogModel()
        {
        }

        public DateTime Data { get; set; }
        public DateTime Hora { get; set; }
        public int idLog { get; set; }
        public string NomeMotorista { get; set; }
        public string Obs { get; set; }
        public string QuemLigou { get; set; }
    }
}