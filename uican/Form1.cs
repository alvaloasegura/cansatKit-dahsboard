using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Defaults;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using System.Runtime.InteropServices;
using System.IO.Ports;
//using System.Threading;
using LiveChartsCore.SkiaSharpView.SKCharts;
using System.Diagnostics;

namespace uican

{
    public partial class Form1 : Form
    {
        private readonly Random _random = new();
        private readonly ObservableCollection<ObservablePoint> _observableValuesH;
        private readonly ObservableCollection<ObservablePoint> _observableValuesT;
        private readonly ObservableCollection<ObservablePoint> _observableValuesP;
        private readonly ObservableCollection<ObservablePoint> _observableValuesGx;
        private readonly ObservableCollection<ObservablePoint> _observableValuesGy;
        private readonly ObservableCollection<ObservablePoint> _observableValuesGz;
        private readonly ObservableCollection<ObservablePoint> _observableValuesAx;
        private readonly ObservableCollection<ObservablePoint> _observableValuesAy;
        private readonly ObservableCollection<ObservablePoint> _observableValuesAz;

        int _baudrate = 9600;

        int timesWindow = 20;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Datos
        {
            public UInt16 temperatura;
            public float altura;
            public byte humedad;
            public float acelX;
            public float acelY;
            public float acelZ;
            public float gyX;
            public float gyY;
            public float gyZ;
        }


        public static unsafe byte[] ConvertToBytes<T>(T value) where T : unmanaged
        {
            byte* pointer = (byte*)&value;

            byte[] bytes = new byte[sizeof(T)];
            for (int i = 0; i < sizeof(T); i++)
            {
                bytes[i] = pointer[i];
            }

            return bytes;
        }
        T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return stuff;
        }

        public ObservableCollection<ISeries> SeriesT { get; set; }
        public ObservableCollection<ISeries> SeriesH { get; set; }
        public ObservableCollection<ISeries> SeriesP { get; set; }
        public ObservableCollection<ISeries> SeriesG { get; set; }
        public ObservableCollection<ISeries> SeriesA { get; set; }
        public Form1()
        {
            InitializeComponent();


            var bytes = new byte[] { 0x64, 0x00, 0x00, 0x00, 0x48, 0x41 };

            //Datos dat = new Datos { temp = 10, alt = 5 };

            //Datos received = ByteArrayToStructure<Datos>(bytes);

            serialPort1.BaudRate = _baudrate;

            //label1.Text = DateTime.Now.Ticks.ToString();
            //int t = System.Runtime.InteropServices.Marshal.SizeOf(dat);
            //label1.Text = t.ToString();

            //byte[] bytess = ConvertToBytes(received);
            //string hexString = BitConverter.ToString(bytess);
            //label1.Text = received.alt.ToString();

            _observableValuesH = new ObservableCollection<ObservablePoint>
            {
                // Use the ObservableValue or ObservablePoint types to let the chart listen for property changes 
                // or use any INotifyPropertyChanged implementation 
                //new ObservablePoint(dates.Ticks,2),
                //new (dates.AddSeconds(2).Ticks,3)
                //new(times.add, 5), // the ObservableValue type is redundant and inferred by the compiler (C# 9 and above)
                //new(DateTime.Now.AddSeconds(10).Ticks,4),
                //new(DateTime.Now.AddSeconds(15).Ticks,5),

                //new ObservablePoint(0,2),
                //new (1,5),
                //new (2,8),
                //new (3,10),
                //new(DateTime.Now.AddDays(9).Ticks,2),
                //new(6),
                //new(6),
                //new(6),
                //new(4),
                //new(2),
                //new(3),
                //new(4),
                //new(3)
            };
            _observableValuesT = new ObservableCollection<ObservablePoint>
            {
                //new ObservablePoint(dates.Ticks,1),
            };
            _observableValuesP = new ObservableCollection<ObservablePoint>
            {
                //new ObservablePoint(dates.Ticks,1),
            };
            _observableValuesAx = new ObservableCollection<ObservablePoint>
            {
                //new ObservablePoint(dates.Ticks,4),
                //new(dates.AddSeconds(2).Ticks, 8),
            };
            _observableValuesAy = new ObservableCollection<ObservablePoint>
            {
                //new ObservablePoint(dates.Ticks,3),
                //new(dates.AddSeconds(2).Ticks, 7),
            };
            _observableValuesAz = new ObservableCollection<ObservablePoint>
            {
                //new ObservablePoint(dates.Ticks,2),
                //new(dates.AddSeconds(2).Ticks, 6),
            };
            _observableValuesGx = new ObservableCollection<ObservablePoint>
            {
                //new ObservablePoint(dates.Ticks,4),
                //new(dates.AddSeconds(2).Ticks, 8),
            };
            _observableValuesGy = new ObservableCollection<ObservablePoint>
            {
                //new ObservablePoint(dates.Ticks,3),
                //new(dates.AddSeconds(2).Ticks, 7),
            };
            _observableValuesGz = new ObservableCollection<ObservablePoint>
            {
                //new ObservablePoint(dates.Ticks,2),
                //new(dates.AddSeconds(2).Ticks, 6),
            };
            //_observableValuesGy = new ObservableCollection<ObservablePoint> { };
            //_observableValuesGz = new ObservableCollection<ObservablePoint> { };
            //_observableValuesAx = new ObservableCollection<ObservablePoint> { };
            //_observableValuesAy = new ObservableCollection<ObservablePoint> { };
            //_observableValuesAz = new ObservableCollection<ObservablePoint> { };
            var colors = new[]
            {
                new SKColor(236, 112, 99).WithAlpha(80),
                new SKColor(242, 215, 213).WithAlpha(80)
                // ...

                // you can add as many colors as you require to build the gradient
                // by default all the distance between each color is equal
                // use the colorPos parameter in the constructor of the RadialGradientPaint class
                // to specify the distance between each color
            };

            SeriesT = new ObservableCollection<ISeries>
            {
                //new LineSeries<ObservablePoint>
                //{
                //    Values = new ObservablePoint[]
                //    {
                //        new ObservablePoint(0, 4),
                //        new ObservablePoint(1, 3),
                //        new ObservablePoint(3, 8),
                //        new ObservablePoint(18, 6),
                //        new ObservablePoint(20, 12)
                //    }
                //}
                new LineSeries<ObservablePoint>
                {
                    
                    Name = "Temperatura",
                    Values = _observableValuesT,
                    Fill = new LinearGradientPaint(colors, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                    Stroke = new LinearGradientPaint(new[]{ new SKColor(255, 0, 0), new SKColor(250, 219, 216) }, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)) { StrokeThickness = 5 },
                    GeometryStroke = new LinearGradientPaint(new[]{ new SKColor(255, 0, 0), new SKColor(250, 219, 216) }, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)) { StrokeThickness = 5 },
                    //DataLabelsFormatter = val => val + "º",
                    GeometrySize = 5,
                }
            };
            SeriesH= new ObservableCollection<ISeries>
            {
                new LineSeries<ObservablePoint>
                {
                    Name="Humedad",
                    Values = _observableValuesH,
                    GeometrySize = 4,
                }
            };

            SeriesP = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservablePoint>
                {
                    Name="Altura",
                    Values = _observableValuesP,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint
                    {
                        Color = SKColors.LimeGreen,
                        StrokeCap = SKStrokeCap.Round,
                        StrokeThickness = 5,
                        //PathEffect = effect
                    },
                    Fill = new SolidColorPaint { Color = SKColors.LimeGreen.WithAlpha(40) },
                }
            };

            SeriesG = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservablePoint>
                {
                    Name="G_X",
                    Values = _observableValuesGx,
                    GeometrySize = 4,
                },
                new LineSeries<ObservablePoint>
                {
                    Name="G_Y",
                    Values = _observableValuesGy,
                    GeometrySize = 4,
                },
                new LineSeries<ObservablePoint>
                {
                    Name="G_Z",
                    Values = _observableValuesGz,
                    GeometrySize = 4,
                }

            };
            SeriesA = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservablePoint>
                {
                    Name="A_X",
                    Values = _observableValuesAx,
                    GeometrySize = 4,
                },
                new LineSeries<ObservablePoint>
                {
                    Name="A_Y",
                    Values = _observableValuesAy,
                    GeometrySize = 4,
                },
                new LineSeries<ObservablePoint>
                {
                    Name="A_Z",
                    Values = _observableValuesAz,
                    GeometrySize = 4,
                }
            };
            //cartesianChart1.Series = new ISeries[]
            //{
            //    new LineSeries<double>
            //    {
            //        Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
            //        Fill = new SolidColorPaint(SKColors.CornflowerBlue),
            //        GeometrySize = 0,
            //        LineSmoothness = 0

            //    }
            //};

            //XAxess = new Axis[]
            //{
            //    Name = "titulo",
            //    Labeler = (value) => value.ToString("C")
            //};

            cartesianChart1.Series = SeriesT;
            //cartesianChart1.XAxes = new Axis[]
            //{
            //    new Axis
            //    {
            //        //Name = "Titulo",
            //        NamePaint = new SolidColorPaint { Color = SKColors.Red },
            //        // Use the labels property for named or static labels 
            //        //Labels = new string[] { "Sergio", "Lando", "Lewis" },
            //        LabelsRotation = 90,
            //        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 },
            //        //SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
            //        //{
            //        //    StrokeThickness = 2,
            //        //    PathEffect = new DashEffect(new float[] { 3, 3 })
            //        //},
            //        //Labeler = (value) => new DateTime((long)value).ToString("HH:mm:ss"),
            //        Labeler = (value) => new DateTime((long)value).ToString("mm:ss"),

            //        //MinLimit = DateTime.Now.Subtract(TimeSpan.FromSeconds(1)).Ticks
            //        MinLimit = dates.Subtract(TimeSpan.FromSeconds(10)).Ticks
            //    }
            //};

            cartesianChart1.AnimationsSpeed = TimeSpan.FromMilliseconds(400);
            //cartesianChart1.TooltipPosition = LiveChartsCore.Measure.LegendPosition.Bottom;
            //cartesianChart1.LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom;
            //cartesianChart1.LegendFont = new System.Drawing.Font("Courier New", 10);
            //cartesianChart1.LegendTextColor = System.Drawing.Color.FromArgb(255, 50, 50, 50);
            //cartesianChart1.LegendBackColor = System.Drawing.Color.FromArgb(255, 250, 250, 250);

            //cartesianChart1.TooltipFont = new System.Drawing.Font("Courier New", 12);
            //cartesianChart1.TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Left;
            //cartesianChart1.TooltipFindingStrategy = LiveChartsCore.Measure.TooltipFindingStrategy.CompareAllTakeClosest;
            cartesianChart2.Series = SeriesG;
            cartesianChart2.LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom;

            cartesianChart3.Series = SeriesA;
            cartesianChart3.LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom;

            cartesianChart4.Series = SeriesH;
            cartesianChart5.Series = SeriesP;

            //cartesianChart1.DrawMarginFrame = new DrawMarginFrame
            //{
            //    Fill = new SolidColorPaint(SKColors.AliceBlue),
            //    Stroke = new SolidColorPaint(SKColors.Black, 1)
            //};

            //cartesianChart1.YAxes = new Axis[]
            //{
            //    new Axis
            //    {
            //        //Name = "Y Axis",
            //        NamePaint = new SolidColorPaint(SKColors.Red),

            //        LabelsPaint = new SolidColorPaint(SKColors.Green),
            //        TextSize = 15,

            //        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
            //        {
            //            StrokeThickness = 2,
            //            PathEffect = new DashEffect(new float[] { 3, 3 })
            //        }
            //    }
            //};
            //cartesianChart1.TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Bottom;
            //cartesianChart1.DrawMargin = new LiveChartsCore.Measure.Margin(45);
            // cartesianChart1.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X;
        }
        
        //public Axis[] XAxess { get; set; } =
        //{
        //new Axis
        //    {
        //        //Name = "Titulo",
        //        NamePaint = new SolidColorPaint { Color = SKColors.Red },
        //        // Use the labels property for named or static labels 
        //        //Labels = new string[] { "Sergio", "Lando", "Lewis" },
        //        LabelsRotation = 90,
        //        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 },
        //        //SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
        //        //{
        //        //    StrokeThickness = 2,
        //        //    PathEffect = new DashEffect(new float[] { 3, 3 })
        //        //},
        //        //Labeler = (value) => new TimeSpan((long)value).ToString("HH:mm:ss"),

        //        MinLimit = dateti Subtract(TimeSpan.FromSeconds(1)).Ticks
        //    }
        //};


        int i = 1;



        private void button2_Click(object sender, EventArgs e)
        {
            //SeriesT.Add(
            //new LineSeries<int>
            //{
            //    Values = new List<int>
            //    {
            //        _random.Next(0, 10),
            //        _random.Next(0, 10),
            //        _random.Next(0, 10)
            //    }
            //});

            //SeriesT = new ObservableCollection<ISeries>
            //{
            
            
            SeriesT[0].IsVisible = !SeriesT[0].IsVisible;


            //SeriesT.Add(
            //new LineSeries<ObservableValue>
            //{
            //    Name = "hola",
            //    Values = _observableValuesT,
            //    Fill = null
            //});


            //};
        }
        DateTime date = new DateTime(2000, 01, 01);


        private void button2_Click_1(object sender, EventArgs e)
        {


                cOMPortToolStripMenuItem.DropDownItems.Clear();
                ToolStripMenuItem item2 = new ToolStripMenuItem();
                item2.Name = "Serial ports message";
                //item.Tag = "specialDataHere";
                item2.Text = "Serial ports";
                //item.Click += new EventHandler(MenuItemClickHandler);
                //item.CheckOnClick = true;
                item2.Enabled = false;
                cOMPortToolStripMenuItem.DropDownItems.Add(item2);
            foreach (string s in SerialPort.GetPortNames())
                {
                    //textBox1.Text += String.Format("{0}\r\n", s);
                    //comboBox1.Items.Add(String.Format("{0}\r\n", s));


                    ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Name = String.Format("{0}", s);
                    //item.Tag = "specialDataHere";
                    item.Text = String.Format("{0}", s);
                    item.Click += new EventHandler(MenuItemClickHandler);
                    item.CheckOnClick = true;

                    if (serialPort1.PortName == s && serialPort1.IsOpen)
                    {
                        item.CheckState = CheckState.Checked;
                    }

                    cOMPortToolStripMenuItem.DropDownItems.Add(item);

                }
            


        }
        private void MenuItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            // Take some action based on the data in clickedItem
        }



        //Se ejecuta una vez presione el raton. Luego de finalizar el metodo, se ejecuta el check/unckeck segun el estado
        //al que haya dejado este metodo
        //antes: unckecked
        //durante(metodo) : unckeck
        //despues: ckeck (porque estaba unckeck)

        //antes: ckecked
        //durante(metodo) : unckeck
        //despues: ckeck (porque estaba unckeck)
        private void cOMPortToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string itemText = e.ClickedItem.Text;

            ToolStripMenuItem items = sender as ToolStripMenuItem;//(ToolStripMenuItem)sender;
            // only clicked will be checked
            var isChecked = CheckSelected(itemText, items);

            ToolStripMenuItem item = e.ClickedItem as ToolStripMenuItem;
            // if was not checked, open Serial. Otherwise, port busy Message
            if (!isChecked)
            {
                serialPort1.Close();
                serialPort1.PortName = itemText;
                serialPort1.BaudRate = _baudrate;
                try
                {
                    serialPort1.Open();
                    baudRateToolStripMenuItem.Enabled = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("El puerto está ocupado o no disponible", "Error");
                    item.Checked = true;
                }
            }
        }


        private bool CheckSelected(string itemSelected, ToolStripMenuItem items)
        {
            bool isChecked = true;
            foreach (var item in items.DropDownItems.Cast<ToolStripMenuItem>())
            {

                if (item.Name == itemSelected && item.Checked == false)
                {
                    isChecked = false;
                }
                item.Checked = false;
            }
            return isChecked;
        }

        private void button3_Click(object sender, EventArgs e)
        {



            if (serialPort1.IsOpen == true)
            {
               // serialPort1.
                serialPort1.Close();
            }
            label1.Text = "";
        }

        private void baudRateToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string itemText = e.ClickedItem.Text;
            _baudrate = int.Parse(itemText);
            ToolStripMenuItem items = sender as ToolStripMenuItem;
            CheckSelected(itemText, items);
        }


        TimeSpan times = new System.TimeSpan(0, 45, 0);
        private void Form1_Load(object sender, EventArgs e)
        {
            //serialPort1.DiscardInBuffer();
            //Control.CheckForIllegalCrossThreadCalls = false;
            label2.Text = "";
            label2.Text = dates.ToString("mm:ss");
            toolStripStatusLabel1.Text = "";
            iconPictureBox3.IconChar = FontAwesome.Sharp.IconChar.Droplet;
            //stopwatch = new Stopwatch();
            //stopwatch. = 638019790570000000;
            //stopwatch.Start();
            
        }
        int count = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {

            //label1.Text = count.ToString();
            count++;

            cOMPortToolStripMenuItem.DropDownItems.Clear();
            ToolStripMenuItem item2 = new ToolStripMenuItem();
            item2.Name = "Serial ports message";
            //item.Tag = "specialDataHere";
            item2.Text = "Serial ports";
            //item.Click += new EventHandler(MenuItemClickHandler);
            //item.CheckOnClick = true;
            item2.Enabled = false;
            cOMPortToolStripMenuItem.DropDownItems.Add(item2);
            foreach (string s in SerialPort.GetPortNames())
            {
                //textBox1.Text += String.Format("{0}\r\n", s);


                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = String.Format("{0}", s);
                //item.Tag = "specialDataHere";
                item.Text = String.Format("{0}", s);
                item.Click += new EventHandler(MenuItemClickHandler);
                item.CheckOnClick = true;

                if (serialPort1.PortName == s && serialPort1.IsOpen)
                {
                    iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.PlugCircleCheck;
                    iconPictureBox1.ForeColor = Color.Green;
                    item.CheckState = CheckState.Checked;
                    toolStripStatusLabel1.Text = "Conectado";
                    labelSerial.Text = "Conectado.";
                    //label2.Text = "opened";
                }
                else
                {
                    iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.PlugCircleXmark;
                    iconPictureBox1.ForeColor = Color.Red;
                    baudRateToolStripMenuItem.Enabled = true;
                    toolStripStatusLabel1.Text = "No conectado";
                    labelSerial.Text = "No Conectado.";
                    //label2.Text = "closed";
                }

                cOMPortToolStripMenuItem.DropDownItems.Add(item);

            }
            if (serialPort1.IsOpen)
            {
                iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.PlugCircleCheck;
                iconPictureBox1.ForeColor = Color.Green;
                toolStripStatusLabel1.Text = "Conectado";
                labelSerial.Text = "Conectado.";
            }
            else
            {
                iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.PlugCircleXmark;
                iconPictureBox1.ForeColor = Color.Red;
                baudRateToolStripMenuItem.Enabled = true;
                toolStripStatusLabel1.Text = "No conectado";
                labelSerial.Text = "No Conectado.";
            }

        }

        private void serialPortToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            //label1.Text = "abierto";
        }

        private void serialPortToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            //label1.Text = "cerrado";
        }

        int aux2 = 0;
        private async void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            await Task.Delay(20);
            this.Invoke((MethodInvoker)delegate
            {
                if (aux2 == 0)
                {
                    //timer2.Enabled = true;
                    aux2 = 1;
                    //label2.Text = "timer2";
                    timer2.Enabled = true;
                }
                //Thread.Sleep(20);

                int datalen = serialPort1.BytesToRead;
                if (datalen == 31)
                {
                    byte[] buffer = new byte[datalen];
                    serialPort1.Read(buffer, 0, datalen);
                    Datos dr = ByteArrayToStructure<Datos>(buffer);

                    //byte[] bytess = ConvertToBytes(dr);
                    //hexString = BitConverter.ToString(bytess);
                    //label1.Text = dr.alt.ToString();
                    //printtext(hexString);
                    //textBox1.Text = hexString;


                    addPointsToCharts(((double)dr.temperatura) / 10, dr.altura, dr.humedad, dr.acelX, dr.acelY, dr.acelZ, dr.gyX, dr.gyY, dr.gyZ);
                    //labelT.Text = (dr.temperatura / 10).ToString();
                    //labelH.Text = dr.humedad.ToString();
                    //labelP.Text = dr.altura.ToString();
                }
                serialPort1.DiscardInBuffer();
            });


            //string indata = serialPort1.ReadExisting();
        }


        private void iconPictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (serialPort1.IsOpen)
                toolTip1.SetToolTip(iconPictureBox1, "Conectado");
            else
                toolTip1.SetToolTip(iconPictureBox1, "Desconectado");
            iconPictureBox3.IconChar = FontAwesome.Sharp.IconChar.Droplet;
        }




        int k=0;
        DateTime dates = new DateTime(2021, 1, 1, 10, 0, 0);
        public void timer2_Tick(object sender, EventArgs e)
        {
            
            dates = dates.AddSeconds(1);
            //label2.Text = stopwatch.ElapsedTicks.ToString();
            label2.Text = dates.ToString("mm:ss");
            //label2.Text = String.Format("{0:hh\\:mm\\:ss\\:fff}", stopwatch.ElapsedTicks);
            //textBox1.Text += DateTime.MaxValue.Ticks.ToString();
            //textBox1.Text += "\r\n";
            //textBox1.Text += String.Format("{0}\r\n", stopwatch.ElapsedTicks);
            label1.Text=k++.ToString();
        }

        private void checkBoxGx_CheckedChanged(object sender, EventArgs e)
        {
            SeriesG[0].IsVisible = !SeriesG[0].IsVisible;
        }

        private void checkBoxGy_CheckedChanged(object sender, EventArgs e)
        {
            SeriesG[1].IsVisible = !SeriesG[1].IsVisible;
        }

        private void checkBoxGz_CheckedChanged(object sender, EventArgs e)
        {
            SeriesG[2].IsVisible = !SeriesG[2].IsVisible;
        }

        private void checkBoxAx_CheckedChanged(object sender, EventArgs e)
        {
            SeriesA[0].IsVisible = !SeriesA[0].IsVisible;
        }

        private void checkBoxAy_CheckedChanged(object sender, EventArgs e)
        {
            SeriesA[1].IsVisible = !SeriesA[1].IsVisible;
        }

        private void checkBoxAz_CheckedChanged(object sender, EventArgs e)
        {
            SeriesA[2].IsVisible = !SeriesA[2].IsVisible;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Está seguro que desea salir?", "Atención", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                System.Environment.Exit(1);
                //Application.Exit();
            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Programa desarrollado por Alvaro Loa");
        }

        private void cómoUsarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form3 settingsForm = new Form3();

            //// Show the settings form
            //settingsForm.Show();
            using (Form3 f2 = new Form3())
            {
                f2.ShowDialog(this);
            }
        }



        //private void iconButton1_Click(object sender, EventArgs e)
        //{
        //    iconButton1.IconChar = FontAwesome.Sharp.IconChar.PlugCircleCheck;
        //}

        private void addPointsToCharts (
            double temperatura,
            double altura,
            double humedad,
            double acelX,
            double acelY,
            double acelZ,
            double gyX,
            double gyY,
            double gyZ)
        {
            //timer2.Start();
            double altura2 = (double)((int)(altura * 10)) / 10;
            labelT.Text = temperatura.ToString();
            _observableValuesT.Add(new ObservablePoint(dates.Ticks, temperatura));
            labelH.Text = humedad.ToString();
            _observableValuesH.Add(new ObservablePoint(dates.Ticks, humedad));
            labelP.Text = altura.ToString("0.0");
            _observableValuesP.Add(new ObservablePoint(dates.Ticks, altura2));
            gyY = gyY - 0.02;

            _observableValuesGx.Add(new ObservablePoint(dates.Ticks, gyX));
            _observableValuesGy.Add(new ObservablePoint(dates.Ticks, gyY));
            _observableValuesGz.Add(new ObservablePoint(dates.Ticks, gyZ));
            _observableValuesAx.Add(new ObservablePoint(dates.Ticks, acelX));
            _observableValuesAy.Add(new ObservablePoint(dates.Ticks, acelY));
            _observableValuesAz.Add(new ObservablePoint(dates.Ticks, acelZ));

            textBoxTemp.Text = temperatura.ToString("0.0");
            textBoxHumedad.Text = humedad.ToString("0");
            textBoxAltura.Text = altura.ToString("0.0");
            textBoxGx.Text = gyX.ToString("0.00");
            textBoxGy.Text = gyY.ToString("0.00");
            textBoxGz.Text = gyZ.ToString("0.00");
            textBoxAx.Text = acelX.ToString("0.00");
            textBoxAy.Text = acelY.ToString("0.00");
            textBoxAz.Text = acelZ.ToString("0.00");

            cartesianChart1.XAxes = new Axis[]
            {
                new Axis
                {
                    //Name = "Y Axis",
                    Labeler = (value) => new DateTime((long) value).ToString("mm:ss"),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 },
                    //MinLimit=DateTime.Now.Subtract(TimeSpan.FromSeconds(10)).Ticks,
                    MinLimit = dates.Subtract(TimeSpan.FromSeconds(timesWindow)).Ticks,
                    UnitWidth = TimeSpan.FromSeconds(2).Ticks
                    }
                };

            cartesianChart1.YAxes = new Axis[]
            {
                new Axis
                    {
                        //Name = "Y Axis",
                        NamePaint = new SolidColorPaint(SKColors.Red),
                        TextSize = 15,
                        MinLimit = 0,
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray){StrokeThickness = 2,PathEffect = new DashEffect(new float[] { 3, 3 })}
                    }
            };

            cartesianChart4.XAxes = new Axis[]
            {
                new Axis
                {
                    //Name = "Y Axis",
                    Labeler = (value) => new DateTime((long)value).ToString("mm:ss"),
                    //SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 },
                    MinLimit = dates.Subtract(TimeSpan.FromSeconds(timesWindow)).Ticks,
                    UnitWidth = TimeSpan.FromSeconds(2).Ticks
                }
            };

            cartesianChart4.YAxes = new Axis[]
            {
                new Axis
                    {
                        //Name = "Y Axis",
                        //NamePaint = new SolidColorPaint(SKColors.Red),
                        //TextSize = 15,
                        MaxLimit = 100,
                        MinLimit = 0,
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray){StrokeThickness = 2,PathEffect = new DashEffect(new float[] { 3, 3 })}
                    }
            };

            cartesianChart5.XAxes = new Axis[]
            {
                new Axis
                {
                    //Name = "Y Axis",
                    Labeler = (value) => new DateTime((long)value).ToString("mm:ss"),
                    //SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 },
                    MinLimit = dates.Subtract(TimeSpan.FromSeconds(timesWindow)).Ticks,
                    UnitWidth = TimeSpan.FromSeconds(2).Ticks
                }
            };

            cartesianChart5.YAxes = new Axis[]
            {
                new Axis
                    {
                        //Name = "Y Axis",
                        //NamePaint = new SolidColorPaint(SKColors.Red),
                        //TextSize = 15,
                        MinLimit = 0,
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray){StrokeThickness = 2,PathEffect = new DashEffect(new float[] { 3, 3 })}
                    }
            };

            cartesianChart2.XAxes = new Axis[]
            {
                new Axis
                {
                    //Name = "Y Axis",
                    Labeler = (value) => new DateTime((long)value).ToString("mm:ss"),
                    //SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 },
                    MinLimit = dates.Subtract(TimeSpan.FromSeconds(timesWindow)).Ticks,
                    UnitWidth = TimeSpan.FromSeconds(2).Ticks
                }
            };

            cartesianChart2.YAxes = new Axis[]
            {
                new Axis
                    {
                        //Name = "Y Axis",
                        //NamePaint = new SolidColorPaint(SKColors.Red),
                        //TextSize = 15,
                        MaxLimit = 5,
                        MinLimit = -5,
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray){StrokeThickness = 2,PathEffect = new DashEffect(new float[] { 3, 3 })}
                    }
            };

            cartesianChart3.XAxes = new Axis[]
            {
                new Axis
                {
                    //Name = "Y Axis",
                    Labeler = (value) => new DateTime((long)value).ToString("mm:ss"),
                    //SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 },
                    MinLimit=dates.Subtract(TimeSpan.FromSeconds(timesWindow)).Ticks,
                    UnitWidth = TimeSpan.FromSeconds(2).Ticks
                }
            };

            cartesianChart3.YAxes = new Axis[]
            {
                new Axis
                    {
                        //Name = "Y Axis",
                        //NamePaint = new SolidColorPaint(SKColors.Red),
                        //TextSize = 15,
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray){StrokeThickness = 2,PathEffect = new DashEffect(new float[] { 3, 3 })}
                    }
            };
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Está seguro que desea salir?", "Atención", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //timer2.Enabled = true;
            while (true)
            {

                double randomValue = _random.Next(1, 25);

                addPointsToCharts(_random.Next(1, 25), _random.Next(3400, 3600), _random.Next(1, 100), _random.Next(-10, 10), randomValue = _random.Next(-10, 10), randomValue = _random.Next(-10, 10), randomValue = _random.Next(-10, 10), randomValue = _random.Next(-10, 10), randomValue = _random.Next(-10, 10));
                //_observableValuesH.Add(new ObservablePoint(DateTime.Now.Ticks, randomValue));
                i++;




                await Task.Delay(2000);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
            }
            timer2.Enabled = false;
            aux2 = 0;
            //timer2.Stop();
            //SeriesT[1].IsVisible = !SeriesT[1].IsVisible;
            //SeriesA[0].IsVisible = !SeriesA[0].IsVisible;
            //var chartControl = cartesianChart1;
            //var skChart = new SKCartesianChart(chartControl) { Width = 900, Height = 600, };
            //skChart.SaveImage("image.png", SKEncodedImageFormat.Jpeg, 40);
        }
    }
}
