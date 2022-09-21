using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

public class OpenFile : MonoBehaviour
{
    public Button button;//按钮
    public Transform text;//text预设体
    public Transform parent;//存放预设体父容器
    void Awake()
    {
        button.onClick.AddListener(OpenFileWin);//按钮事件添加
    }
    public void OpenFileWin()
    {
        //初始化
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "All Files\0*.*\0\0";
        ofn.file = new string(new char[1024]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        ofn.initialDir = path;  //默认路径
        ofn.title = "Open Project";
        ofn.defExt = "JPG";//显示文件的类型  
        //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  

        //判断是否打开文件
        if (WindowDll.GetOpenFileName(ofn))
        {
            //多选文件
            string[] Splitstr = { "\0" };
            string[] strs = ofn.file.Split(Splitstr,StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length>1)
            {
                for (int i = 1; i < strs.Length; i++)
                {
                    Transform item = Instantiate(text, parent);
                    item.gameObject.SetActive(true);
                    item.GetComponent<Text>().text = strs[0] + "\\" + strs[i];
                    Debug.Log(strs[0] + "\\" + strs[i]);
                }
            }
            else
            {
                Transform item = Instantiate(text, parent);
                item.gameObject.SetActive(true);
                item.GetComponent<Text>().text = strs[0];
                Debug.Log(strs[0]);
            }
        }
        else
        {
            Debug.LogFormat("路径为空");
            
        }
        
    }

}

