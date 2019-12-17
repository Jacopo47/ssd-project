using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ssdProject.Model
{
    class GAPClass
    {
        public int n; // numero clienti
        public int m; // numero magazzini
        public double[,] c; // costi assegnamento
        public int[] req; // richieste clienti
        public int[] cap; // capacità magazzini

        public int[] sol, solbest; // per ogni cliente, il suo magazzino
        public double zub, zlb;

        const double EPS = 0.0001;
        System.Random rnd = new Random(550);

        public GAPClass()
        {
            zub = double.MaxValue;
            zlb = double.MinValue;
        }
    }
}