using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using RestSharp.Extensions;

using System.Net;
using System.Media;
using Firebase.Storage;
using WMPLib;

namespace Project
{

    public partial class Playlist : Form
    {
        private WMPLib.WindowsMediaPlayer player;

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "5zLz5ZvHjPWZcJXn6JEYkyjN1feGW0bY4YBBzCod",
            BasePath = "https://soul-song-782cd.firebaseio.com/"
        };

        IFirebaseClient client;

        Form1 captured;
        public Playlist(string emotion, Form1 form1)
        {
            InitializeComponent();
            results.Text = emotion;
            captured = form1;
        }

        private void close_Butt_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void maximize_Butt_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;

        }

        private void minimize_Butt_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void back_Butt_Click(object sender, EventArgs e)
        {
            this.Hide();
            //Form1 f1 = new Form1();
            //f1.ShowDialog();

            captured.Show();
            this.Close();
        }
        private void configure_firebase()
        {
            client = new FireSharp.FirebaseClient(config);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            configure_firebase();

            var res = client.Get("Song/2/");
            Song song = res.ResultAs<Song>();
            //results.Text = song.File;


            //byte[] data = File.ReadAllBytes(@"B:\poli\an3\sem2\SD\songs\Ceux_Qui_Rêvent.mp3");
            //string encoded = Convert.ToBase64String(data); //asta trebe pus in Firebase
            //byte[] convertedData = Convert.FromBase64String(encoded);


            string encoded = song.File1 + song.File2 + song.File3 +
                song.File4 + song.File5 + song.File6 + song.File7 +
                song.File8 + song.File9 + song.File10; 
            byte[] convertedData = Convert.FromBase64String(encoded);

            string path = @"Downloaded Songs\" + song.Name + ".mp3";

            if (!File.Exists(path))
                File.WriteAllBytes(path, convertedData);

            player = new WindowsMediaPlayer();
            player.URL = path;
            player.controls.play();

        }
    }
}

//TODO
//if user, after emotion winform => take all those songs that have the according emotion
//make the winform display all downloaded mp3 files