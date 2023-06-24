using EspacioMonedas;
using System.Net;
using System.Text.Json;
internal class Program
{
    private static void Main(string[] args)
    {
       var url=$"https://api.coindesk.com/v1/bpi/currentprice.json";
       var request = (HttpWebRequest)WebRequest.Create(url);
       request.Method="GET";
       request.ContentType="application/json";
       request.Accept="application/json";
       try
       {
            using(WebResponse response = request.GetResponse())
            {
                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader!=null)
                    {
                        using(StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            var datosPrecios = JsonSerializer.Deserialize<CoinDeskApi>(responseBody);
                            PreciosDisponibles(datosPrecios);

                            bool flag = true;
                            int opcion;
                            string inputOpcion;
                            while (flag)
                            {
                                
                                EscrituraConEfecto("***** Elija una moneda *****\n",100);
                                Console.WriteLine("1:---USD---\n2:---EUR---\n3:---BGP---");
                                do
                                {
                                    inputOpcion = Console.ReadLine()!;
                                } while (string.IsNullOrEmpty(inputOpcion));

                                bool resultado = int.TryParse(inputOpcion, out opcion);
                            
                                if (resultado && 1<=opcion && opcion<=3)
                                {
                                        switch (opcion)
                                    {
                                        case 1:
                                            MostrarCaracteristicas("USD",datosPrecios.bpi.USD);
                                            break;
                                        case 2:
                                            MostrarCaracteristicas("EUR",datosPrecios.bpi.EUR);
                                            break;
                                        case 3:
                                            MostrarCaracteristicas("GBP",datosPrecios.bpi.GBP);
                                            break;
                                    }
                                    String input2;
                                    Console.WriteLine("Desea ver la caracteristica de otra moneda: y:[yes] - n[no]");
                                    do
                                    {
                                        input2 = Console.ReadLine()!;
                                    } while (String.IsNullOrEmpty(input2));

                                    if (input2 == "n")
                                    {
                                        flag = false;
                                        EscrituraConEfecto("Finalizó la ejecución del programa...", 100);

                                    }
                                }
                                else
                                    {
                                        Console.WriteLine("Opcion Invalida");
                                    }
                            }
                        }
                    }
                }
            }
       }
       catch (WebException)
        {
            Console.WriteLine("Problemas de acceso a la API");
       }
    
    }

    private static void EscrituraConEfecto(string texto, int velocidad)
    {
        for (int i = 0; i < texto.Length; i++)
        {
            Console.Write(texto[i]);
            Thread.Sleep(velocidad);
        }
    }

    private static void PreciosDisponibles(CoinDeskApi? datosPrecios)
    {
        Console.WriteLine("***** Precios Disponibles para:{0} ******",datosPrecios.chartName);
        Console.WriteLine(datosPrecios.bpi.USD.code + ":" + datosPrecios.bpi.USD.rate_float);
        Console.WriteLine(datosPrecios.bpi.EUR.code + ":" + datosPrecios.bpi.EUR.rate_float);
        Console.WriteLine(datosPrecios.bpi.GBP.code + ":" + datosPrecios.bpi.GBP.rate_float);
    }
    private static void MostrarCaracteristicas(string NombreMoneda, dynamic moneda){
        Console.WriteLine("***** Caracteristicas:{0} *****",NombreMoneda);
        Console.WriteLine("Codigo:{0}",moneda.code);
        Console.WriteLine("Simbolo:{0}",moneda.symbol);
        Console.WriteLine("Rate:{0}",moneda.rate);
        Console.WriteLine("Descripcion:{0}",moneda.description);
        Console.WriteLine("Rate float:{0}",moneda.rate_float);
    }
    
}