using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using Npgsql;
using MahApps.Metro.Controls;
using System.Text.RegularExpressions;

namespace SilverFE
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : MetroWindow
    {
        public Main()
        {
            InitializeComponent();
        }
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
      


        NpgsqlConnection conn = new NpgsqlConnection("Server=murky.freeborough.com;Port=5432;User Id=connor;Password=poop123;Database=silver;");

        #region Connection Stuff
        public void Openconn()
        {
            try
            {
                conn.Open();
            }
            catch(Exception Error)
            {
                MessageBox.Show(Error.ToString());
            }
        }

        public void Closeconn()
        {
            try
            {
                conn.Close();
            }
            catch(Exception Error)
            {
                MessageBox.Show(Error.ToString());
            }
        }

        #endregion



        private void btShow_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if(cbSortType.Text == "All")
                {

                   
                    this.Openconn();
                    string sql = "SELECT * FROM " + cbTable.Text.ToString() + ";";
                    TableWindow.Title = "" + cbTable.Text.ToString() + "";
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql,conn);
                    ds.Reset();
                    da.Fill(ds);
                    dt = ds.Tables[0];
                    dgSilver.ItemsSource = dt.DefaultView;
                    this.Closeconn();
                }

                if (cbSortType.Text != "All" && tbSearch.Text.ToString() != "")
                {
                    this.Openconn();
                    string sql = "SELECT * FROM " + cbTable.Text.ToString() + " WHERE " + cbSortType.Text.ToString() + " = '" + tbSearch.Text.ToString() + "'";
                    TableWindow.Title = "" + cbTable.Text.ToString() + "";
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                    ds.Reset();
                    da.Fill(ds);
                    dt = ds.Tables[0];
                    dgSilver.ItemsSource = dt.DefaultView;
                    this.Closeconn();
                }

            }
            catch(Exception Error)
            {
                MessageBox.Show(Error.ToString());
            }
        }

        private void dgSilver_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (cbSortType.Text != "All" && tbSearch.Text.ToString() != "")
                {
                    this.Openconn();
                    string sql = "SELECT * FROM " + cbTable.Text.ToString() + " WHERE " + cbSortType.Text.ToString() + " = '" + tbSearch.Text.ToString() + "'";
                    TableWindow.Title = "" + cbTable.Text.ToString() + "";
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                    ds.Reset();
                    da.Fill(ds);
                    dt = ds.Tables[0];
                    dgSilver.ItemsSource = dt.DefaultView;
                    this.Closeconn();
                }
            }
            catch(Exception Error)
            {
                MessageBox.Show(Error.ToString());
                Closeconn();
            }
        }

        private void tbSearch_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if(cbSortType.Text != "All" || cbSortType.Text != "Type" || cbSortType.Text != "Mint")
            {
                Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$"); 
                e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
            }
        }
    }
}
