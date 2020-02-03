using System;

class Untitled
{
	static void Main(string[] args)
	{
		Aplicacion_perceptron.rna_pmc algo = new Aplicacion_perceptron.rna_pmc();
		rna_pmc.leer_archivo_pmc(@".\funcion.pml");
		rna_pmc.entrenar_pmc(@".\funcion.ppm");
		
		double[] x = new double[] { 0.0, 250, 95, 49, 20 };
		double[] y;
		rna_pmc.abrir_pesos(@".\funcion.ppm");
		y = rna_pmc.reconocer_pmc(x);
	}
}