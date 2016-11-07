using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;
using ZedGraph;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace SimpleCameraReader
{
  public partial class FMainForm : Form
  {
    //string Path = @"C:\Users\Leica\Documents\Visual Studio 2012\Projects\SimpleCameraSaveFiles\Exp\"; //Путь куда сохраняем файлы
    string Path = @"C:\Users\Leica\Documents\Visual Studio 2012\Projects\SimpleCameraSaveFiles\Debug\"; //Путь куда сохраняем файлы
    //string Path = @"\\206DEVSTATION\Users\Sima\Desktop\Hren From 209\"; //Путь куда сохраняем файлы

    CCameraAdapter camAdapter;// класс камеры
    Image<Bgr, Byte> Input; 
    Task SaveToFileTask; // async
    Queue<Tuple<Bitmap, long, long>> QIMG; // очередь

    int Queue_CutOff_value = 5720; // Максимальная длина очереди. При превышении - новые элементы игнорируются
    
    bool doWork = false;
    long scanned_img_number = 0;
    long saved_img_number = 0;

    int exposure_value;
    float gain_value;
    Stopwatch timer;

    object queueLock = new object();
    
    List<string> ImageFormats;
    List<string> PixelFormats;
    
    static long skipped_img_counter = 0;
    bool isStarted = false;
 

    public FMainForm()
    {
      InitializeComponent();
    }

    private void FMainForm_Load(object sender, EventArgs e)
    {
      camAdapter = new CCameraAdapter();
      QIMG = new Queue<Tuple<Bitmap, long, long>>();

      ImageFormats = new List<string>();
      PixelFormats = new List<string>();
    }


    // Функция для настройки полей формы и прочего. Принимает Bitmap только из-за проблематичного класса камеры - нельзя просто взять и узнать параметры камеры
    // Поэтому сначала один раз вызывается через event эта фунция, считываем параметры камеры, а потом отписываемся и подписываем сам обработчик изображений
    private void PrepareForWork(Bitmap img)
    {
      doWork = true;
      
    

      // доступные форматы изображений
      ImageFormats = camAdapter.GetAvailableImageFormats();
      foreach (var Val in ImageFormats) if (Combo_ImageFormats.InvokeRequired) Combo_ImageFormats.Invoke(new Action(() => Combo_ImageFormats.Items.Add(Val)));

      //доступные форматы пикселей
      PixelFormats = camAdapter.GetAvailablePixelFormats();
      foreach (var Val in PixelFormats) if (Combo_PixelFormats.InvokeRequired) Combo_PixelFormats.Invoke(new Action(() => Combo_PixelFormats.Items.Add(Val)));

      //выбираем дефолтные
      if (Combo_ImageFormats.InvokeRequired) Combo_ImageFormats.Invoke(new Action(() => Combo_ImageFormats.SelectedIndex = Combo_ImageFormats.Items.IndexOf(camAdapter.GetCurrentImageFormat())));
      if (Combo_PixelFormats.InvokeRequired) Combo_PixelFormats.Invoke(new Action(() => Combo_PixelFormats.SelectedIndex = Combo_PixelFormats.Items.IndexOf(camAdapter.GetCurrentPixelFormat())));

      //экспозиция и gain
      exposure_value = camAdapter.GetExposure();
      gain_value = camAdapter.GetGain();

      if (TB_Exposition.InvokeRequired) TB_Exposition.Invoke(new Action(() => TB_Exposition.Text = exposure_value.ToString()));
      if (TB_Gain.InvokeRequired) TB_Gain.Invoke(new Action(() => TB_Gain.Text = gain_value.ToString()));

      //--------------
      timer = new Stopwatch(); // таймер запустится позже в RecieveImage
      
      camAdapter.OnImageReady -= PrepareForWork; //отписываемся от текущего задания
      
      DateTime dt = DateTime.UtcNow;
      StartSaving(Path, dt); // запускаем асинхронное сохранение

      camAdapter.OnImageReady += RecieveImage; // и подписываемся на основное
    }


    //callback-функция, получающая изображение с камеры от camAdapter
    // изображение сохраняется в очередь и показывается на форме
    private void RecieveImage(Bitmap img)
    {
      if (!timer.IsRunning) 
        timer.Restart();

      long time_passed = timer.ElapsedMilliseconds;

      if (doWork)
      {
        try
        {
          // Работа с очередью
          lock (queueLock)
          {
              // сохраянем изображение, его номер и время с прошлого изображения
              QIMG.Enqueue(new Tuple<Bitmap, long, long>(img, scanned_img_number, time_passed));
          }

          scanned_img_number++;

          while (QIMG.Count > Queue_CutOff_value)
          {
            //QIMG.Dequeue(); 
            skipped_img_counter++;
          }


          //Работа с формой
          Input = new Image<Bgr, byte>((Bitmap)img.Clone());
          //Input = Input.Resize(4, Inter.Cubic); // раскомментировать если хотим видеть большое изображение
          Bitmap toShow = Input.ToBitmap();

          PictureBox.Invoke(new Action<Image>((s) => PictureBox.Image = s), toShow);
          label_scanned_images.Invoke(new Action<string>((s) => label_scanned_images.Text = s), scanned_img_number.ToString());

        }
        catch (Exception e)
        {
          //MessageBox.Show("Something awful happened in ReciveImage> " + e.Message);
        }
      }
      else throw new Exception("VideoWriter not good");

    }

    // асинхронное сохранение изображений 
    private void StartSaving(string path, DateTime dt)
    {
      //асинхронность)))
      SaveToFileTask = new Task(new Action(() =>
      {
        
        Image<Gray, Byte> timestamp = new Image<Gray, byte>(1, 1, new Gray(0));
        string sss = dt.TimeOfDay.ToString() + DateTime.UtcNow.Millisecond.ToString();
        sss = sss.Replace(':', '_');
        timestamp.Save(@"C:\Users\Leica\Documents\Visual Studio 2012\Projects\SimpleCameraSaveFiles\TimeStamps\" + sss + ".png");
        timer.Stop();


        Image<Bgr, Byte> tmp;
        int i = 0; // порядковый номер изображения
        Tuple<Bitmap, long, long> recieved;// битмап, номер, время в тиках

        int N = 0;

        while (true)
        {
          if (QIMG.Count > 0)
          {
            try
            {
              lock (queueLock)
              {
                recieved = QIMG.Dequeue(); //считываем из очереди
                N = QIMG.Count;
                saved_img_number++;
                //в диагностических целях
                //int n = QIMG.Count();
                //label3.BeginInvoke(new Action(() => { label3.Text = "Длина очереди: " + N; }));
              }
              
              tmp = new Image<Bgr, byte>(recieved.Item1);
              tmp.Convert<Gray, Byte>().Save(path + recieved.Item2.ToString() + "_" + (recieved.Item3).ToString() + ".Png");
              
              label_saved_images.BeginInvoke(new Action(() => label_saved_images.Text = recieved.Item2.ToString()));
              label_queue_images.BeginInvoke(new Action(() => label_queue_images.Text = N.ToString()));
            }
            catch (Exception e)
            {
              // MessageBox.Show(e.Message);
            }

          }
          //Thread.Sleep(0);
        }
      }
     ));
      SaveToFileTask.Start();
    }

    #region Buttons

    private void StartButton_Click(object sender, EventArgs e)
    {
      if (!isStarted)
        camAdapter.Connect();
      isStarted = true;

      //TB_Exposition.Text = camAdapter.GetExposure().ToString();
      camAdapter.OnImageReady += PrepareForWork;
    }

    private void PauseBtn_Click(object sender, EventArgs e)
    {
      camAdapter.OnImageReady -= RecieveImage;
    }

    private void BTN_SetExposition_Click(object sender, EventArgs e)
    {
      int val = Int32.Parse(TB_Exposition.Text);
      camAdapter.SetExposure(val);
    }

    private void BTN_SetGain_Click(object sender, EventArgs e)
    {
      float val = float.Parse(TB_Gain.Text);
      camAdapter.SetGain(val);
    }
    
    private void BTN_Kill_Click(object sender, EventArgs e)
    {
      camAdapter.OnImageReady -= RecieveImage;
      doWork = false;
      MessageBox.Show("Skipped images: " + skipped_img_counter.ToString());
      camAdapter.Kill();
    }

    #endregion

    #region ComboBoxes
    private void Combo_ImageFormats_SelectedIndexChanged(object sender, EventArgs e)
    {
      camAdapter.SetImageFormat(Combo_ImageFormats.SelectedItem.ToString());

      PixelFormats = camAdapter.GetAvailablePixelFormats();

      Combo_PixelFormats.Items.Clear();
      foreach (var Val in PixelFormats) if (Combo_PixelFormats.InvokeRequired) Combo_PixelFormats.Invoke(new Action(() => Combo_PixelFormats.Items.Add(Val)));
        else Combo_PixelFormats.Items.Add(Val);
      if (Combo_PixelFormats.InvokeRequired) Combo_PixelFormats.Invoke(new Action(() => Combo_PixelFormats.SelectedIndex = Combo_PixelFormats.Items.IndexOf(camAdapter.GetCurrentPixelFormat())));
      else camAdapter.SetPixelFormat(Combo_PixelFormats.Items[0].ToString());
      //string d = Combo_PixelFormats.Items[0].ToString();
    }

    private void Combo_PixelFormats_SelectedIndexChanged(object sender, EventArgs e)
    {
      camAdapter.SetPixelFormat(Combo_PixelFormats.SelectedItem.ToString());
    }

    #endregion

    private void FMainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      BTN_Kill.PerformClick();
    }

  
   
    
  }
}

