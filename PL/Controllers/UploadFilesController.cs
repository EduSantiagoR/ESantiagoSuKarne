using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class UploadFilesController : Controller
    {
        [HttpGet]
        public ActionResult Juego()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetResultado()
        {
            HttpPostedFileBase filetxt = Request.Files["txtFile"]; //Recupera el archivo txt cargado en la vista.
            string nuevaRuta = Server.MapPath("~/Files/") + Path.GetFileNameWithoutExtension(filetxt.FileName) + "-" + DateTime.Now.ToString("yyyMMddHHmmss") + ".txt";
            if (!System.IO.File.Exists(nuevaRuta))
            {
                filetxt.SaveAs(nuevaRuta); //Guarda el archivo en la carpeta Files.
                
                StreamReader streamReader = new StreamReader(nuevaRuta); //Leer el archivo de la carpeta Files.
                int contardor = int.Parse(streamReader.ReadLine());
                int maxVentaja = 0;
                int maxLider = 0;
                for (int i = 1; i <= contardor; i++)
                {
                    string linea = streamReader.ReadLine();
                    string[] resultados = linea.Split(' ');
                    int jugador1 = int.Parse(resultados[0]);
                    int jugador2 = int.Parse(resultados[1]);
                    int lider = jugador1 > jugador2 ? 1 : 2;
                    int ventaja = jugador1 > jugador2 ? jugador1 - jugador2 : jugador2 - jugador1;
                    if (maxVentaja < ventaja)
                    {
                        maxVentaja = ventaja;
                        maxLider = lider;
                    }
                }
                string txtResultado = Server.MapPath("~/Files/Resultado") + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                using (StreamWriter streamWriter = new StreamWriter(txtResultado))
                {
                    streamWriter.WriteLine(maxLider + " " + maxVentaja); //Escribir los resultados en el archivo.
                }
                byte[] fileBytes = System.IO.File.ReadAllBytes(txtResultado);
                System.IO.File.Delete(txtResultado); //Eliminar el archivo del servidor.
                Response.Headers.Add("Content-Disposition", $"attachment; filename=Resultado-{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                Response.Headers.Add("Content-Type", "text/plain");
                return File(fileBytes, "text/plain"); //Esta acción descarga la el archivo.
            }
            return RedirectToAction("Juego");
        }

        [HttpGet]
        public ActionResult Mensaje()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMensaje()
        {
            HttpPostedFileBase filetxt = Request.Files["txtFile"]; //Recupera el archivo txt cargado en la vista.
            string nuevaRuta = Server.MapPath("~/Files/") + Path.GetFileNameWithoutExtension(filetxt.FileName) + "-" + DateTime.Now.ToString("yyyMMddHHmmss") + ".txt";
            filetxt.SaveAs(nuevaRuta); //Guarda el archivo en la carpeta Files.
            StreamReader streamReader = new StreamReader(nuevaRuta); //Leer el archivo de la carpeta Files.
            string linea1 = streamReader.ReadLine();
            string primerMensaje = streamReader.ReadLine();
            string segundoMensaje = streamReader.ReadLine();
            string mensajeEncriptado = streamReader.ReadLine();
            char[] letras = mensajeEncriptado.ToArray();
            string mensajeSinRepeticiones = "";
            for (int i = 0; i < letras.Length; i++)
            {
                if(i != letras.Length - 1)
                {
                    if (letras[i] != letras[i + 1])
                    {
                        mensajeSinRepeticiones += letras[i];
                    }
                }
                else
                {
                    mensajeSinRepeticiones += letras[i];
                }
            }
            //Evaluamos si los mensajes existen dentro del mensaje sin repeticiones.
            bool existeMensaje1 = mensajeSinRepeticiones.Contains(primerMensaje);
            bool existeMensaje2 = mensajeSinRepeticiones.Contains(segundoMensaje);
            string txtResultado = Server.MapPath("~/Files/ResultadoDeMensajes") + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            using (StreamWriter streamWriter = new StreamWriter(txtResultado))
            {
                //Escribir los resultados en el archivo TXT de salida
                streamWriter.WriteLine(existeMensaje1 ? "SI" : "NO");
                streamWriter.WriteLine(existeMensaje2 ? "SI" : "NO");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(txtResultado);
            System.IO.File.Delete(txtResultado); //Eliminar el archivo del servidor.
            Response.Headers.Add("Content-Disposition", $"attachment; filename=ResultadoDeMensajes-{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
            Response.Headers.Add("Content-Type", "text/plain");
            return File(fileBytes, "text/plain"); //Esta acción descarga la el archivo.
        }
    }
}