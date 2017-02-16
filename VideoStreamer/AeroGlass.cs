using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace VideoStreamer {

    public partial class MainWindow : Window {
        private void Window_Loaded (object sender, RoutedEventArgs e) {
            AeroGlass.EnableBlur(this);
        }

        private Point _mousePos;

        private void Rectangle_MouseLeftButtonDown (object sender, MouseButtonEventArgs e) {
            Window window = Application.Current.MainWindow;

            if (e.ChangedButton != MouseButton.Left) return;

            if (window.WindowState == WindowState.Maximized) {
                window.WindowState = WindowState.Normal;
                window.Left = _mousePos.X - window.Width / 2;
                window.Top = _mousePos.Y - 20;
            }

            window.DragMove();
        }

        private void Window_LocationChanged (object sender, EventArgs e) {
            Window window = Application.Current.MainWindow;

            _mousePos = Mouse.GetPosition(this);
            label.Content = "Mouse X: " + PointToScreen(_mousePos).X + "\nMouse Y: " + PointToScreen(_mousePos).Y;
        }
    }

    internal static class AeroGlass {
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute (IntPtr hwnd, ref WindowCompositionAttributeData data);

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute {
            // ...
            WCA_ACCENT_POLICY = 19
            // ...
        }

        internal enum AccentState {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        internal static void EnableBlur (Window window) {
            WindowInteropHelper windowHelper = new WindowInteropHelper(window);

            AccentPolicy accent = new AccentPolicy();
            int accentStructSize = Marshal.SizeOf(accent);
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            WindowCompositionAttributeData data = new WindowCompositionAttributeData {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }
    }
}
