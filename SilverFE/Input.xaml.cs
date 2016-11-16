using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Npgsql;


namespace SilverFE
{
    /// <summary>
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input :MetroWindow
    {
        public Input()
        {
            InitializeComponent();
        }
        NpgsqlConnection conn = new NpgsqlConnection("Server=murky.freeborough.com;Port=5432;User Id=connor;Password=poop123;Database=silver");

        private void btInsert_Click(object sender, RoutedEventArgs e)
        {
            Insert();
        }


        #region Connection Stuff
        public void Openconn()
        {
            try
            {
                conn.Open();
            }
            catch (Exception Error)
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
            catch (Exception Error)
            {
                MessageBox.Show(Error.ToString());
            }
        }

        #endregion


        private void Insert()
        {
            try
            {
                Decimal Buy = Decimal.Parse(tbBuy.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                Decimal Sell = Decimal.Parse(tbSell.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                Decimal Profit = Sell - Buy;

                this.Openconn();
                string sql = "INSERT INTO " + cbTable.Text.ToString() + "(Type,Year,Qty,Mint,Buy,Sell,Profit) VALUES('" + tbType.Text.ToString() + "','" + tbYear.Text.ToString() + "','" + tbQty.Text.ToString() + "','" + cbMint.Text.ToString() + "','"+Buy+"','"+Sell+"','"+Profit+"')";
                NpgsqlCommand InsertCommand = new NpgsqlCommand(sql, conn);
                int Success = InsertCommand.ExecuteNonQuery();
                
                if(Success >= 1)
                {
                    MessageBox.Show("Sucess");
                }
                else
                {
                    MessageBox.Show("Something went wrong :(");
                }
                Closeconn();
            }
            catch(Exception Error)
            {
                MessageBox.Show(Error.ToString(), "Insert Failed");
            }    
        }

        private void tbYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                this.Openconn();
                string sql = "DELETE FROM "+cbTable.Text.ToString()+" WHERE id = "+tbID.Text.ToString()+"";
                NpgsqlCommand deleteCommand = new NpgsqlCommand(sql, conn);
                int Success = deleteCommand.ExecuteNonQuery();
                if (Success >= 1)
                {
                    MessageBox.Show("Successfully deleted row " +tbID.Text.ToString() + " from the table", "Delete Command Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {

                    MessageBox.Show("Something went wrong :( \n Double check the Id is correct", "Delete Command Result", MessageBoxButton.OK, MessageBoxImage.Warning);
                }


            }
            catch(Exception Error)
            {
                ApplicationException error = new ApplicationException(Error.ToString());
                throw error;
            }
        }
       
         
        }
    }









