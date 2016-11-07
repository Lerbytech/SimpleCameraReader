using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using BGAPI;
using draw = System.Drawing;
using System.Diagnostics;

namespace SimpleCameraReader
{
  public class CCameraAdapter : IDisposable
  {
    #region BUGAPI variables
    int system_count = 0;
    int image_count = 0;
    int currSystem = 0;
    int inputVal = 0;
    int i = 0;
    int res = BGAPI.Result.FAIL;

    BGAPI.System[] ppSystem;
    BGAPI.Camera pCamera;
    BGAPI.Image[] ppImage;
    BGAPI.BGAPIX_TypeListINT listint;
    BGAPI.BGAPI_FeatureState state;
    BGAPI.BGAPIX_TypeINT intNumberofbuffer;
    BGAPI.BGAPI_NOTIFY_CALLBACK imgcallback;

    BGAPIX_CameraPixelFormat CameraPixelFormat = new BGAPIX_CameraPixelFormat();
    BGAPI_FeatureState FeatureState = new BGAPI_FeatureState();
    BGAPIX_CameraInfo CameraInfo = new BGAPIX_CameraInfo();
    BGAPIX_CameraImageFormat CameraImageFormat = new BGAPIX_CameraImageFormat();
    BGAPIX_TypeListINT TypeListINT = new BGAPIX_TypeListINT();
    BGAPIX_TypeRangeINT TypeRangeINT = new BGAPIX_TypeRangeINT();
    BGAPIX_TypeRangeFLOAT TypeRangeFLOAT = new BGAPIX_TypeRangeFLOAT();
    BGAPIX_CameraPixelFormat pformat = new BGAPIX_CameraPixelFormat();


    int formatindex = 7;
    int pixelformatindex = 0;

    int exposurevalue = 30000;
    int MaxExposure = 60000000;
    int MinExposure = 4;

    int gainvalue = 1;
    int MinGainValue = 0;
    int MaxGainValue = 9;
    #endregion
    
    Task CameraConnectionTask;
    public delegate void imageReadyDelegate(draw.Bitmap image);
    public event imageReadyDelegate OnImageReady;

    public CCameraAdapter()
    {
    }
    /*
    private int CallbackFunction(object callBackOwner, ref BGAPI.Image image)
    {

      /*
      int bgapierror = 0;
      System.Drawing.Imaging.ColorPalette palette;
      System.Drawing.Imaging.PixelFormat format = new System.Drawing.Imaging.PixelFormat(); // UNDEFINED
      //TransformationType tmpConvertImage = mConvertImage; // AFTERCAPTURE

      int w = 0;
      int h = 0;
      int len = 0;
      IntPtr buffer = new IntPtr(0);
      int pixelformat = 0; // 173....
      int stride = 0;
      image.getPixelFormat(ref pixelformat);
      image.getSize(ref w, ref h); //172 130
      image.getImageLength(ref len); //22360 
      //image.getState(ref imagestatus); // GOOD
      Size CurrentImageSize = new Size(w, h);
      draw.Bitmap bitmap = new Bitmap(w, h, );
      bitmap = new Bitmap
      bgapierror = image.get(ref buffer);
      if (pixelformat == BGAPI.Pixtype.MONO8)
      {
        stride = w;
        format = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
        Rectangle rect = new Rectangle(0, 0, w, h);
        System.Drawing.Imaging.BitmapData bmpData =
                 bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                 bitmap.PixelFormat);
        unsafe
        {
          byte* bitmapBuffer = (byte*)bmpData.Scan0;
          byte* byteBuffer = (byte*)buffer.ToPointer();
          // Kopieren der Bytes innerhalb des Buffers
          for (int i = 0; i < len; i++)
          {
            bitmapBuffer[i] = byteBuffer[i];
          }
        }
        bitmap.UnlockBits(bmpData);


        palette = bitmap.Palette;
        int nColors = 256;
        for (uint i = 0; i < nColors; i++)
        {
          uint Alpha = 0xFF;                      // Colors are opaque.
          uint Intensity = (uint)(i * 0xFF / (nColors - 1));    // Even distribution. 
          palette.Entries[i] = Color.FromArgb(
              (int)Alpha,
              (int)Intensity,
              (int)Intensity,
              (int)Intensity);
        }
        bitmap.Palette = palette;
      }
        return 1;

      */

    private int CallbackFunction(object callBackOwner, ref BGAPI.Image image)
    {
      draw.Bitmap bitmap;
      
      System.Drawing.Imaging.PixelFormat format = new System.Drawing.Imaging.PixelFormat();

      format = System.Drawing.Imaging.PixelFormat.Format8bppIndexed; // Format24bppRgb;
      int len = 0;
      int w = 0;
      int h = 0;

      IntPtr imagebuffer = new IntPtr(0);
      int pixelformat = 0;
      image.getPixelFormat(ref pixelformat);

      image.get(ref imagebuffer);
      image.getSize(ref w, ref h);
      image.getImageLength(ref len);
      image.setDestinationPixelFormat(BGAPI.Pixtype.MONO8);///%% RGB8_Packed
      image.getTransformBufferLength(ref len);
      draw.Rectangle rect = new draw.Rectangle(0, 0, w, h);

      //int stride = w * 3; // old version
      
      int stride = w;
      bitmap = new draw.Bitmap(w, h, stride, format, imagebuffer);
      System.Drawing.Imaging.BitmapData bmpData =
                  bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                  bitmap.PixelFormat);
      unsafe
      {
        byte* bitmapBuffer = (byte*)bmpData.Scan0;
        byte* byteBuffer = (byte*)imagebuffer.ToPointer();
        // Kopieren der Bytes innerhalb des Buffers
        for (int i = 0; i < len; i++)
        {
          bitmapBuffer[i] = byteBuffer[i];
        }
      }
      bitmap.UnlockBits(bmpData);
      System.Drawing.Imaging.ColorPalette palette;
      palette = bitmap.Palette;
      int nColors = 256;
      for (uint i = 0; i < nColors; i++)
      {
        uint Alpha = 0xFF;                      // Colors are opaque.
        uint Intensity = (uint)(i * 0xFF / (nColors - 1));    // Even distribution. 
        palette.Entries[i] = Color.FromArgb(
            (int)Alpha,
            (int)Intensity,
            (int)Intensity,
            (int)Intensity);
         
      }
      
      bitmap.Palette = palette;

      if (res != BGAPI.Result.OK)
        Console.WriteLine("bgapi error");

      //-----------------------------------------
      if (OnImageReady != null) OnImageReady(bitmap);
      //after you are ready with this image, return it to the camera for the next image
      ((BGAPI.Camera)callBackOwner).setImage(ref image);
      return res;

    }


    #region IO Functions
    public void SetExposure(int val)
    {
      if (val <= MinExposure) pCamera.setExposure(MinExposure);
      else if (val >= MaxExposure) pCamera.setExposure(MaxExposure);
           else pCamera.setExposure(val);
    }

    public int GetExposure()
    {      
      int res = pCamera.getExposure(ref FeatureState, ref TypeRangeINT);
      return TypeRangeINT.current;
    }

    public void SetGain(float val)
    {
      if (val <= MinGainValue) pCamera.setGain(MinGainValue);
      else if (val >= MaxGainValue) pCamera.setGain(MaxGainValue);
      else pCamera.setGain(val);
    }

    public float GetGain()
    {
      int res = pCamera.getGain(ref FeatureState, ref TypeRangeFLOAT);
      return TypeRangeFLOAT.current;
    }

    public string GetCurrentImageFormat()
    {
      res = pCamera.getImageFormat(ref FeatureState, ref TypeListINT);
      res = pCamera.getImageFormatDescription(TypeListINT.current, ref CameraImageFormat);
      return CameraImageFormat.sName;
    }

    public string GetCurrentPixelFormat()
    {
      res = pCamera.getPixelFormat(formatindex, ref FeatureState, ref TypeListINT);
      res = pCamera.getPixelFormatDescription(formatindex, TypeListINT.array[ TypeListINT.current], ref CameraPixelFormat);
      return CameraPixelFormat.sName;
    }
      
    public List<string> GetAvailableImageFormats()
    {
      /* 0 || Fullframe HQ: 12 1392 x 1040 BayerRG
       * 1 || Fullframe: 08 1392 x 1040 BayerRG
       * 3 || Binning 2x2: 08 696 x 520 Mono
       * 4 || Binning 4x4 HQ: 12 348 x 260 Mono
       * 6 || Binning 8x8 HQ: 12 172 x 130 Mono
       * 7 || Binning 8x8: 08 172 x 130 Mono
       * WTF?!!!
       * где остальные форматы?!
       */

      List<string> formats = new List<string>();
      res = pCamera.getImageFormat(ref FeatureState, ref TypeListINT);
      ReturnError(res, "getImageFormat in  GetAvailableImageFormats ");

      for (int i = 0; i < TypeListINT.array.Length; i++)
      {
        res = pCamera.getImageFormatDescription(TypeListINT.array[i], ref CameraImageFormat);
        //ReturnError(res, "getImageFormatDescription in  GetAvailableImageFormats ");
        if (res == 1) formats.Add(CameraImageFormat.sName);
      }
      formats = formats.Distinct().ToList();
      return formats;
    }

    public List<string> GetAvailablePixelFormats()
    {
      List<string> formats = new List<string>();
      res = pCamera.getPixelFormat(formatindex, ref FeatureState, ref TypeListINT);

      for (int i = 0; i < TypeListINT.array.Length; i++)
      {
        res = pCamera.getPixelFormatDescription(formatindex, TypeListINT.array[i], ref CameraPixelFormat);
        //ReturnError(res, "getPixelFormatDescription in  GetAvailableImageFormats ");
        if (res == 1) formats.Add(CameraPixelFormat.sName);
      }

      formats = formats.Distinct().ToList();
      return formats;
    }

    public void SetImageFormat(string new_format)
    {
      res = pCamera.getImageFormat(ref FeatureState, ref TypeListINT);
      for (int i = 0; i < TypeListINT.array.Length; i++)
      {
        res = pCamera.getImageFormatDescription(TypeListINT.array[i], ref CameraImageFormat);
        if (String.Compare(new_format, CameraImageFormat.sName) == 0)
        {
          formatindex = TypeListINT.array[i];
          res = pCamera.setImageFormat(TypeListINT.array[i]);
          break;
        }
      }
    }

    public void SetPixelFormat(string new_format)
    {
      res = pCamera.getPixelFormat(formatindex, ref FeatureState, ref TypeListINT);
      for (int i = 0; i < TypeListINT.array.Length; i++)
      {
        res = pCamera.getPixelFormatDescription(formatindex, TypeListINT.array[i], ref CameraPixelFormat);
        if (String.Compare(new_format, CameraPixelFormat.sName) == 0)
        {
          pixelformatindex = TypeListINT.array[i];
          res = pCamera.setPixelFormat(TypeListINT.array[i]);
          break;
        }
      }
    }

    #endregion

    private void initSystem()
    {
      ppSystem = new BGAPI.System[0];
      pCamera = null;
      ppImage = new BGAPI.Image[0];
      listint = new BGAPI.BGAPIX_TypeListINT();
      state = new BGAPI.BGAPI_FeatureState();
      intNumberofbuffer = new BGAPI.BGAPIX_TypeINT();
      imgcallback = new BGAPI.BGAPI_NOTIFY_CALLBACK(CallbackFunction);

      //init the system like shown in bgapi_init example
      res = init_systems(ref system_count, ref ppSystem);
      ReturnError(res, "init_systems");

    }

    private void ReturnError(int code, string msg)
    {
      if (code != BGAPI.Result.OK)
      {
        MessageBox.Show(string.Format(msg + "returned with errorcode {0}\n", code));
      }
    }

    private void prepareCamera()
    {
      res = pCamera.getDataAccessMode(ref state, ref listint, ref intNumberofbuffer);
      ReturnError(res, "getDataAccessMode");
      /*
      if (res != BGAPI.Result.OK)
      {
        MessageBox.Show(string.Format("getDataAccessMode returned with errorcode {0}\n", res));
      }*/

      for (i = 0; i < listint.length; i++)
      {
        //MessageBox.Show(string.Format("{0} select DataAccessMode {1}\n", i, ((BGAPI_DataAccessMode)listint.array[i]).ToString()));
        //externppSystem[i].releaseCamera( ref externppCamera );
      }

      do
      {
        inputVal = 0;
        //inputVal = Convert.ToInt32(Console.ReadLine(), 10);
      }
      while (inputVal < 0 || inputVal > listint.length);
      if (((BGAPI_DataAccessMode)listint.array[inputVal]) == BGAPI_DataAccessMode.BGAPI_DATAACCESSMODE_NORMALEXTERN ||
      ((BGAPI_DataAccessMode)listint.array[inputVal]) == BGAPI_DataAccessMode.BGAPI_DATAACCESSMODE_QUEUEDEXTERN)
      {
        MessageBox.Show(string.Format("DataAccessMode {0} is not supported by this example!\n", ((BGAPI_DataAccessMode)listint.array[inputVal]).ToString()));
        res = release_systems(system_count, ref ppSystem);
        ReturnError(res, "release_systems");
        /*
        if (res != BGAPI.Result.OK)
        {
          MessageBox.Show(string.Format("release_systems Errorcode: {0}\n", res));
        }
         */
        return;
      }
      res = pCamera.setDataAccessMode((BGAPI_DataAccessMode)listint.array[inputVal], intNumberofbuffer.current);
      ReturnError(res, "setDataAccessMode");
      /*
      if (res != BGAPI.Result.OK)
      {
        MessageBox.Show(string.Format("setDataAccessMode returned with errorcode {0}\n", res));
      }
      */

      res = pCamera.getDataAccessMode(ref state, ref listint, ref intNumberofbuffer);
      ReturnError(res, "getDataAccessMode");
      /*
      if (res != BGAPI.Result.OK)
      {
        MessageBox.Show(string.Format("getDataAccessMode returned with errorcode {0}\n", res));
      }
      */
      if ((((BGAPI_DataAccessMode)listint.array[listint.current]) == BGAPI_DataAccessMode.BGAPI_DATAACCESSMODE_NORMALINTERN) || (((BGAPI_DataAccessMode)listint.array[listint.current]) == BGAPI_DataAccessMode.BGAPI_DATAACCESSMODE_NORMALEXTERN))
      {
        ppImage = new BGAPI.Image[1];

        //create image object
        res = BGAPI.EntryPoint.createImage(ref ppImage[image_count]);
        ReturnError(res, "createImage for Image 0");
        /*
        if (res != BGAPI.Result.OK)
        {
          MessageBox.Show(string.Format("createImage for Image 0 returned with errorcode {0}\n", res));
        }*/
        image_count++;

        if (((BGAPI_DataAccessMode)listint.array[listint.current]) == BGAPI_DataAccessMode.BGAPI_DATAACCESSMODE_NORMALEXTERN)
        {
          //not yet supported
        }

        res = pCamera.setImage(ref ppImage[image_count - 1]);
        ReturnError(res, "setImage for Image 0");
        /*
        if (res != BGAPI.Result.OK)
        {
          MessageBox.Show(string.Format("setImage for Image 0 returned with errorcode {1}\n", res));
        }*/



      }
      if ((((BGAPI_DataAccessMode)listint.array[listint.current]) == BGAPI_DataAccessMode.BGAPI_DATAACCESSMODE_QUEUEDINTERN) || (((BGAPI_DataAccessMode)listint.array[listint.current]) == BGAPI_DataAccessMode.BGAPI_DATAACCESSMODE_QUEUEDEXTERN))
      {
        ppImage = new BGAPI.Image[intNumberofbuffer.current];

        //create image objects
        for (i = 0; i < intNumberofbuffer.current; i++)
        {
          res = BGAPI.EntryPoint.createImage(ref ppImage[image_count]);
          ReturnError(res, "createImage for Image");
          /*
          if (res != BGAPI.Result.OK)
          {
            MessageBox.Show(string.Format("createImage for Image {0} returned with errorcode {1}\n", i, res));
            break;
          }
           * */
          image_count++;

          if (((BGAPI_DataAccessMode)listint.array[listint.current]) == BGAPI_DataAccessMode.BGAPI_DATAACCESSMODE_QUEUEDEXTERN)
          {
            //not yet supported
          }

          res = pCamera.setImage(ref ppImage[image_count - 1]);
          ReturnError(res, "setImage for Image");
          /*
          if (res != BGAPI.Result.OK)
          {
            MessageBox.Show(string.Format("setImage for Image {0} returned with errorcode {1}\n", i, res));
            break;
          }*/
        }
      }

      //--------------------------------------
      
      //this command will set a gainvalue
      res = pCamera.setGain(gainvalue);
      ReturnError(res, "setGain");
      /*
    
      if (res != BGAPI.Result.OK)
      {
        System.Console.Write("setGain Errorcode: {0}\n", res);
      }*/

      //this command will set an exposurevalue
      res = pCamera.setExposure(exposurevalue);
      ReturnError(res, "setExposure");
      /*
      if (res != BGAPI.Result.OK)
      {
        System.Console.Write("setExposure Errorcode: {0}\n", res);
      }
       * */


      //--------------------------------------
      //this command will get the current imageformat
      res = pCamera.getImageFormat(ref state, ref listint);
      ReturnError(res, "getImageFormat");
      /*
      if (res != BGAPI.Result.OK)
      {
        System.Console.Write("getImageFormat Errorcode: {0}\n", res);
      }
      */

      //this command will set an imageformat
      res = pCamera.setImageFormat(formatindex);
      ReturnError(res, "setImageFormat");
      /*
      if (res != BGAPI.Result.OK)
      {
        System.Console.Write("setImageFormat Errorcode: {0}\n", res);
      }
      */
      res = pCamera.getPixelFormat(7, ref FeatureState, ref TypeListINT);
      res = pCamera.setPixelFormat(TypeListINT.array[0]);
     //res = formatlist.array[1];

      res = pCamera.registerNotifyCallback(pCamera, imgcallback);
      if (res != BGAPI.Result.OK)
      {
        MessageBox.Show(string.Format("Error {0} while register NotifyCallback.\n", res));
      }

      res = pCamera.setStart(true);
      ReturnError(res, "setStart");
      /*
      if (res != BGAPI.Result.OK)
      {
        MessageBox.Show(string.Format("setStart returned with errorcode {0}\n", res));
      }
       */
    }

    private void initCamera()
    {
      //select a camera to use
      res = init_camera(system_count, ref ppSystem, ref currSystem, ref pCamera);
      ReturnError(res, "init_camera");
      /*
      if (res != BGAPI.Result.OK)
      {
        MessageBox.Show(string.Format("init_camera Errorcode: {0}\n", res));
      }
       */
    }

    public void Connect()
    {
      CameraConnectionTask = new Task(ConnectProcessing);
      CameraConnectionTask.Start();

    }

    private void ConnectProcessing()
    {
      initSystem();
      initCamera();
      prepareCamera();
    }

    void IDisposable.Dispose()
    {
      res = release_systems(system_count, ref ppSystem);
      ReturnError(res, "release_systems");
      /*
      if (res != BGAPI.Result.OK)
      {
        MessageBox.Show(string.Format("release_systems Errorcode: {0}\n", res));
      }*/
      res = release_images(image_count, ref ppImage);
      ReturnError(res, "release_images");
      /*
      if (res != BGAPI.Result.OK)
      {
        MessageBox.Show(string.Format("release_images Errorcode: {0}\n", res));
      }*/
    }


    public void Kill()
    {
      Process.GetCurrentProcess().Kill();
      //Thread.CurrentThread.Abort();
    }


    #region 100% works lib-like functions

    private static int init_systems(ref int system_count, ref BGAPI.System[] externppSystem)
    {
      int res = BGAPI.Result.FAIL;
      int i = 0;

      //this is the base call to find the bgapi_system modules which are necessary to perform any further action
      //every BGPAPI_Function returns a BGAPI_RESULT
      res = BGAPI.EntryPoint.countSystems(ref system_count);

      //You should always check the result to make sure everything works fine
      if (res != BGAPI.Result.OK)
      {
        //in case of error you will get a result different from BGAPI.Result.OK 
        //all resultcodes are defined in bgapiresult.h and are returned for special reasons
        MessageBox.Show(string.Format("BGAPI.EntryPoint.countSystems Errorcode: {0} system_count {1}\n", res, system_count));
        return res;
      }

      //this is an example how to store pointers for all available systems 
      externppSystem = new BGAPI.System[system_count];

      for (i = 0; i < system_count; i++)
      {
        res = BGAPI.EntryPoint.createSystem(i, ref externppSystem[i]);
        if (res != BGAPI.Result.OK)
        {
          MessageBox.Show(string.Format("BGAPI.EntryPoint.createSystem Errorcode: {0} Systemnumber {1}\n", res, i));
          return res;
        }
        res = externppSystem[i].open();
        if (res != BGAPI.Result.OK)
        {
          MessageBox.Show(string.Format("open Systemnumber {0} Errorcode: {1}\n", i, res));
          return res;
        }
      }
      return res;
    }

    private static int init_camera(int system_count, ref BGAPI.System[] externppSystem, ref int pCurrSystem, ref BGAPI.Camera externppCamera)
    {
      int res = BGAPI.Result.FAIL;
      int i = 0;
      int cam = 0;
      int[] cameras = new int[system_count];
      int camera_count = 0;
      BGAPI.BGAPI_FeatureState state = new BGAPI_FeatureState();
      BGAPI.BGAPIX_CameraInfo cameradeviceinfo = new BGAPIX_CameraInfo();
      int inputVal = 0;

      for (i = 0; i < system_count; i++)
      {
        //this is an example how to count available cameras for all available systems
        res = externppSystem[i].countCameras(ref cameras[i]);
        if (res != BGAPI.Result.OK)
        {
          MessageBox.Show(string.Format("countCameras Systemnumber {0} Errorcode: {1}\n", i, res));
          return 0;
        }

        for (cam = 0; cam < cameras[i]; cam++)
        {
          camera_count++;

          //this is an example how to create a camera
          res = externppSystem[i].createCamera(cam, ref externppCamera);
          if (res != BGAPI.Result.OK)
          {
            MessageBox.Show(string.Format("\n"));
            MessageBox.Show(string.Format("createCamera Systemnumber {0} Errorcode: {1}\n", i, res));
            return res;
          }

          //this is an example how to get the device information for a camera
          res = externppCamera.getDeviceInformation(ref state, ref cameradeviceinfo);
         
          if (res != BGAPI.Result.OK)
          {
            MessageBox.Show(string.Format("\n"));
            MessageBox.Show(string.Format("getDeviceInformation Errorcode: {0}\n", res));
            return 0;
          }
          MessageBox.Show(string.Format("{0} select Camera {1} of system {2} - {3} SN: {4}\n", camera_count, cam, i, cameradeviceinfo.modelName, cameradeviceinfo.serialNumber));
          externppSystem[i].releaseCamera(ref externppCamera);
        }
      }

      do
      {
        //[TODO] CUSTOM SELECTOR
        inputVal = 1;
        //inputVal = Convert.ToInt32(Console.ReadLine(), 10);
      }
      while (inputVal < 0 || inputVal > camera_count);

      camera_count = 0;
      for (i = 0; i < system_count; i++)
      {
        for (cam = 0; cam < cameras[i]; cam++)
        {
          camera_count++;

          if (camera_count == inputVal)
          {
            pCurrSystem = i;

            //this is an example how to create a camera
            res = externppSystem[pCurrSystem].createCamera(cam, ref externppCamera);
            if (res != BGAPI.Result.OK)
            {
              MessageBox.Show(string.Format("createCamera Systemnumber {0} Errorcode: {1}\n", i, res));
              return res;
            }

            //this is an example how to open a camera
            res = externppCamera.open();
            if (res != BGAPI.Result.OK)
            {
              MessageBox.Show(string.Format("open Systemnumber {0} Errorcode: {1}\n", i, res));
              return res;
            }
            break;
          }
        }
      }
      return res;
    }

    private static int release_systems(int system_count, ref BGAPI.System[] ppSystem)
    {
      int res = BGAPI.Result.FAIL;
      int i = 0;

      for (i = 0; i < system_count; i++)
      {
        res = ppSystem[i].release();
        if (res != BGAPI.Result.OK)
        {
          MessageBox.Show(string.Format("release {0} Errorcode: {1}\n", i, res));
        }
        ppSystem[i] = null;
      }
      return res;
    }

    private static int release_images(int image_count, ref BGAPI.Image[] ppImage)
    {
      int res = BGAPI.Result.FAIL;
      int i = 0;

      for (i = 0; i < image_count; i++)
      {
        res = BGAPI.EntryPoint.releaseImage(ref ppImage[i]);
        if (res != BGAPI.Result.OK)
        {
          MessageBox.Show(string.Format("release {0} Errorcode: {1}\n", i, res));
        }
        ppImage[i] = null;
      }
      return res;
    }

    #endregion
  }
}
