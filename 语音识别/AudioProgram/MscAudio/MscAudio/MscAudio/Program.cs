using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MscAudio {


    class Program {

        static void Main(string[] args) {

            string path = @"E:\vs_ProJect\Audio\MscAudio\MscAudio\MscAudio\iflytek01.wav";

            new SpeechRecognition(path).Begin();



            Console.ReadKey();
        }
    }
}