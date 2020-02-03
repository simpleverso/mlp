using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Aplicacion_perceptron
{
    public class rna_pmc
    {
        private static int C, numiter, num_patrones;
        private static int[] n;
        private static double alfa, emin;
        private static double[,] x;
        private static double[,] y;
        private static double[,] yd;
        private static double[, ,] winicial;
        private static double[, ,] w;
        private static double[,] a;
        private static double[,] u;
        private static double[,] delta;

        private static double[] xmax;
        private static double[] xmin;
        private static double[] ymax;
        private static double[] ymin;

        public static void entrenar_pmc(String nombre_archivo)
        {
            int i, j, k, c, aux, p;
            double error_total = 1;

            int N = num_patrones;

            Random rand = new Random();

            //Variables para almacenar los valores máximos y mínimos de entrada y salida
            xmax = new double[n[1] + 1];
            xmin = new double[n[1] + 1];
            ymax = new double[n[C] + 1];
            ymin = new double[n[C] + 1];

            for (i = 1; i <= n[1]; i++)
            {
                xmax[i] = 0;
                xmin[i] = 10000;
            }

            for (i = 1; i <= n[C]; i++)
            {
                ymax[i] = 0;
                ymin[i] = 10000;
            }

            //Se encuentras los valores máximos y mínimos de x
            for (i = 1; i <= n[1]; i++)
            {
                for (p = 1; p <= N; p++)
                {
                    if (xmax[i] < x[p, i])
                    {
                        xmax[i] = x[p, i];
                    }
                    if (xmin[i] > x[p, i])
                    {
                        xmin[i] = x[p, i];
                    }
                }
            }

            //Se encuentran los valores máximos y mínimos de y
            for (i = 1; i <= n[C]; i++)
            {
                for (p = 1; p <= N; p++)
                {
                    if (ymax[i] < y[p, i])
                    {
                        ymax[i] = y[p, i];
                    }
                    if (ymin[i] > y[p, i])
                    {
                        ymin[i] = y[p, i];
                    }
                }
            }

            aux = 0;

            for (i = 1; i <= C; i++)
            {
                if (aux < n[i])
                    aux = n[i];
            }

            //Se asigna memmoria dinámica a los pesos y umbrales de acuerdo a la arquitectura de la red
            winicial = new double[C, aux + 1, aux + 1];
            w = new double[C, aux + 1, aux + 1];
            u = new double[C + 1, aux + 1];
            a = new double[C + 1, aux + 1];
            delta = new double[C + 1, aux + 1];

            //Seasignan valores aleatorios a los pesos
            for (c = 1; c <= C - 1; c++)
            {
                for (j = 1; j <= n[c]; j++)
                {
                    for (i = 1; i <= n[c + 1]; i++)
                    {
                        //w[c, j, i] = rand.Next(100) / 100.0;
                        w[c, j, i] = rand.NextDouble();
                        winicial[c, j, i] = w[c, j, i];
                    }
                }
            }

            //Se asignan valores de uno a los umbrales
            for (c = 2; c <= C; c++)
            {
                for (i = 1; i <= n[c]; i++)
                {
                    //u[c, i] = 1;
                    u[c, i] = rand.NextDouble();
                }
            }

            //Se normalizan los patrones de entrada y de salida
            for (i = 1; i <= n[1]; i++)
            {
                for (p = 1; p <= N; p++)
                {
                    x[p, i] = (1.0 / (xmax[i] - xmin[i])) * (x[p, i] - xmin[i]);
                }
            }

            for (i = 1; i <= n[C]; i++)
            {
                for (p = 1; p <= N; p++)
                {
                    y[p, i] = (1.0 / (ymax[i] - ymin[i])) * (y[p, i] - ymin[i]);
                }
            }

            k = 0;
            while (k < numiter && error_total >= emin)
            {
                error_total = 0;
                for (p = 1; p <= N; p++)
                {
                    //Activación de la capa de entrada.
                    for (i = 1; i <= n[1]; i++)
                    {
                        a[1, i] = x[p, i];
                    }

                    //Propagación de las activaciones de las capas ocultas y la capa de salida.
                    for (c = 2; c <= C; c++)
                    {
                        for (i = 1; i <= n[c]; i++)
                        {
                            double f = 0;
                            for (j = 1; j <= n[c - 1]; j++)
                            {
                                f += w[c - 1, j, i] * a[c - 1, j];
                            }
                            f += u[c, i];
                            a[c, i] = 1.0 / (1 + Math.Exp(-f));

                            //Almacena los valores obtenidos.
                            if (c == C)
                                yd[p, i] = a[c, i];
                        }
                    }

                    double error = 0;
                    for (i = 1; i <= n[C]; i++)
                    {
                        error += Math.Pow(y[p, i] - a[C, i], 2);
                    }

                    error = 0.5 * error;
                    error_total += error;

                    //Cálculculo de las delta.

                    //Caso a:
                    for (i = 1; i <= n[C]; i++)
                    {
                        delta[C, i] = -(y[p, i] - yd[p, i]) * yd[p, i] * (1 - yd[p, i]);
                    }

                    //Caso b:
                    for (c = C - 1; c > 1; c--)
                    {
                        for (j = 1; j <= n[c]; j++)
                        {
                            double s = 0;
                            for (i = 1; i <= n[c + 1]; i++)
                            {
                                s += delta[c + 1, i] * w[c, j, i];
                            }

                            delta[c, j] = a[c, j] * (1 - a[c, j]) * s;
                        }
                    }

                    //Actualización de pesos.
                    for (c = 1; c < C; c++)
                    {
                        for (i = 1; i <= n[c + 1]; i++)
                        {
                            for (j = 1; j <= n[c]; j++)
                            {
                                w[c, j, i] = w[c, j, i] - (alfa * delta[c + 1, i] * a[c, j]);
                            }
                            u[c + 1, i] = u[c + 1, i] - (alfa * delta[c + 1, i]);
                        }
                    }

                    //Actualización de umbrales.
                    //for (c = 1; c < C; c++)
                    //{
                    //    for (j = 0; j <= n[c + 1]; j++)
                    //    {
                    //        u[c + 1, j] = u[c + 1, j] + (alfa * delta[c + 1, j]);
                    //    }
                    //}
                }
                error_total = (1.0 / N) * error_total;
                k++;
            }

            //Se almacenan los pesos y umbrales en un archivo *.ppm
            //(pesos perceptrón multicapa)

            FileStream pesos = new FileStream(nombre_archivo, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter escribir = new StreamWriter(pesos);

            escribir.Write("{0}", C);

            for (i = 1; i <= C; i++)
            {
                escribir.Write(" {0}", n[i]);
            }

            escribir.WriteLine();

            for (i = 1; i <= n[1]; i++)
            {
                escribir.Write("{0} {1} ", xmax[i], xmin[i]);
            }

            escribir.WriteLine();

            for (i = 1; i <= n[C]; i++)
            {
                escribir.Write("{0} {1} ", ymax[i], ymin[i]);
            }

            escribir.WriteLine();
            escribir.WriteLine();

            //Almacenar en el archivo los pesos.
            for (c = 1; c <= C - 1; c++)
                for (j = 1; j <= n[c]; j++)
                    for (i = 1; i <= n[c + 1]; i++)
                        escribir.WriteLine("{0}", w[c, j, i]);

            escribir.WriteLine();

            for (c = 2; c <= C; c++)
                for (i = 1; i <= n[c]; i++)
                    escribir.WriteLine("{0}", u[c, i]);

            escribir.WriteLine();

            //Escribir en el archivo la comparación de los valores deseados y obtenidos de la red
            for (c = 1; c <= n[C]; c++)
                escribir.Write("y[{0}]	yd[{1}]		", c, c);

            escribir.WriteLine();

            escribir.WriteLine();

            for (p = 1; p <= num_patrones; p++)
            {
                for (i = 1; i <= n[C]; i++)
                {
                    yd[p, i] = (yd[p, i] * (ymax[i] - ymin[i])) + ymin[i];
                    y[p, i] = (y[p, i] * (ymax[i] - ymin[i])) + ymin[i];
                    escribir.Write("{0} {1} ", y[p, i], yd[p, i]);
                }
                escribir.WriteLine();
            }

            escribir.Close();
        }

        //Método para leer la arquitectura y los pesos y umbrales almacenados en un archivo *.ppm
        public static void abrir_pesos(String nombre_archivo)
        {
            FileStream archivos;
            StreamReader leer;

            int c, i, j, aux;
            String texto;
            String[] datos;

            archivos = new FileStream(nombre_archivo, FileMode.Open, FileAccess.Read);
            leer = new StreamReader(archivos);

            //Se lee la primera línea que corresponde a la arquitectura de la red
            texto = leer.ReadLine();
            datos = texto.Split(' ');

            C = Convert.ToInt16(datos[0]);
            n = new int[C + 1];

            for (i = 1; i <= C; i++)
            {
                n[i] = Convert.ToInt16(datos[i]);
            }


            aux = 0;

            for (i = 1; i <= C; i++)
            {
                if (aux < n[i])
                    aux = n[i];
            }

            //Variables para almacenar los pesos y umbrales
            w = new double[C, aux + 1, aux + 1];
            u = new double[C + 1, aux + 1];

            //Variables para almacenar los valores máximos y mínimos de entrada y salida
            xmax = new double[n[1] + 1];
            xmin = new double[n[1] + 1];
            ymax = new double[n[C] + 1];
            ymin = new double[n[C] + 1];

            //Se lee la segunda línea que corresponde los valores máximos y mínimos
            //de los patrones de entrada
            texto = leer.ReadLine();
            datos = texto.Split(' ');
            j = 0;
            for (i = 1; i <= n[1]; i++)
            {
                xmax[i] = Convert.ToDouble(datos[j]);
                xmin[i] = Convert.ToDouble(datos[j + 1]);
                j += 2;
            }

            //Se lee la tercera línea que corresponde los valores máximos y mínimos
            //de los patrones de salida
            texto = leer.ReadLine();
            datos = texto.Split(' ');
            j = 0;
            for (i = 1; i <= n[C]; i++)
            {
                ymax[i] = Convert.ToDouble(datos[j]);
                ymin[i] = Convert.ToDouble(datos[j + 1]);
                j += 2;
            }

            texto = leer.ReadLine();

            //A partir de esta línea se leen los pesos
            for (c = 1; c <= C - 1; c++)
                for (j = 1; j <= n[c]; j++)
                    for (i = 1; i <= n[c + 1]; i++)
                        w[c, j, i] = Convert.ToDouble(leer.ReadLine());

            texto = leer.ReadLine();

            //Apartir de esta línea se leen los umbrales
            for (c = 2; c <= C; c++)
                for (i = 1; i <= n[c]; i++)
                    u[c, i] = Convert.ToDouble(leer.ReadLine());
            leer.Close();
        }

        //Método para clasificar un nuevo patron de entrada
        public static double[] reconocer_pmc(double[] x)
        {
            int c, i, j, aux;
            double[,] a;
            double[] ys = new double[n[C] + 1];
            aux = 0;

            for (i = 1; i <= C; i++)
            {
                if (aux < n[i])
                    aux = n[i];
            }

            a = new double[C + 1, aux + 1];
            yd = new double[2, n[C] + 1];

            //Se normalizan los patrones de entrada
            for (i = 1; i <= n[1]; i++)
            {
                x[i] = (1.0 / (xmax[i] - xmin[i])) * (x[i] - xmin[i]);
            }

            //Activación de la capa de entrada.
            for (i = 1; i <= n[1]; i++)
            {
                a[1, i] = x[i];
            }

            //Propagación de las activaciones de las capas ocultas y la capa de salida.
            for (c = 2; c <= C; c++)
            {
                for (i = 1; i <= n[c]; i++)
                {
                    double f = 0;
                    for (j = 1; j <= n[c - 1]; j++)
                    {
                        f += w[c - 1, j, i] * a[c - 1, j];
                    }
                    f += u[c, i];
                    a[c, i] = 1.0 / (1 + Math.Exp(-f));

                    //Almacena los valores obtenidos.
                    if (c == C)
                    {
                        yd[1, i] = a[c, i];
                        yd[1, i] = (yd[1, i] * (ymax[i] - ymin[i])) + ymin[i];
                    }
                }
            }

            for (i = 1; i <= n[C]; i++)
            {
                ys[i] = yd[1, i];
            }
            return ys;
        }
    }
}