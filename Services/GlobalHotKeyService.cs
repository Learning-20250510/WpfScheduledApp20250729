using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;
using WpfScheduledApp20250729.Interfaces;

namespace WpfScheduledApp20250729.Services
{
    public class GlobalHotKeyService : IGlobalHotKeyService
    {
        private const int WM_HOTKEY = 0x0312;
        
        // ホットキーID定義
        private const int HOTKEY_ACTIVATE_WINDOW = 0x0001;
        private const int HOTKEY_ADD_TASK = 0x0002;
        private const int HOTKEY_MOTIVATION_1 = 0x0005;
        private const int HOTKEY_MOTIVATION_2 = 0x0006;
        private const int HOTKEY_MOTIVATION_3 = 0x0007;
        private const int HOTKEY_MOTIVATION_4 = 0x0008;
        private const int HOTKEY_MOTIVATION_5 = 0x0009;
        private const int HOTKEY_MOTIVATION_6 = 0x0010;
        private const int HOTKEY_MOTIVATION_7 = 0x0011;
        private const int HOTKEY_MOTIVATION_8 = 0x0012;
        private const int HOTKEY_MOTIVATION_9 = 0x0013;
        private const int HOTKEY_MOTIVATION_0 = 0x0014;

        private readonly Dictionary<int, string> _hotKeyNames;

        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, int modKey, int vKey);

        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

        public event EventHandler<HotKeyPressedEventArgs> HotKeyPressed;

        public GlobalHotKeyService()
        {
            _hotKeyNames = new Dictionary<int, string>
            {
                { HOTKEY_ACTIVATE_WINDOW, "ActivateWindow" },
                { HOTKEY_ADD_TASK, "AddTask" },
                { HOTKEY_MOTIVATION_1, "Motivation1" },
                { HOTKEY_MOTIVATION_2, "Motivation2" },
                { HOTKEY_MOTIVATION_3, "Motivation3" },
                { HOTKEY_MOTIVATION_4, "Motivation4" },
                { HOTKEY_MOTIVATION_5, "Motivation5" },
                { HOTKEY_MOTIVATION_6, "Motivation6" },
                { HOTKEY_MOTIVATION_7, "Motivation7" },
                { HOTKEY_MOTIVATION_8, "Motivation8" },
                { HOTKEY_MOTIVATION_9, "Motivation9" },
                { HOTKEY_MOTIVATION_0, "Motivation0" }
            };
        }

        public bool RegisterHotKey(IntPtr windowHandle, int hotkeyId, ModifierKeys modifiers, Key key)
        {
            int vKey = KeyInterop.VirtualKeyFromKey(key);
            return RegisterHotKey(windowHandle, hotkeyId, (int)modifiers, vKey) != 0;
        }

        public bool UnregisterHotKey(IntPtr windowHandle, int hotkeyId)
        {
            return UnregisterHotKey(windowHandle, hotkeyId) != 0;
        }

        public void SetupHotKeys(IntPtr windowHandle)
        {
            // Ctrl+R: ウィンドウアクティベート
            RegisterHotKey(windowHandle, HOTKEY_ACTIVATE_WINDOW, ModifierKeys.Control, Key.R);
            
            // Ctrl+Shift+T: タスク追加
            RegisterHotKey(windowHandle, HOTKEY_ADD_TASK, ModifierKeys.Control | ModifierKeys.Shift, Key.T);
            
            // Ctrl+Shift+1~0: モチベーション追加
            RegisterHotKey(windowHandle, HOTKEY_MOTIVATION_1, ModifierKeys.Control | ModifierKeys.Shift, Key.D1);
            RegisterHotKey(windowHandle, HOTKEY_MOTIVATION_2, ModifierKeys.Control | ModifierKeys.Shift, Key.D2);
            RegisterHotKey(windowHandle, HOTKEY_MOTIVATION_3, ModifierKeys.Control | ModifierKeys.Shift, Key.D3);
            RegisterHotKey(windowHandle, HOTKEY_MOTIVATION_4, ModifierKeys.Control | ModifierKeys.Shift, Key.D4);
            RegisterHotKey(windowHandle, HOTKEY_MOTIVATION_5, ModifierKeys.Control | ModifierKeys.Shift, Key.D5);
            RegisterHotKey(windowHandle, HOTKEY_MOTIVATION_6, ModifierKeys.Control | ModifierKeys.Shift, Key.D6);
            RegisterHotKey(windowHandle, HOTKEY_MOTIVATION_7, ModifierKeys.Control | ModifierKeys.Shift, Key.D7);
            RegisterHotKey(windowHandle, HOTKEY_MOTIVATION_8, ModifierKeys.Control | ModifierKeys.Shift, Key.D8);
            RegisterHotKey(windowHandle, HOTKEY_MOTIVATION_9, ModifierKeys.Control | ModifierKeys.Shift, Key.D9);
            RegisterHotKey(windowHandle, HOTKEY_MOTIVATION_0, ModifierKeys.Control | ModifierKeys.Shift, Key.D0);
        }

        public void ProcessHotKeyMessage(IntPtr wParam)
        {
            int hotkeyId = wParam.ToInt32();
            
            if (_hotKeyNames.ContainsKey(hotkeyId))
            {
                HotKeyPressed?.Invoke(this, new HotKeyPressedEventArgs(hotkeyId, _hotKeyNames[hotkeyId]));
            }
        }

        public void Cleanup(IntPtr windowHandle)
        {
            foreach (var hotkeyId in _hotKeyNames.Keys)
            {
                UnregisterHotKey(windowHandle, hotkeyId);
            }
        }
    }
}