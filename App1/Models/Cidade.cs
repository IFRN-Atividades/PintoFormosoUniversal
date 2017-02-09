using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Models
{
    public class Cidade
    {
        private int Id { get; set; }
        public string Nome { get; set; }
        public int IdEstado { get; set; }

        public Estado estado { get; set; }

        public Cidade comEstado(Estado e)
        {
            this.estado = e;
            return this;
        }

        public override string ToString()
        {
            return $"{this.Nome} - {this.estado.Sigla}";
        }
    }
}
