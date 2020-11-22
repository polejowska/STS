using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialTransmissionSimulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string convertToBinary(string textToConvert)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Insert(0, 0);

            foreach (char L in textToConvert.ToString())
            {
                // Formatowanie tekstu, L - value, 2 - base
                stringBuilder.Append(Convert.ToString(L, 2));
                stringBuilder.Append(11);
                stringBuilder.Append(" ");
                stringBuilder.Append(0);
            }

            string newText;
            newText = stringBuilder.ToString();

            int x = newText.Length;

            return newText.Remove(x-2);
        }

        private void sendToConverter_Click(object sender, EventArgs e)
        {
            converterBinaryCodeBox.Text = convertToBinary(senderTextBox.Text);
        }

        public string convertToASCII(string binaryCode)
        {
            char c = ' ';
            // Rozdzielenie słów na podstawie c
            string[] splitList = binaryCode.Split(c);

            foreach (string s in splitList)
            {
                // Usunięcie bitów startu i stopu
                s.Remove(9);
                s.Remove(8);
                s.Remove(0);
            }

            string newText = string.Join("", splitList);
            // Ilość znaków
            int step = 6;

            List<Byte> bytesList = new List<Byte>();

            for(int i = 0; i < newText.Length; i+= step)
            {
                bytesList.Add(Convert.ToByte(newText.Substring(i, step), 2));
            }

            byte[] bytesNewArray = new byte[bytesList.Count];

            bytesList.CopyTo(bytesNewArray);

            return ASCIIEncoding.ASCII.GetString(bytesNewArray);
        }

        private void sendToReceiver_click(object sender, EventArgs e)
        {
            string sentText = senderTextBox.Text;
            char x = ' ';
            string[] wordsInText = sentText.Split(x);

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\Words.txt");
            string[] badWords = File.ReadAllLines(path);

            for (int i = 0; i < wordsInText.Length; i++)
            {
                for (int j = 0; j < badWords.Length; j++)
                {
                    if (wordsInText[i].Contains(badWords[j]))
                    {
                        foreach (char c in wordsInText[i])
                        {
                            char w = '*';
                            wordsInText[i] = wordsInText[i].Replace(c, w);
                        }
                        receiverTextBox.Text = wordsInText[i];
                    }
                    else
                    {
                        senderTextBox.Text = receiverTextBox.Text;
                    }
                    string newWord = string.Join(" ", wordsInText);
                    senderTextBox.Text = newWord;
                    receiverTextBox.Text = newWord;
                }
            }
        }
        
        private static string ConvertToUTF8String(String binaryCode)
        {
            const int step = 8;

            List<Byte> bytes = new List<Byte>();
            
            for (int i = 0; i < binaryCode.Length; i += step)
            {
                bytes.Add(Convert.ToByte(binaryCode.Substring(i, step), 2));
            }
                
            byte[] newByteArray = new byte[bytes.Count];
            bytes.CopyTo(newByteArray);
            return UTF8Encoding.UTF8.GetString(newByteArray);
        }
        
        public string saveText()
        {
            byte[] bArray = ASCIIEncoding.ASCII.GetBytes(senderTextBox.Text);
            string savedText = ASCIIEncoding.ASCII.GetString(bArray);
            return savedText;
        }

        private void clearSender_Click(object sender, EventArgs e)
        {
            senderTextBox.Clear();
        }

        private void clearConverter_Click(object sender, EventArgs e)
        {
            converterBinaryCodeBox.Clear();
        }

        private void clearReceiver_Click(object sender, EventArgs e)
        {
            receiverTextBox.Clear();
        }

    }
}
