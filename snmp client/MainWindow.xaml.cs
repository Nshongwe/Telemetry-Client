using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Pocos;
using Service;
using snmp_client.Services;

namespace snmp_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MonitorNetwork _monitorNetwork;
        private SetMonitors _setMonitors;
        private ILogService _logService;
        private IOIDService _ioidService;
        private List<OIDModel> oids;
        private int _count;
        private OIDModel _oidItem;
        public MainWindow()
        {
            InitializeComponent();
            _monitorNetwork = new MonitorNetwork();
            _setMonitors =new SetMonitors();
            _logService = new LogService();
            _ioidService = new OIDService();
            SetupTimer();
            BindDataGrid();
            BindItems();
        }

        private void BindItems()
        {
            oids = _ioidService.GetAllOiDs().FindAll(x=>x.Item.Equals("sysLocation"));
            cmbItems.ItemsSource = oids;
        }
        private void SetupTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer(30000);
            timer.Start();
            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            timer.Elapsed += (sender, e) =>
            {
                if (!worker.IsBusy)
                {
                    _count++;
                    worker.RunWorkerAsync();
                }
            };
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _monitorNetwork.WritePackets(_count % 2 == 0);

            }
            catch (Exception exception)
            {
                //ToDo write this to system log and print friendly error message to screen
                var ex = exception.Message;
                MessageBox.Show("Oops something went wrong");
            }
        }

        private void BindDataGrid()
        {
            var searchResults = _logService.Search(txtSearch.Text);
            dgLog.ItemsSource = searchResults;

        }

        private void TxtSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchResults = _logService.Search(txtSearch.Text);
            dgLog.ItemsSource = searchResults;
        }

        private void cmbItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _oidItem = oids.FirstOrDefault(x => x.OIDKey == Convert.ToInt32(cmbItems.SelectedValue));
            if (_oidItem != null) txtNewValue.Text = _oidItem.OID1;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var status = _setMonitors.SetRequest(_oidItem, txtNewValue.Text);
                if (status == null || status.Pdu.ErrorStatus != 0)
                {
                    MessageBox.Show("An error occurred");
                }
                else
                {
                    MessageBox.Show("OID Updated");
                }

            }
            catch (Exception exception)
            {
                //ToDo write this to system log and print friendly error message to screen
                var ex = exception.Message;
                MessageBox.Show("An error occurred");
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //_monitorNetwork = new MonitorNetwork();
        }
    }
}
