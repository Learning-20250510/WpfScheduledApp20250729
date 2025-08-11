using System;
using System.Windows.Input;

namespace WpfScheduledApp20250729.Interfaces
{
    public interface IGlobalHotKeyService
    {
        bool RegisterHotKey(IntPtr windowHandle, int hotkeyId, ModifierKeys modifiers, Key key);
        bool UnregisterHotKey(IntPtr windowHandle, int hotkeyId);
        void ProcessHotKeyMessage(IntPtr wParam);
        event EventHandler<HotKeyPressedEventArgs> HotKeyPressed;
        void SetupHotKeys(IntPtr windowHandle);
        void Cleanup(IntPtr windowHandle);
    }

    public class HotKeyPressedEventArgs : EventArgs
    {
        public int HotKeyId { get; set; }
        public string HotKeyName { get; set; }
        
        public HotKeyPressedEventArgs(int hotKeyId, string hotKeyName)
        {
            HotKeyId = hotKeyId;
            HotKeyName = hotKeyName;
        }
    }
}