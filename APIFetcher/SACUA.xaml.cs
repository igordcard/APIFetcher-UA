using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;

namespace APIFetcher
{
    public partial class SACUA : Window
    {
        private String version;

        private TicketService ts;
        private DispatcherTimer dt;
        private BackgroundWorker bw;

        public SACUA()
        {
            InitializeComponent();
        }

        private void readyUI()
        {
            TheGrid.Background = Brushes.Transparent;
            Loading.Visibility = Visibility.Collapsed;
            UAImage.Visibility = Visibility.Hidden;
            TheGrid.RowDefinitions.RemoveRange(1, 5);
        }

        private void hideTitleLine()
        {
            TicketTitle.Visibility = Visibility.Collapsed;
            ServiceTitle.Visibility = Visibility.Collapsed;
            QueueTitle.Visibility = Visibility.Collapsed;
            WaitTimeTitle.Visibility = Visibility.Collapsed;
            BalconyTitle.Visibility = Visibility.Collapsed;
            StateTitle.Visibility = Visibility.Collapsed;
        }

        private void showTitleLine()
        {
            TicketTitle.Visibility = Visibility.Visible;
            ServiceTitle.Visibility = Visibility.Visible;
            QueueTitle.Visibility = Visibility.Visible;
            WaitTimeTitle.Visibility = Visibility.Visible;
            BalconyTitle.Visibility = Visibility.Visible;
            StateTitle.Visibility = Visibility.Visible;
        }

        private TextBlock createFooterText(String name, String text, bool isLink)
        {
            TextBlock newText = new TextBlock();
            newText.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            newText.TextAlignment = TextAlignment.Center;
            newText.Foreground = Brushes.DarkGreen;
            newText.Background = Brushes.GreenYellow;
            newText.FontSize = 12;
            newText.Name = name;
            newText.Text = text;
            if (isLink)
            {
                newText.Cursor = Cursors.Hand;
                newText.Foreground = Brushes.OliveDrab;
                newText.MouseLeftButtonDown += new MouseButtonEventHandler(Link_MouseLeftButtonDown);
            }

            return newText;
        }

        private void updateUI()
        {
            List<Ticket> tickets = ts.GetTicketList();

            int line = 1;
            foreach (Ticket ticket in tickets)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.Height = new GridLength(1, GridUnitType.Star);
                TheGrid.RowDefinitions.Add(newRow);

                TextBlock tb = new TextBlock();

                tb.Name = "ticket" + line;
                tb.Text = ticket.LatestNumber.ToString();
                setTicketLineProperties(tb);
                tb.FontSize = 16;
                tb.FontWeight = FontWeights.Bold;
                addToGrid(tb, line, 0, 1, 1);

                tb = new TextBlock();
                tb.Name = "Service" + line;
                tb.Text = ticket.Description;
                setTicketLineProperties(tb);
                addToGrid(tb, line, 1, 1, 1);

                tb = new TextBlock();
                tb.Name = "Queue" + line;
                tb.Text = ticket.WaitQueue.ToString();
                setTicketLineProperties(tb);
                addToGrid(tb, line, 2, 1, 1);

                tb = new TextBlock();
                tb.Name = "Wait" + line;
                tb.Text = ticket.WaitTime.ToString();
                setTicketLineProperties(tb);
                addToGrid(tb, line, 3, 1, 1);

                tb = new TextBlock();
                tb.Name = "Balcony" + line;
                tb.Text = ticket.BalconyTime.ToString();
                setTicketLineProperties(tb);
                addToGrid(tb, line, 4, 1, 1);

                tb = new TextBlock();
                tb.Name = "State" + line;
                setTicketLineProperties(tb);
                if (ticket.Enabled)
                {
                    tb.Foreground = Brushes.Green;
                    if(ticket.HasInfo())
                        tb.Text = ticket.Info;
                    else
                        tb.Text = "Em funcionamento";
                }
                else if (!ticket.Enabled)
                {
                    tb.Foreground = Brushes.Red;
                    if (ticket.HasInfo())
                        tb.Text = ticket.Info;
                    else
                        tb.Text = "Bloqueado a novas senhas";
                }

                addToGrid(tb, line, 5, 1, 1);

                line++;
            }

            if (line == 1)
            {
                RowDefinition spaceRow = new RowDefinition();
                spaceRow.Height = new GridLength(1, GridUnitType.Star);
                TheGrid.RowDefinitions.Add(spaceRow);

                TextBlock tb = new TextBlock();
                tb.Name = "wait" + line;
                tb.Text = "O serviço não está disponível";
                setTicketLineProperties(tb);
                tb.FontSize = 48;
                tb.Foreground = Brushes.Gray;
                addToGrid(tb, line, 0, 1, 6);

                hideTitleLine();

                line++;
            }
            else
                showTitleLine();

            addFooterToUI(line);
        }

        private void addToGrid(UIElement element, int row, int column, int rspan, int cspan)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            Grid.SetRowSpan(element, rspan);
            Grid.SetColumnSpan(element, cspan);
            TheGrid.Children.Add(element);
        }

        private void clearUI()
        {
            TheGrid.Children.RemoveRange(6, TheGrid.Children.Count - 6);
            TheGrid.RowDefinitions.RemoveRange(1, TheGrid.RowDefinitions.Count - 1);
        }

        private void addFooterToUI(int line)
        {
            RowDefinition statusRow = new RowDefinition();
            statusRow.Height = new GridLength(20, GridUnitType.Pixel);
            TheGrid.RowDefinitions.Add(statusRow);
            TextBlock updateText = createFooterText("UpdateText", "Última actualização: " + ts.LastUpdate.ToString(), false);
            updateText.Background = new SolidColorBrush(Color.FromRgb(220,255,150));
            updateText.FontWeight = FontWeights.Bold;
            addToGrid(updateText, line++, 0, 1, 6);

            RowDefinition aboutRow = new RowDefinition();
            aboutRow.Height = new GridLength(20, GridUnitType.Pixel);
            TheGrid.RowDefinitions.Add(aboutRow);

            TextBlock hyperLink = createFooterText("HyperLink", "igordcard.blogspot.com", true);
            addToGrid(hyperLink, line, 0, 1, 2);
            TextBlock versionText = createFooterText("VersionText", version, false);
            addToGrid(versionText, line, 2, 1, 2);
            TextBlock apiLink = createFooterText("ApiLink", "api.web.ua.pt", true);
            addToGrid(apiLink, line, 4, 1, 2);
        }

        private void setTicketLineProperties(TextBlock tb)
        {
            tb.FontSize = 14;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            version =
            "SACUA v" +
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major +
            "." +
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor +
            " ";

            ts = new TicketService();

            // Only execute the update after the Window has been rendered.
            Dispatcher.BeginInvoke(new Action(delegate
            {
                ts.InterpretXML();
                readyUI();
                updateUI();
                this.Height = 300 + (TheGrid.RowDefinitions.Count - 5) * 64;
            }), System.Windows.Threading.DispatcherPriority.ContextIdle, null);

            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 5);
            dt.Tick += new EventHandler(dt_Tick);
            dt.Start();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            clearUI();
            updateUI();
            dt.Start();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            ts.InterpretXML();
        }

        private void Link_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://" + ((TextBlock)sender).Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dt.Stop();
            dt.Dispatcher.InvokeShutdown();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            try
            {
                dt.Stop();
                bw.RunWorkerAsync();
            }
            catch (InvalidOperationException)
            {
                // Probably due to lack of server response.. skipping this attempt.
            }
        }
    }
}
