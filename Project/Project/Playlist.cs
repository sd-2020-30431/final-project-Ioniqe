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
using System.Threading;

namespace Project
{

    public partial class Playlist : Form
    {
        private WMPLib.WindowsMediaPlayer player = new WindowsMediaPlayer();

        private Song currentlyPlaying;
        private int currentlyPlaying_pos = 0;

        private List<Song> playlist = new List<Song>();

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };

        IFirebaseClient client;

        Form1 captured;
        string emotion;

        public Playlist(string emotion, Form1 form1)
        {
            InitializeComponent();
            results.Text = emotion;
            captured = form1;
            this.emotion = emotion;

            Song_Form.PlayClick += UserClickedOnPlay; //listener for the buttons from flowlayoutcontrol

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            this.playlist = getSongs();
            loadPlaylist(); //also play song

        }

        private void UserClickedOnPlay(object sender, EventArgs e)
        {
            pauseSong(currentlyPlaying_pos);
            Song_Form song_form = (Song_Form)sender;
            playCurrentSong(song_form);

            int i = 0;
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control.GetType() == typeof(Song_Form))
                {
                    Song_Form sf = (Song_Form)control;
                    Song s = sf.Song;
                    if (sf == song_form)
                    {
                        currentlyPlaying_pos = i;
                    }
                    i++;
                }
            }
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

        private void downloaded(Song song)
        {
            string encoded = song.File1 + song.File2 + song.File3 +
                song.File4 + song.File5 + song.File6 + song.File7 +
                song.File8 + song.File9 + song.File10;
            byte[] convertedData = Convert.FromBase64String(encoded);

            string path = @"Downloaded Songs\" + song.Name + ".mp3";

            if (!File.Exists(path))
                File.WriteAllBytes(path, convertedData);
        }

        private void back_Butt_Click(object sender, EventArgs e)
        {
            player.controls.stop();

            this.Hide();
            captured.Show();
            this.Close();
        }

        private void configure_firebase()
        {
            client = new FireSharp.FirebaseClient(config);
        }

        private List<Song> getSongs()
        {
            configure_firebase();

            List<Song> playlist = new List<Song>();
            int counter = 1;
            var response = client.Get(@"Song/" + counter + "/Emotion");

            while (response.Body != "null")
            {
                string currEmotion = response.Body.Trim(new char[] { '"' });


                if (currEmotion == emotion)
                {

                    var responseSong = client.Get(@"Song/" + counter);

                    Song song = responseSong.ResultAs<Song>();

                    playlist.Add(song);
                }

                counter++;
                response = client.Get(@"Song/" + counter + "/Emotion");
            }

            return playlist;
        }

        private int i = 0;
        private Color dark_blue = Color.FromArgb(18, 23, 39);
        private Color light_blue = Color.FromArgb(11, 29, 49);

        private void loadPlaylist()
        {
            foreach (Song song in this.playlist)
            {
                if (i == 0) //also play it
                {
                    pictureBox1.Image = getImage(song.Picture);
                    songLabel.Text = song.Name;
                    artistLabel.Text = song.Artist;
                    currentlyPlaying = song;

                    downloaded(song);

                    string path = @"Downloaded Songs\" + song.Name + ".mp3";
                    player.URL = path;
                    //player.controls.play(); //play the song


                    Song_Form sf = new Song_Form(); //display in flowlayoutpanel
                    sf.Song = song;
                    sf.loadSong();

                    sf.makeButt_invisible();

                    if (i % 2 == 0)
                        sf.BackColor = light_blue;
                    else
                        sf.BackColor = dark_blue;


                    flowLayoutPanel1.Controls.Add(sf);
                }
                else//just add it into the list
                {
                    Song_Form sf = new Song_Form(); //display in flowlayoutpanel
                    sf.Song = song;
                    sf.loadSong();

                    if (i % 2 == 0)
                        sf.BackColor = light_blue;
                    else
                        sf.BackColor = dark_blue;

                    flowLayoutPanel1.Controls.Add(sf);
                }

                i++;
            }
        }

        private Image getImage(string img)
        {
            Image image;
            byte[] array = Convert.FromBase64String(img);
            using (var ms = new MemoryStream(array))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }

        Boolean paused = false;
        private void play_Click(object sender, EventArgs e)
        {
            if (!paused) //playing
            {
                player.controls.pause();
                paused = true;
                play.Image = new Bitmap(@"icons\play.png");
            }
            else//paused
            {
                player.controls.play();
                paused = false;
                play.Image = new Bitmap(@"icons\pause.png");

                button1_Click(sender, e);
            }


        }

        private void playCurrentSong(Song_Form sf)
        {
            Song song = sf.Song;
            sf.makeButt_invisible();

            pictureBox1.Image = getImage(song.Picture);
            songLabel.Text = song.Name;
            artistLabel.Text = song.Artist;
            currentlyPlaying = song;

            downloaded(song);

            string path = @"Downloaded Songs\" + song.Name + ".mp3";
            player.URL = path;
            player.controls.play();

        }

        private void skip_next_Click(object sender, EventArgs e)
        {
            currentlyPlaying_pos++;

            int i = 0;
            if (currentlyPlaying_pos == playlist.Count)
            {
                currentlyPlaying_pos = 0;
                pauseSong(playlist.Count - 1);
            }

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control.GetType() == typeof(Song_Form))
                {
                    Song_Form sf = (Song_Form)control;

                    if (i == currentlyPlaying_pos - 1)
                    {
                        pauseSong(currentlyPlaying_pos - 1);
                    }

                    else if (i == currentlyPlaying_pos)
                    {
                        playCurrentSong(sf);

                        break;
                    }

                    i++;
                }

            }

        }

        private void pauseSong(int pos)
        {
            int i = 0;
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control.GetType() == typeof(Song_Form))
                {
                    if (i == pos)
                    {
                        Song_Form sf = (Song_Form)control;
                        sf.makeButt_visible();
                        return;
                    }

                    i++;
                }
            }
        }

        private void skip_back_Click(object sender, EventArgs e)
        {
            currentlyPlaying_pos--;

            if (currentlyPlaying_pos == playlist.Count - 1)
            {
                pauseSong(0);
            }
            else if (currentlyPlaying_pos == -1)
            {
                pauseSong(0);
                currentlyPlaying_pos = playlist.Count - 1;
            }
            else
            {
                pauseSong(currentlyPlaying_pos + 1);
            }

            int i = 0;

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control.GetType() == typeof(Song_Form))
                {
                    Song_Form sf = (Song_Form)control;

                    if (i == currentlyPlaying_pos)
                    {
                        playCurrentSong(sf);
                        break;
                    }

                    i++;
                }

            }
        }


        private void changeValue(object sender, EventArgs e)
        {
            player.settings.volume = volume.Value;
        }

        private void changePosition(object sender, EventArgs e)
        {
            player.controls.currentPosition = slider.Value * (int)player.currentMedia.duration / 100;
        }





        System.Threading.ManualResetEvent _busy = new System.Threading.ManualResetEvent(false);
        //_busy.Reset(); => pause 
        //_busy.Set(); => resume
        private void button1_Click(object sender, EventArgs e)
        {
            slider.MaximumValue = (int)player.currentMedia.duration;
            results.Text = slider.MaximumValue.ToString();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            while (player.controls.currentPosition <= player.currentMedia.duration) //player.currentMedia.duration e 258 indiferent de melodie
            {
                    Thread.Sleep(1000);
                    backgroundWorker1.ReportProgress(0);
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            slider.Value += 1;
            results.Text = slider.Value.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Task completed!");
        }

    }
}
