using System;
using System.Collections.Generic;

namespace ssdProject.Model
{
    //REQ lo riempio con i dati di forecast devono essere 52 quindi io ne ho un top poi il resto lo prevedo
    public class GAPClass
    {
        private const double EPS = 0.0001;
        public double[,] c; // costi assegnamento
        public int[] cap; // capacità magazzini
        public int m; // numero magazzini
        public int n; // numero clienti
        public int[] req; // richieste clienti
        private readonly Random rnd = new Random(550);

        public int[] sol, solbest; // per ogni cliente, il suo magazzino
        public double zub, zlb;

        public GAPClass()
        {
            zub = double.MaxValue;
            zlb = double.MinValue;
        }
        
        
        // costruzione, ognuno al suo deposito più vicino
        public double simpleContruct()
        {  
            int i,ii,j;
            int[] capleft = new int[cap.Length],ind = new int[m];
            double[] dist = new double[m];
            Array.Copy(cap,capleft,cap.Length);

            zub = 0;
            for(j=0;j<n;j++)
            {  for(i=0;i<m;i++)
                {  dist[i]= c[i,j];
                    ind[i] = i;
                }
                Array.Sort(dist,ind);
                ii=0;
                while(ii<m)
                {  i=ind[ii];
                    if(capleft[i]>=req[j])
                    {  sol[j]=i;
                        capleft[i] -= req[j];
                        zub += c[i,j];

                        break;
                    }
                    ii++;
                }
                if(ii==m)
                    Console.WriteLine("[SimpleConstruct] Ahi ahi. ii="+ii);
            }
            return zub;
        }
        

        // tolgo un cliente da un magazzino e lo metto in un altro
        public OptimizationResult opt10(double[,] c)
        {
            var log = new List<string>();
            double z = 0;
            int i, isol, j;
            var capres = new int[cap.Length];

            Array.Copy(cap, capres, cap.Length);
            for (j = 0; j < n; j++)
            {
                capres[sol[j]] -= req[j];
                z += c[sol[j], j];
            }

            l0:
            for (j = 0; j < n; j++)
            {
                isol = sol[j];
                for (i = 0; i < m; i++)
                {
                    if (i == isol) continue;
                    if (c[i, j] < c[isol, j] && capres[i] >= req[j])
                    {
                        sol[j] = i;
                        capres[i] -= req[j];
                        capres[isol] += req[j];
                        z -= c[isol, j] - c[i, j];
                        if (z < zub)
                        {
                            zub = z;
                            log.Add("[1-0 opt] new zub " + zub);
                        }

                        goto l0;
                    }
                }
            }

            double zcheck = 0;
            for (j = 0; j < n; j++)
                zcheck += c[sol[j], j];
            if (Math.Abs(zcheck - z) > EPS)
                log.Add("[1.0opt] Solution is different of: "+ Math.Abs(z - zcheck) + " should not be that!");
            
            return new OptimizationResult(log, z);
        }

        // Tabu search 30 - 1000
        public OptimizationResult TabuSearch(int Ttenure, int maxiter)
        {
            var log = new List<string>();
            int i, isol, j, imax, jmax, iterazione;
            double z, DeltaMax;
            var capacitaResidua = new int[cap.Length];
            var TL = new int[m, n];
            /*
            1.	Generate an initial feasible solution S, 
	            set S* = S and initialize TL=nil.
            2.	Find S' \in N(S), such that 
	            z(S')=min {z(S^), foreach S^\in N(S), S^\notin TL}.
            3.	S=S', TL=TL \in {S}, if (z(S*) > z(S)) set S* = S.
            4.	If not(end condition) go to step 2.
            */

            Array.Copy(cap, capacitaResidua, cap.Length);
            for (j = 0; j < n; j++)
                capacitaResidua[sol[j]] -= req[j];

            z = zub;
            iterazione = 0;
            for (i = 0; i < m; i++)
            for (j = 0; j < n; j++)
                TL[i, j] = int.MinValue;

            log.Add("Starting tabu search");
            start:
            DeltaMax = imax = jmax = int.MinValue;
            iterazione++;

            for (j = 0; j < n; j++)
            {
                isol = sol[j];
                for (i = 0; i < m; i++)
                {
                    if (i == isol) continue;
                    if (c[isol, j] - c[i, j] > DeltaMax && capacitaResidua[i] >= req[j] && TL[i, j] + Ttenure < iterazione)
                    {
                        imax = i;
                        jmax = j;
                        DeltaMax = c[isol, j] - c[i, j];
                    }
                }
            }

            isol = sol[jmax];
            sol[jmax] = imax;
            capacitaResidua[imax] -= req[jmax];
            capacitaResidua[isol] += req[jmax];
            z -= DeltaMax;
            if (z < zub)
                zub = z;
            TL[imax, jmax] = iterazione;
            if (iterazione % 100 == 0)
                log.Add("Tabu Search z value -> " + z + " / iteration: " + iterazione + " dMax: " + DeltaMax);

            if (iterazione < maxiter) // end condition
                goto start;
            log.Add("Tabu search: fine");

            double zcheck = 0;
            for (j = 0; j < n; j++)
                zcheck += c[sol[j], j];
            if (Math.Abs(z - zcheck) > EPS)
                log.Add("Solution is different of: "+ Math.Abs(z - zcheck) + " should not be that!");

            return new OptimizationResult(log, zub);
        }

        // ILS, su 1-0
        public double IteratedLocalSearch(int maxIter)
        {
            int iter;
            double z;

            iter = 0;
            while (iter < maxIter)
            {
                z = opt10(c).solution;
                dataPerturbation();
                iter++;
            }

            return zub;
        }

        private void dataPerturbation()
        {
            int i, j;
            var cPert = new double[m, n];

            for (i = 0; i < m; i++)
            for (j = 0; j < n; j++)
                cPert[i, j] = c[i, j] + c[i, j] * 0.5 * rnd.NextDouble();

            opt10(cPert);
        }

        // controllo ammissibilità soluzione
        public double checkSol(int[] sol)
        {
            double cost = 0;
            int j;
            var capused = new int[m];

            // controllo assegnamenti
            for (j = 0; j < n; j++)
                if (sol[j] < 0 || sol[j] >= m)
                {
                    cost = double.MaxValue;
                    goto lend;
                }
                else
                {
                    cost += c[sol[j], j];
                }

            // controllo capacità
            for (j = 0; j < n; j++)
            {
                capused[sol[j]] += req[j];
                if (capused[sol[j]] > cap[sol[j]])
                {
                    cost = double.MaxValue;
                    goto lend;
                }
            }

            lend:
            return cost;
        }
    }
}

public class OptimizationResult
{
    public OptimizationResult(List<string> log, double solution)
    {
        this.log = log;
        this.solution = solution;
    }

    public double solution { get; }
    public List<string> log { get; }
}