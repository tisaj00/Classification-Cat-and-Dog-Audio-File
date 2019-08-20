using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing;
using IronPython.Runtime.Operations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NAudio.Wave;

namespace Audio_Cat_Dog
{

    public partial class Form1 : Form
    {
        private List<string> dataList;
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder retstring, int returnlenth, IntPtr callback);
        public Form1()
        {
            InitializeComponent();
            mciSendString("open new Type waveaudio alias recsound", null, 0, IntPtr.Zero);
            btnRecord.Click += new EventHandler(btnRecord_Click);
            btnStopSave.Enabled = false;
            
        }


        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Audio Files|*.wav;*.WAV;";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                string fileName = null;
                fileName = Path.GetFileName(path);
                lblFileName.Text = "Audio file: " + fileName;
                Console.WriteLine("Execute python process...");
                Option1_ExecProcess(path);
                




            }
        }
        void Option1_ExecProcess(String filePath)
        {

            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\Josip\AppData\Local\Programs\Python\Python37-32\python.exe";

            
            var script = @"C:\Users\Josip\PycharmProjects\cat_dog\featrue_extractionInC#.py";
            psi.Arguments = $"\"{script}\" \"{filePath}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            Console.WriteLine("ERRORS:");
            Console.WriteLine(errors);
            Console.WriteLine();
            Console.WriteLine("Results:");
            Console.WriteLine(results);
            results = results.Replace("[", string.Empty);
            results = results.Replace("]", string.Empty);
            results = results.Replace("\r\n", string.Empty);
            dataList = results.Split(',').ToList();
            Console.WriteLine(dataList);
            txtValue1.Text = dataList[0].ToString();
            txtValue2.Text = dataList[1].ToString();
            txtValue3.Text = dataList[2].ToString();
            txtValue4.Text = dataList[3].ToString();
            txtValue5.Text = dataList[4].ToString();
            txtValue6.Text = dataList[5].ToString();
            txtValue7.Text = dataList[6].ToString();
            txtValue8.Text = dataList[7].ToString();
            txtValue9.Text = dataList[8].ToString();
            txtValue10.Text = dataList[9].ToString();
            txtValue11.Text = dataList[10].ToString();
            txtValue12.Text = dataList[11].ToString();



        }

        private async void button2_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                        {
                            "input1",
                            new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
                                            {
                                                "Name", ""
                                            },
                                            {
                                                "Value1", dataList[0]
                                            },
                                            {
                                                "Value2", dataList[1]
                                            },
                                            {
                                                "Value3", dataList[2]
                                            },
                                            {
                                                "Value4", dataList[3]
                                            },
                                            {
                                                "Value5", dataList[4]
                                            },
                                            {
                                                "Value6", dataList[5]
                                            },
                                            {
                                                "Value7", dataList[6]
                                            },
                                            {
                                                "Value8", dataList[7]
                                            },
                                            {
                                                "Value9", dataList[8]
                                            },
                                            {
                                                "Value10", dataList[9]
                                            },
                                            {
                                                "Value11", dataList[10]
                                            },
                                            {
                                                "Value12", dataList[11]
                                            },
                                            {
                                                "Prediction", ""
                                            },

                                }
                            }

                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                const string apiKey = "Npel02M5oYuWlcSvqnDPs0efXldWYGgO1KaNMCr5oYKEKzCbeL/5yfqaVETsxRmitb7l11j8olcKOxBd2NfTLg==";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/8bcde83445d24d5b9f39579953185a58/services/3bc74986fbfd4034acce4e4be81746e1/execute?api-version=2.0&format=swagger");
                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {

                    string result = await response.Content.ReadAsStringAsync();
                    int startIndex = result.IndexOf("Scored Labels") + 16;
                    int scoredLabel = Int32.Parse(result.Substring(startIndex, 1));
                    if (scoredLabel == 1)
                    {
                        Image img = Image.FromFile(@"C:\Users\Josip\source\repos\Audio_Cat_Dog\Audio_Cat_Dog\Resources\cat.jpg");
                        txtResults.Text = "Cat";
                        pictureBox1.Image = new Bitmap(img);
                        Console.WriteLine("Result: {0}", result);
                    }
                    else
                    {
                        Image img1 = Image.FromFile(@"C:\Users\Josip\source\repos\Audio_Cat_Dog\Audio_Cat_Dog\Resources\dog.jpg");
                        txtResults.Text = "Dog";
                        pictureBox1.Image = new Bitmap(img1);
                        Console.WriteLine("Result: {0}", result);
                    }



                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));
                    Console.WriteLine(response.Headers.ToString());
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            mciSendString("save recsound C:\\Users\\Josip\\source\\repos\\Audio_Cat_Dog\\Audio_Cat_Dog\\Recorded\\zvuk.wav", null, 0, IntPtr.Zero);
            mciSendString("close recsound", null, 0, IntPtr.Zero);
            this.btnRecord.Enabled = true;
            this.btnStopSave.Enabled = false;


        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            mciSendString("record recsound", null, 0, IntPtr.Zero);
            btnRecord.Click += new EventHandler(this.button3_Click);
            this.btnRecord.Enabled = false;
            this.btnStopSave.Enabled = true;
        }
    }

}
