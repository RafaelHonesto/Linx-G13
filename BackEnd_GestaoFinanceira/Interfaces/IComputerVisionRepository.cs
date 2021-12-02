using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Interfaces
{
    public interface IComputerVisionRepository
    {
        public ReadOperationResult ReadFile(string url);
    }
}
