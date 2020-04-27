using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarrenLee.Media;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.CodeDom.Compiler;

namespace Project
{
    public partial class Form1 : Form
    {

        //---------------
        // Replace <Subscription Key> with your valid subscription key.
        const string subscriptionKey = "c136b8ab43d740bd80735e3be98551a6";

        // replace <myresourcename> with the string found in your endpoint URL
        const string uriBase =
            "https://eastus.api.cognitive.microsoft.com/face/v1.0/detect";

        //---------------


        //int count = 1;
        Camera myCamera = new Camera();

        public Form1()
        {
            InitializeComponent();

            GetInfo();
            myCamera.OnFrameArrived += MyCamera_OnFrameArrived;
        }

        public void GetInfo()
        {
            var cameraResolutions = myCamera.GetSupportedResolutions();

            foreach(var r in cameraResolutions)
            {
                cameraResolution.Items.Add(r);
            }

            cameraResolution.SelectedIndex = 7;
        }

        public void MyCamera_OnFrameArrived(object source, FrameArrivedEventArgs e)
        {
            Image img = e.GetFrame();
            pictureBox1.Image = img;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myCamera.Stop();
        }

        private void cameraResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            myCamera.Start(cameraResolution.SelectedIndex);
        }

        private void captureButt_Click(object sender, EventArgs e)
        {
            //string filename = Application.StartupPath + @"\" + "Image" + count.ToString(); //E:\an3\sem2\SD\Images
            //string filename = @"E:\an3\sem2\SD\Project\Project\bin\Debug\Images\Image" + count.ToString(); //--good one
            string filename = @"E:\an3\sem2\SD\Project\Project\bin\Debug\Images\Image";
            myCamera.Capture(filename);
            //count++;

            Process.Start(@"E:\an3\sem2\SD\Project\Project\bin\Debug\Images");

        }

        private void detect_butt_Click(object sender, EventArgs e)
        {
            //------AI------
            launchEmotionDetection();
        }

        private void launchEmotionDetection()
        {
            string imageFilePath = @"E:\an3\sem2\SD\Project\Project\bin\Debug\Images\Image.jpg";

            if (File.Exists(imageFilePath))
            {
                try
                {
                    MakeAnalysisRequest(imageFilePath);
                    results.Text = "\nWait a moment for the results to appear.\n";
                    //Console.WriteLine("\nWait a moment for the results to appear.\n");
                }
                catch (Exception e)
                {
                    results.Text = "\n" + e.Message + "\nPress Enter to exit...\n";
                    //Console.WriteLine("\n" + e.Message + "\nPress Enter to exit...\n");
                }
            }
            else
            {
                results.Text = "\nInvalid file path.\nPress Enter to exit...\n";
                //Console.WriteLine("\nInvalid file path.\nPress Enter to exit...\n");
            }
            Console.ReadLine();
        }

        async void MakeAnalysisRequest(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            //string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
            //    "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
            //    "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=emotion";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.

                results.Text = "\nResponse:\n";
                results.Text = JsonPrettyPrint(contentString);

            }
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }


    }
}
