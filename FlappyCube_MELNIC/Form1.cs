using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyCube_MELNIC
{
    public partial class Form1 : Form
    {
        
        //Силы тяжести. Чем она выше, тем быстрее игрок набирает скорость.
        const int gravity = 1;
        //Минимальная толщина обводки
        const int minWidth = 2;

        //переменная которая хранит в себе текущий кадр
        Bitmap bmp;
        //Определяет стиль контуров
        Pen pen;
        //Генерирует высоту появления труб
        Random rnd = new Random();
        //Шрифт очков
        Font f = SystemFonts.DefaultFont;
        //Описание игрока
        Rectangle player;
        //Скорость игрока
        int PV = 0;
        //Количество набранных очков
        int score = 0;

        bool isGlowingOn = true;
        //Трубы
        Rectangle tube1;
        Rectangle tube2;
        Rectangle tube3;
        Rectangle tube4;
        Rectangle tube5;
        Rectangle tube6;
        //Расстояние между трубами 
        int space = 150;
        //Скорость движения труб
        int tubeVelocity = -3;

        SoundPlayer soundPlayer;

        public Form1()
        {
            InitializeComponent();
            //Создаем кадр
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //Задаем стиль и цвет линий
            pen = new Pen(Brushes.Aqua);
            //Размещаем игрока и задаем размеры
            player = new Rectangle(30, 30, 30, 30);
            //Размещаем трубы так чтобы верхние трубы
            //располагались относительно нижних труб
            //на space пикселей выше
            tube1 = new Rectangle(500, 300, 80, 500);
            tube2 = new Rectangle(tube1.X, tube1.Y - tube1.Height - space, 80, 500);
            tube3 = new Rectangle(700, 400, 80, 500);
            tube4 = new Rectangle(tube3.X, tube3.Y - tube3.Height - space, 80, 500);
            tube5 = new Rectangle(900, 500, 80, 500);
            tube6 = new Rectangle(tube5.X, tube5.Y - tube5.Height - space, 80, 500);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream resourceStream = assembly.GetManifestResourceStream(@"FlappyCube_MELNIC.my_music_MELNIK.wav");
            soundPlayer = new SoundPlayer(resourceStream);
            soundPlayer.PlayLooping();

            button5.Enabled = false;
            button6.Enabled = false;
            button5.Visible = false;
            button6.Visible = false;
            label1.Visible = false;
            label1.Enabled = false;
        }
        //Главный цикл игры,
        //в нем происходит отрисовка и игровая логика
        private void timer1_Tick(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(bmp);

            if (pen.Width > minWidth)
            {
                pen.Width--;
            }

            g.Clear(Color.Black);
            
            Draw(g);

            pictureBox1.Image = bmp;
            g.Dispose();
        }
        //Метод отрисовки.
        //Если мы хотим чтобы что-то отобразилось на экране,
        //то мы должны добавить в него соотвествующую строку.
        private void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Yellow, player);
            g.FillRectangle(Brushes.Green, tube1);
            g.FillRectangle(Brushes.Green, tube2);
            g.FillRectangle(Brushes.Green, tube3);
            g.FillRectangle(Brushes.Green, tube4);
            g.FillRectangle(Brushes.Green, tube5);
            g.FillRectangle(Brushes.Green, tube6);


            g.DrawRectangle(pen, player);
            g.DrawRectangle(pen, tube1);
            g.DrawRectangle(pen, tube2);
            g.DrawRectangle(pen, tube3);
            g.DrawRectangle(pen, tube4);
            g.DrawRectangle(pen, tube5);
            g.DrawRectangle(pen, tube6);

            g.DrawString(score.ToString(), f, Brushes.White, 400, 20);
            
        }

        private void TubesLogic()
        {
            
            //Двигаем первую пару труб
            tube1.X += tubeVelocity;
            tube2.X = tube1.X;
            //Двигаем вторую пару труб
            tube3.X += tubeVelocity;
            tube4.X = tube3.X;
            //
            tube5.X += tubeVelocity;
            tube6.X = tube5.X;
            // возвращаем первую пару труб
            if (tube1.Right <= 0)
            {
                tube1.X = pictureBox1.Right;
                tube1.Y = rnd.Next(200, 450);
                tube2.Y = tube1.Y - tube1.Height - space;
            }
            // возвращаем вторую пару труб
            if (tube3.Right <= 0)
            {
                tube3.X = pictureBox1.Right;
                tube3.Y = rnd.Next(200, 450);
                tube4.Y = tube3.Y - tube3.Height - space;
            }
            // 
            
            if (tube5.Right <= 0)
            {
                tube5.X = pictureBox1.Right;
                tube5.Y = rnd.Next(200, 450);
                tube6.Y = tube5.Y - tube5.Height - space;
            }
        }

        private void PlayerLogic()
        {
            //добавляем очки игроку
            score++;
            //Алгоритм движения игрока
            //Скорость увеличивается в зависимости от величины гравитации
            //Игрок за один тик таймера перемещается на расстояние равное скорости
            player.Y += PV;
            PV += gravity;
            //Если игрок столкнулся с нижней частью,
            //то переместить его наверх и сбросить скорость
            //иначе, если игрок столкнулся с верхней частью,
            //то погасить его скоростьи не пустить выше
            if (player.Bottom >= pictureBox1.Bottom)
            {
                player.Y = 0;
                PV = 0;
                score = 0;
                drawtimer.Enabled = false;
                playertimer.Enabled = false;
                tubetimer.Enabled = false;
                label1.Visible = true;
                label1.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button5.Visible = true;
                button6.Visible = true;

            }
            if (player.Y < 0)
            {
                player.Y = 0;
                PV = 0;
                
            }
            /////////////////////////////////
            //Логика столкновений первой пары труб
            /////////////////////////////////
            if (player.Right >= tube1.Left && 
                player.Left <= tube1.Right && 
                player.Bottom >= tube1.Top)
            {
                player.Y = 0;
                PV = 0;
                score = 0;
                
                drawtimer.Enabled = false;
                playertimer.Enabled = false;
                tubetimer.Enabled = false;
                label1.Visible = true;
                label1.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button5.Visible = true;
                button6.Visible = true;
            }

            if (player.Right >= tube2.Left && 
                player.Left <= tube2.Right && 
                player.Top <= tube2.Bottom)
            {
                player.Y = 0;
                PV = 0;
                score = 0;
                drawtimer.Enabled = false;
                playertimer.Enabled = false;
                tubetimer.Enabled = false;
                label1.Visible = true;
                label1.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button5.Visible = true;
                button6.Visible = true;
            }
            /////////////////////////////////
            //Логика столкновений второй пары труб
            /////////////////////////////////
            if (player.Right >= tube3.Left &&
                player.Left <= tube3.Right &&
                player.Bottom >= tube3.Top)
            {
                player.Y = 0;
                PV = 0;
                score = 0;
                drawtimer.Enabled = false;
                playertimer.Enabled = false;
                tubetimer.Enabled = false;
                label1.Visible = true;
                label1.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button5.Visible = true;
                button6.Visible = true;
            }

            if (player.Right >= tube4.Left &&
                player.Left <= tube4.Right &&
                player.Top <= tube4.Bottom)
            {
                player.Y = 0;
                PV = 0;
                score = 0;
                drawtimer.Enabled = false;
                playertimer.Enabled = false;
                tubetimer.Enabled = false;
                label1.Visible = true;
                label1.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button5.Visible = true;
                button6.Visible = true;
            }
            /////////////////////////////////
            //
            /////////////////////////////////
            if (player.Right >= tube5.Left &&
                player.Left <= tube5.Right &&
                player.Bottom >= tube5.Top)
            {
                player.Y = 0;
                PV = 0;
                score = 0;
                drawtimer.Enabled = false;
                playertimer.Enabled = false;
                tubetimer.Enabled = false;
                label1.Visible = true;
                label1.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button5.Visible = true;
                button6.Visible = true;
            }

            if (player.Right >= tube6.Left &&
                player.Left <= tube6.Right &&
                player.Top <= tube6 .Bottom)
            {
                player.Y = 0;
                PV = 0;
                score = 0;
                drawtimer.Enabled = false;
                playertimer.Enabled = false;
                tubetimer.Enabled = false;
                label1.Visible = true;
                label1.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button5.Visible = true;
                button6.Visible = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (label1.Visible == true && label1.Enabled == true && button5.Enabled == true && button6.Enabled == true && button5.Visible == true && button6.Visible == true)
            {
                
            }
            else
            {
                if (e.KeyCode == Keys.Space)
                {
                    if (isGlowingOn)
                    {
                        pen.Width = 10;
                    }
                    drawtimer.Enabled = true;
                    playertimer.Enabled = true;
                    tubetimer.Enabled = true;
                    PV -= 20;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    drawtimer.Enabled = !drawtimer.Enabled;
                    playertimer.Enabled = !playertimer.Enabled;
                    tubetimer.Enabled = !tubetimer.Enabled;
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;
                    button4.Visible = true;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                }
            }
            //if (e.KeyCode == Keys.L)
            //{
                //new LeaderBoardForm(score).Show();
            //}
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Отрисовывыем первый кадр,
            //чтобы экран не был пустым на старте
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(Color.Black);

            Draw(g);

            pictureBox1.Image = bmp;
            g.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new LeaderBoardForm(score).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new settingsForm().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            drawtimer.Enabled = true;
            playertimer.Enabled = true;
            tubetimer.Enabled = true;
        }

        private void playertimer_Tick(object sender, EventArgs e)
        {
            
            PlayerLogic();
        }

        private void tubetimer_Tick(object sender, EventArgs e)
        {
            TubesLogic();
        }

        private void settingsUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string[] settings;
                settings = File.ReadAllLines("settings");
                if (settings.Length >= 3)
                {
                    playertimer.Interval = Convert.ToInt32(settings[0]);
                    tubetimer.Interval = Convert.ToInt32(settings[0]);

                    isGlowingOn = Convert.ToBoolean(settings[2]);
                }
            }
            catch (FileNotFoundException)
            {
                File.Create("settings");
                
            }
            catch (Exception)
            {

            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label1.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button5.Visible = false;
            button6.Visible = false;

            tube1 = new Rectangle(500, 300, 80, 500);
            tube2 = new Rectangle(tube1.X, tube1.Y - tube1.Height - space, 80, 500);
            tube3 = new Rectangle(700, 400, 80, 500);
            tube4 = new Rectangle(tube3.X, tube3.Y - tube3.Height - space, 80, 500);
            tube5 = new Rectangle(900, 500, 80, 500);
            tube6 = new Rectangle(tube5.X, tube5.Y - tube5.Height - space, 80, 500);

            //Отрисовывыем первый кадр,
            //чтобы экран не был пустым на старте
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            Draw(g);
            pictureBox1.Image = bmp;
            g.Dispose();

            drawtimer.Enabled = true;
            playertimer.Enabled = true;
            tubetimer.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            

            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            label1.Visible = false;
            label1.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button5.Visible = false;
            button6.Visible = false;

            tube1 = new Rectangle(500, 300, 80, 500);
            tube2 = new Rectangle(tube1.X, tube1.Y - tube1.Height - space, 80, 500);
            tube3 = new Rectangle(700, 400, 80, 500);
            tube4 = new Rectangle(tube3.X, tube3.Y - tube3.Height - space, 80, 500);
            tube5 = new Rectangle(900, 500, 80, 500);
            tube6 = new Rectangle(tube5.X, tube5.Y - tube5.Height - space, 80, 500);



            //Отрисовывыем первый кадр,
            //чтобы экран не был пустым на старте
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(Color.Black);

            Draw(g);

            pictureBox1.Image = bmp;
            g.Dispose();
        }
    }
}
