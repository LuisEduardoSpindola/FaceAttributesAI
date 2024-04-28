using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FaceReconAI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Caminho da imagem
            string imagePath = Path.Combine("Uploads", "foto2.png");

            // Chamar a função de análise facial em Python e obter o resultado
            var result = AnalyzeImage(imagePath);

            return View(result);
        }

        // Método para chamar o script Python e realizar a análise facial
        private dynamic AnalyzeImage(string imagePath)
        {
            // Caminho para o script Python
            string pythonScriptPath = Path.Combine("Recognition", "ReconMain.py");

            // Construir o comando para chamar o script Python
            string command = $"python {pythonScriptPath} {imagePath}";

            // Executar o comando e obter o resultado
            string result = ExecuteCommand(command);

            // Converter o resultado para um objeto dinâmico
            dynamic resultObject = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

            return resultObject;
        }

        // Método para executar comandos no terminal
        private string ExecuteCommand(string command)
        {
            // Configurar o processo de inicialização
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "/bin/bash";
            processInfo.RedirectStandardInput = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;

            // Iniciar o processo
            Process process = new Process();
            process.StartInfo = processInfo;
            process.Start();

            // Escrever o comando no terminal
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
            process.StandardInput.Close();

            // Ler e retornar a saída do terminal
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }
    }
}
