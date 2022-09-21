using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
 
public class Test : MonoBehaviour
{
    private static Test _instance;
    private string ScreenShotPath = Application.dataPath.Replace('/', '\\') + "\\GameScreenShot";
 
    public static Test Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Test();
            }
            return _instance;
        }
    }
 
    /// <summary>
    /// 打开项目
    /// </summary>
    public void OpenProject(Action luafunc, string defaultPath)
    {
        if (!System.IO.Directory.Exists(ScreenShotPath))
        {
            System.IO.Directory.CreateDirectory(ScreenShotPath);
        }
        defaultPath = ScreenShotPath;
        Action<Action, string> _OpenProject = OpenOpenFileDlg;
        _OpenProject.BeginInvoke(luafunc, defaultPath, null, null);
    }
 
    private void OpenOpenFileDlg(Action luafunc, string defaultPath)
    {
        OpenFileDlg pth = new OpenFileDlg();
        pth.structSize = Marshal.SizeOf(pth);
        //pth.filter = "All files (*.*)|*.*";
        pth.filter = "图片文件(*.jpg,*.png)\0*.jpg;*.png";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = defaultPath; //默认路径//@"%userprofile%\Pictures";//@"D:\";//Application.dataPath.Replace("/", "\\") + "\\Resources"; //默认路径
        pth.title = "选择文件";
        pth.defExt = "png";//显示文件的类型
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        pth.dlgOwner = OpenFileDialog.GetForegroundWindow();
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            string filepath = pth.file; //选择的文件路径; 
            string filename = pth.fileTitle;
            if (luafunc != null)
            {
                luafunc();
            }
        }
        else
        {
            if (luafunc != null)
            {
                luafunc();
            }
        }
    }
 
    /// <summary>
    /// 保存文件项目
    /// </summary>
    public void SaveProject()
    {
        try
        {
            OpenFileDlg pth = new OpenFileDlg();
            pth.structSize = Marshal.SizeOf(pth);
            pth.filter = "*.jpg|*.png";
            pth.file = new string(new char[256]);
            pth.maxFile = pth.file.Length;
            pth.fileTitle = new string(new char[64]);
            pth.maxFileTitle = pth.fileTitle.Length;
            pth.initialDir = Application.dataPath; //默认路径
            pth.title = "保存项目";
            //pth.defExt = "dat";
            pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
            pth.dlgOwner = OpenFileDialog.GetForegroundWindow();
            if (OpenFileDialog.GetSaveFileName(pth))
            {
                string filepath = pth.file; //选择的文件路径;  
                Debug.Log(filepath);
            }
        }
        catch (Exception)
        {
        }
    }
 
 
 
    public void ScreenShot()
    {
        if (!System.IO.Directory.Exists(ScreenShotPath))
        {
            System.IO.Directory.CreateDirectory(ScreenShotPath);
        }
        string filename = "/" + DateTime.Now.ToString("yyyyMdHms") + ".png";
        UnityEngine.ScreenCapture.CaptureScreenshot(ScreenShotPath + filename);
    }
 
}
 
[StructLayout(LayoutKind.Sequential,CharSet = CharSet.Auto)]
public class OpenFileDlg
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}
 
public class OpenFileDialog
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileDlg ofd);
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileDlg ofd);
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetForegroundWindow();
}

