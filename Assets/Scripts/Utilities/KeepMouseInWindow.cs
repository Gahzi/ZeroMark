using System.Runtime.InteropServices;
using System.Collections;
using System;
using UnityEngine;

public class KeepMouseInWindow : MonoBehaviour
{
#if UNITY_STANDALONE_WIN

    [DllImport("user32.dll")]
    private static extern bool ClipCursor(ref RECT lpRect);

    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);

    public static void SetPosition(int x, int y, int resX = 0, int resY = 0)
    {
        SetWindowPos(FindWindow(null, "Untitled Alpha"), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
    }

#endif

    public int windowPosX;
    public int windowPosY;

    public void Start()
    {
        SetPosition(windowPosX, windowPosY);
        RECT cursorLimits;
        cursorLimits.Left = windowPosX;
        cursorLimits.Top = windowPosY;
        cursorLimits.Right = Screen.width - 1 + windowPosX;
        cursorLimits.Bottom = Screen.height - 1 + windowPosY;
        ClipCursor(ref cursorLimits);
    }

}