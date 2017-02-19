﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using WinInterop = System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThePirateBay;
using Application = System.Windows.Application;

namespace VideoStreamer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            SourceInitialized += win_SourceInitialized;

            GetSearch("the walking dead");
        }

        void win_SourceInitialized(object sender, EventArgs e) {
            IntPtr handle = new WinInterop.WindowInteropHelper(this).Handle;
            WinInterop.HwndSource.FromHwnd(handle).AddHook(WindowProc);
        }

        List<Label> labelList = new List<Label>();

        private void GetSearch(string search, int page = 0, int category = TorrentCategory.All, QueryOrder queryOrder = QueryOrder.BySeeds) {
            List<Torrent> torrentList = Tpb.Search(
                new Query(
                    search,
                    page,
                    category,
                    queryOrder
                )
            ).ToList();

            foreach (Label label in labelList)
                WindowContent.Children.Remove(label);

            labelList.Clear();
            foreach (Torrent t in torrentList) {
                labelList.Add(
                    new Label {
                        Content = t.Name,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(10, labelList.Count * 35, 0, 0),
                        Foreground = Brushes.White,
                        FontWeight = FontWeight.FromOpenTypeWeight(labelList.Count == 0 ? 1 : 999)
                    }
                );

                AddUIElementToPanel(labelList[labelList.Count - 1], WindowContent);
            }
        }
    }
}
