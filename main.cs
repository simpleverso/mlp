using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

class ejemplo
{
	static void Main(string[] args)
	{
		double[,] patrones = { {0,0}, {0, 1},{1, 0},{1, 1}};
		perceptron.C = 2;
		perceptron.num_patrones = 4;
		perceptron.numiter = 1000;
		perceptron.emin = 0.01;
		perceptron.alfa = 0.6;
		perceptron.x = patrones;
		
		perceptron.entrenar();
		
		double[] x = new double[] { 0, 0};
		double[] y = perceptron.reconocer_pmc(x);
		foreach (var item in y)
		{
			Console.WriteLine(item);
		}
		Console.ReadLine();
	}
}

public class perceptron
{
	public static int C, numiter, num_patrones;
	public static int[] n;
	public static double alfa, emin;
	public static double[,] x;
	public static double[,] y;
	public static double[,] yd;
	public static double[, ,] winicial;
	public static double[, ,] w;
	public static double[,] a;
	public static double[,] u;
	public static double[,] delta;

	public static double[] xmax;
	public static double[] xmin;
	public static double[] ymax;
	public static double[] ymin;

	public static void entrenar()
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