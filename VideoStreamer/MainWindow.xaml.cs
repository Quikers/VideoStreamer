using System;
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
            GetSearch("the walking dead");
        }

        private void GetSearch(string search, int page = 0, int category = TorrentCategory.All, QueryOrder queryOrder = QueryOrder.BySeeds) {
            List<Torrent> torrentList = Tpb.Search(
                new Query(
                    search,
                    page,
                    category,
                    queryOrder
                )
            ).ToList();

            foreach (Torrent t in torrentList) {
                label.Content = t.File;
            }

            label.Content = torrentList[0].Name + "\nOK";
        }

    }
}
